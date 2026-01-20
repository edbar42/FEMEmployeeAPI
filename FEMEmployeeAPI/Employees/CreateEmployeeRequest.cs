namespace FEMEmployeeAPI.Employees;

public class CreateEmployeeRequest
{
  public required string FirstName { get; set; }
  public required string LastName { get; set; }
  public required string SocialSecurityNumber { get; set; }

  public required string? Address1 { get; set; }
  public required string? Address2 { get; set; }
  public required string? City { get; set; }
  public required string? State { get; set; }
  public required string? ZipCode { get; set; }
  public required string? PhoneNumber { get; set; }
  public required string? Email { get; set; }

}
