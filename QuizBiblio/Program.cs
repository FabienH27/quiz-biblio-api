using Microsoft.AspNetCore.Authentication.JwtBearer;
using QuizBiblio.DataAccess;
using QuizBiblio.DatabaseSettings;
using QuizBiblio.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QuizBiblio;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//shortcuts
var services = builder.Services;
var configuration = builder.Configuration;

string? connectionString = configuration.GetValue<string>("QuizStoreDatabase:ConnectionString");
string? dbName = configuration.GetValue<string>("QuizStoreDatabase:DatabaseName");

var jwtSettings = configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];

// Add services to the container.
services.Configure<QuizStoreDatabaseSettings>(
    configuration.GetSection("QuizStoreDatabase"));

services.Configure<JwtSettings>(jwtSettings);


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

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
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

services.AddMongoDB<QuizBiblioDbContext>(connectionString ?? "", dbName ?? "");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(CORSOpenPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
