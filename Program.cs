using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using WebAPITransaction;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connection  = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;Initial Catalog=TransactionDb;Integrated Security=True;Encrypt=False");

connection.Open();

builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>(x=>
    new UnitOfWork(connection));

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
