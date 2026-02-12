using FEMEmployeeAPI;
using FEMEmployeeAPI.Abstractions;
using FEMEmployeeAPI.Employees;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<EmployeeRepository>();
builder.Services.AddProblemDetails();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

var employeeRoute = app.MapGroup("employees");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

employeeRoute.MapGet(
    string.Empty,
    (EmployeeRepository repo) =>
    {
        return Results.Ok(
            repo.GetAll()
                .Select(employee => new GetEmployeeResponse
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Address1 = employee.Address1,
                    Address2 = employee.Address2,
                    City = employee.City,
                    State = employee.State,
                    ZipCode = employee.ZipCode,
                    PhoneNumber = employee.PhoneNumber,
                    Email = employee.Email,
                })
        );
    }
);

employeeRoute.MapGet(
    "/{id:int}",
    (int id, EmployeeRepository repo) =>
    {
        var employee = repo.GetById(id);
        if (employee == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(
            new GetEmployeeResponse
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Address1 = employee.Address1,
                Address2 = employee.Address2,
                City = employee.City,
                State = employee.State,
                ZipCode = employee.ZipCode,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
            }
        );
    }
);

employeeRoute.MapPost(string.Empty, async (CreateEmployeeRequest employeeRequest, IRepository<Employee> repository, IValidator<CreateEmployeeRequest> validator) => {
    var validationResults = await validator.ValidateAsync(employeeRequest);
    if (!validationResults.IsValid)
    {
        return Results.ValidationProblem(validationResults.ToDictionary());
    }

    var newEmployee = new Employee {
        FirstName = employeeRequest.FirstName!,
        LastName = employeeRequest.LastName!,
        SocialSecurityNumber = employeeRequest.SocialSecurityNumber!,
        Address1 = employeeRequest.Address1,
        Address2 = employeeRequest.Address2,
        City = employeeRequest.City,
        State = employeeRequest.State,
        ZipCode = employeeRequest.ZipCode,
        PhoneNumber = employeeRequest.PhoneNumber,
        Email = employeeRequest.Email
    };
    repository.Create(newEmployee);
    return Results.Created($"/employees/{newEmployee.Id}", employeeRequest);
});

employeeRoute.MapPut(
    "{id}",
    (UpdateEmployeeRequest employee, int id, EmployeeRepository repo) =>
    {
        var existingEmployee = repo.GetById(id);
        if (existingEmployee == null)
        {
            return Results.NotFound();
        }

        existingEmployee.Address1 = employee.Address1;
        existingEmployee.Address2 = employee.Address2;
        existingEmployee.City = employee.City;
        existingEmployee.State = employee.State;
        existingEmployee.ZipCode = employee.ZipCode;
        existingEmployee.PhoneNumber = employee.PhoneNumber;
        existingEmployee.Email = employee.Email;

        repo.Update(existingEmployee);
        return Results.Ok(existingEmployee);
    }
);

app.Run();
