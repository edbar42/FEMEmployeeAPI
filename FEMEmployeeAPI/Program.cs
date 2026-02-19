using FEMEmployeeAPI;
using FEMEmployeeAPI.Abstractions;
using FEMEmployeeAPI.Employees;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IRepository<Employee>, EmployeeRepository>();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddControllers();

var app = builder.Build();

var employeeRoute = app.MapGroup("employees");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
