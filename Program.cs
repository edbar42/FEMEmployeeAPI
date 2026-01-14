// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var employeeRoute = app.MapGroup("employees");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var employees = new List<Employee>
{
    new Employee
    {
        Id = 1,
        FirstName = "John",
        LastName = "Nightreign",
    },
    new Employee
    {
        Id = 2,
        FirstName = "Jack",
        LastName = "Harlow",
    },
};

app.UseHttpsRedirection();

employeeRoute.MapGet(
    string.Empty,
    () =>
    {
        return Results.Ok(employees);
    }
);

employeeRoute.MapGet(
    "/{id:int}",
    (int id) =>
    {
        var employee = employees.SingleOrDefault(e => e.Id == id);
        if (employee == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(employee);
    }
);

employeeRoute.MapPost(
    string.Empty,
    (Employee employee) =>
    {
        employee.Id = employees.Max(e => e.Id) + 1;
        employees.Add(employee);
        return Results.Created($"/employees/{employee.Id}", employee);
    }
);

app.Run();
