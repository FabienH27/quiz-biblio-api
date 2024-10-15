using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using QuizBiblio.DataAccess;
using QuizBiblio.Models;
using QuizBiblio.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddServices();

string? connectionString = builder.Configuration.GetValue<string>("BookStoreDatabase:ConnectionString");
string? dbName = builder.Configuration.GetValue<string>("BookStoreDatabase:DatabaseName");

builder.Services.AddMongoDB<QuizBiblioDbContext>(connectionString ?? "", dbName ?? "");

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
