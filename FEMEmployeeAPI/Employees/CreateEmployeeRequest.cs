using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace FEMEmployeeAPI.Employees;

public class CreateEmployeeRequest
{
  public CreateEmployeeRequest()
  {
  }

  [Required(AllowEmptyStrings = false)]
  public required string? FirstName { get; set; }

  [Required(AllowEmptyStrings = false)]
  public required string? LastName { get; set; }

  [Required(AllowEmptyStrings = false)]
  public required string? SocialSecurityNumber { get; set; }

  public required string? Address1 { get; set; }
  public required string? Address2 { get; set; }
  public required string? City { get; set; }
  public required string? State { get; set; }
  public required string? ZipCode { get; set; }
  public required string? PhoneNumber { get; set; }
  public required string? Email { get; set; }
}

public class CreateEmployeeRequestValidator : AbstractValidator<CreateEmployeeRequest>
{
  public CreateEmployeeRequestValidator()
  {
    RuleFor(x => x.FirstName).NotEmpty();
    RuleFor(x => x.LastName).NotEmpty();
    RuleFor(x => x.SocialSecurityNumber).NotEmpty();
  }
}
