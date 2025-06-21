using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using QuizBiblio;
using QuizBiblio.DataAccess.QbDbContext;
using QuizBiblio.Infrastructure.Configuration;
using QuizBiblio.Infrastructure.Storage;
using QuizBiblio.JobScheduler;
using QuizBiblio.JobScheduler.Authorization;
using QuizBiblio.Middleware;
using QuizBiblio.Models.DatabaseSettings;
using QuizBiblio.Models.Rbac;
using QuizBiblio.Models.Settings;
using QuizBiblio.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // Avoid renaming claims

//shortcuts
var services = builder.Services;
var configuration = builder.Configuration;

string? dbName = configuration.GetValue<string>("QuizStoreDatabase:DatabaseName");

services.Configure<QuizStoreDatabaseSettings>(
    configuration.GetSection("QuizStoreDatabase"));

var jwtSettings = configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];
services.Configure<JwtSettings>(jwtSettings);

//Google Storage Client Settings
var bucketSettings = configuration.GetSection("BucketSettings");
services.Configure<BucketSettings>(bucketSettings);

services.AddSingleton<IStorageClientWrapper, StorageClientWrapper>();
services.AddSingleton<IDatabaseConfigurationProvider, DefaultDatabaseConfigurationProvider>();

// MongoDB Settings
services.AddSingleton<IMongoClient>(provider =>
{
    var configProvider = provider.GetRequiredService<IDatabaseConfigurationProvider>();
    var connectionString = configProvider.GetConnectionString();

    var mongoSettings = MongoClientSettings.FromConnectionString(connectionString);
    mongoSettings.SslSettings = new SslSettings
    {
        EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12
    };

    return new MongoClient(mongoSettings);
});

services.AddSingleton<IMongoDbContext, MongoDbContext>();

services.AddScoped<ICloudStorageService, CloudStorageService>();

services.AddJobScheduler();

// JWT settings
services.Configure<List<Role>>(configuration.GetSection("Roles"));

//cors policy
string CORSOpenPolicy = "OpenCORSPolicy";

services.AddCors(options =>
{
    options.AddPolicy(
      name: CORSOpenPolicy,
      builder => {
          builder.WithOrigins(configuration["FrontUrl"] ?? "https://quiz-biblio.fabien-hannon.com").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
      });
});

byte[] key = Encoding.UTF8.GetBytes(secretKey ?? "");
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
    };
    //reads the token in the cookie instead of the Authorization header
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Check if the token is in the cookie
            if (context.Request.Cookies.ContainsKey(jwtSettings["CookieName"] ?? "AuthToken"))
            {
                var cookieName = jwtSettings["CookieName"] ?? "AuthToken";
                context.Token = context.Request.Cookies[cookieName];
            }
            return Task.CompletedTask;
        }
    };
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

services.AddAutoMapper(cfg => cfg.AddProfile<AutoMapperProfile>());

services.AddControllers();

services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization = [new NoAuthFilter()]
});

app.UseHttpsRedirection();

app.UseCors(CORSOpenPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

JobsInitializer.StartJobs();

if (app.Environment.IsDevelopment())
{
    app.Run();
}
else
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    var url = $"http://0.0.0.0:{port}";
    app.Run();
}

