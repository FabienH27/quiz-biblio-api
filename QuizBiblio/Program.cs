using Microsoft.AspNetCore.Authentication.JwtBearer;
using QuizBiblio.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using QuizBiblio.Models.Rbac;
using System.IdentityModel.Tokens.Jwt;
using MongoDB.Driver;
using QuizBiblio.DataAccess.QbDbContext;
using Google.Cloud.Storage.V1;
using QuizBiblio.Models.DatabaseSettings;
using QuizBiblio.Models.Settings;
using QuizBiblio.Helper;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using QuizBiblio.JobScheduler;
using Hangfire.Dashboard;
using QuizBiblio.JobScheduler.Authorization;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // Avoid renaming claims

//shortcuts
var services = builder.Services;
var configuration = builder.Configuration;

string? dbName = configuration.GetValue<string>("QuizStoreDatabase:DatabaseName");

//Google Storage Client Settings
var bucketSettings = configuration.GetSection("BucketSettings");
services.Configure<BucketSettings>(bucketSettings);

services.AddSingleton(await StorageClient.CreateAsync());

// MongoDB Settings
var connectionString = SecretManagerHelper.GetConnectionStringFromSecretManager("quiz-database-connection");

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
    connectionString = configuration.GetConnectionString("QuizStoreDatabase");
}

var settings = MongoClientSettings.FromConnectionString(connectionString);
settings.SslSettings = new SslSettings() { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
services.AddSingleton<IMongoClient>(new MongoClient(settings));

services.AddSingleton<IMongoDbContext, MongoDbContext>();

if(connectionString != null && dbName != null)
{
    services.AddJobScheduler(connectionString,dbName);
}

// JWT settings
var jwtSettings = configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];

services.Configure<QuizStoreDatabaseSettings>(
    configuration.GetSection("QuizStoreDatabase"));
services.Configure<JwtSettings>(jwtSettings);
services.Configure<List<Role>>(configuration.GetSection("Roles"));

//cors policy
string CORSOpenPolicy = "OpenCORSPolicy";

services.AddCors(options =>
{
    options.AddPolicy(
      name: CORSOpenPolicy,
      builder => {
          builder.WithOrigins(configuration["FrontUrl"] ?? "").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
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

services.AddControllers();

services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization = [new NoAuthFilter()]
});

app.UseHttpsRedirection();

app.UseCors(CORSOpenPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

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

