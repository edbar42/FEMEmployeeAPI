namespace FEMEmployeeAPI.Employees;

// Do not allow to update First name, Last name 
// and SSN through this workflow

public class UpdateEmployeeRequest
{
  public required string? Address1 { get; set; }
  public required string? Address2 { get; set; }
  public required string? City { get; set; }
  public required string? State { get; set; }
  public required string? ZipCode { get; set; }
  public required string? PhoneNumber { get; set; }
  public required string? Email { get; set; }

}

