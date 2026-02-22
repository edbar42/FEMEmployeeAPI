using System.Net;
using System.Net.Http.Json;
using FEMEmployeeAPI.Abstractions;
using FEMEmployeeAPI.Employees;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace FEMEmployeeAPI.Tests;

public class BasicTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly int _employeeId = 1;
  private readonly WebApplicationFactory<Program> _factory;

  public BasicTests(WebApplicationFactory<Program> factory)
  {
    _factory = factory;

    var repo = _factory.Services.GetRequiredService<IRepository<Employee>>();
    repo.Create(new Employee
    {
      FirstName = "John",
      LastName = "Doe",
      Address1 = "123 Main St",
      SocialSecurityNumber = "69696969"
    });
    _employeeId = repo.GetAll().First().Id;
  }

  [Fact]
  public async Task GetAllEmployees_ReturnsOkResult()
  {
    var client = _factory.CreateClient();
    var response = await client.GetAsync("/employees");

    response.EnsureSuccessStatusCode();
  }

  [Fact]
  public async Task GetEmployeeById_ReturnsOkResult()
  {
    var client = _factory.CreateClient();
    var response = await client.GetAsync("/employees/1");

    response.EnsureSuccessStatusCode();
  }

  [Fact]
  public async Task CreateEmployee_ReturnsCreatedResult()
  {
    var client = _factory.CreateClient();
    var response = await client.PostAsJsonAsync("/employees", new CreateEmployeeRequest
    {
      FirstName = "John",
      LastName = "Doe",
      SocialSecurityNumber = "69696969",
      Address1 = "",
      Address2 = "",
      City = "",
      State = "",
      ZipCode = "",
      PhoneNumber = "",
      Email = ""
    });

    response.EnsureSuccessStatusCode();
  }

  [Fact]
  public async Task CreateEmployee_ReturnsBadRequestResult()
  {
    // Arrange
    var client = _factory.CreateClient();
    var invalidEmployee = new CreateEmployeeRequest
    {
      Address1 = "",
      Address2 = "",
      ZipCode = "",
      City = "",
      State = "",
      PhoneNumber = "",
      Email = "",
      FirstName = "",
      LastName = "",
      SocialSecurityNumber = ""
    }; // Empty object to trigger validation errors

    // Act
    var response = await client.PostAsJsonAsync("/employees", invalidEmployee);

    // Assert
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    Assert.NotNull(problemDetails);
    Assert.Contains("FirstName", problemDetails.Errors.Keys);
    Assert.Contains("LastName", problemDetails.Errors.Keys);
    Assert.Contains("The FirstName field is required.", problemDetails.Errors["FirstName"]);
    Assert.Contains("The LastName field is required.", problemDetails.Errors["LastName"]);
  }

  [Fact]
  public async Task UpdateEmployee_ReturnsOkResult()
  {
    var client = _factory.CreateClient();
    var response = await client.PutAsJsonAsync($"/employees/{_employeeId}", new UpdateEmployeeRequest
    {
      Address1 = "456 New St",
      Address2 = "",
      City = "",
      State = "",
      ZipCode = "",
      PhoneNumber = "",
      Email = ""
    });

    response.EnsureSuccessStatusCode();
  }

  [Fact]
  public async Task UpdateEmployee_ReturnsBadRequestWhenAddress()
  {
    // Arrange
    var client = _factory.CreateClient();
    var invalidEmployee = new UpdateEmployeeRequest
    {
      Address1 = "",
      Address2 = "",
      ZipCode = "",
      City = "",
      State = "",
      PhoneNumber = "",
      Email = "",
    }; // Empty object to trigger validation errors

    // Act
    var response = await client.PutAsJsonAsync($"/employees/{_employeeId}", invalidEmployee);

    // Assert
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

    var problemDetails = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>();
    Assert.NotNull(problemDetails);
    Assert.Contains("Address1", problemDetails.Errors.Keys);
  }
}
