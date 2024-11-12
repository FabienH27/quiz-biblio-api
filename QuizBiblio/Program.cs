using Microsoft.AspNetCore.Authentication.JwtBearer;
using QuizBiblio.DataAccess;
using QuizBiblio.DatabaseSettings;
using QuizBiblio.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QuizBiblio;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

//shortcut
var services = builder.Services;

string? connectionString = configuration.GetValue<string>("QuizStoreDatabase:ConnectionString");
string? dbName = configuration.GetValue<string>("QuizStoreDatabase:DatabaseName");

var jwtSettings = configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Secret"];

// Add services to the container.
services.Configure<QuizStoreDatabaseSettings>(
    configuration.GetSection("QuizStoreDatabase"));

services.Configure<JwtSettings>(jwtSettings);

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
services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
