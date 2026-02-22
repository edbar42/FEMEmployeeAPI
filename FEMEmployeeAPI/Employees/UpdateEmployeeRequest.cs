using FEMEmployeeAPI.Abstractions;
using FluentValidation;

namespace FEMEmployeeAPI.Employees;

// Do not allow to update First name, Last name 
// and SSN through this workflow

public class UpdateEmployeeRequest
{
  public UpdateEmployeeRequest()
  {
  }

  public required string? Address1 { get; set; }
  public required string? Address2 { get; set; }
  public required string? City { get; set; }
  public required string? State { get; set; }
  public required string? ZipCode { get; set; }
  public required string? PhoneNumber { get; set; }
  public required string? Email { get; set; }

}

public class UpdateEmployeeRequestValidator : AbstractValidator<UpdateEmployeeRequest>
{
  private readonly HttpContext _httpContext;
  private readonly IRepository<Employee> _repository;

  public UpdateEmployeeRequestValidator(IHttpContextAccessor httpContextAccessor, IRepository<Employee> repository)
  {
    _httpContext = httpContextAccessor.HttpContext!;
    _repository = repository;

    RuleFor(x => x.Address1).MustAsync(NotBeEmptyIfItIsSetOnEmployeeAlreadyAsync).WithMessage("Address1 must not be empty.");
  }

  private async Task<bool> NotBeEmptyIfItIsSetOnEmployeeAlreadyAsync(string? address, CancellationToken token)
  {
    await Task.CompletedTask;

    var id = Convert.ToInt32(_httpContext.Request.RouteValues["id"]);
    var employee = _repository.GetById(id);

    if (employee!.Address1 != null && string.IsNullOrWhiteSpace(address))
    {
      return false;
    }

    return true;
  }
}
