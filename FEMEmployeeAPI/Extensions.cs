using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FEMEmployeeAPI
{
  public static class Extensions
  {

    public static ModelStateDictionary ToModelStateDictionary(this FluentValidation.Results.ValidationResult validationResult)
    {
      var modelState = new ModelStateDictionary();

      foreach (var error in validationResult.Errors)
      {
        modelState.AddModelError(error.PropertyName, error.ErrorMessage);
      }

      return modelState;

    }

    public static ValidationProblemDetails ToValidationProblemDetails(
        this List<System.ComponentModel.DataAnnotations.ValidationResult> validationResults
    )
    {
      var problemDetails = new ValidationProblemDetails();

      foreach (var validationResult in validationResults)
      {
        foreach (var memberName in validationResult.MemberNames)
        {
          if (problemDetails.Errors.ContainsKey(memberName))
          {
            problemDetails.Errors[memberName] = problemDetails
                .Errors[memberName]
                .Concat([validationResult.ErrorMessage])
                .ToArray()!;
          }
          else
          {
            problemDetails.Errors[memberName] = [validationResult.ErrorMessage!];
          }
        }
      }

      return problemDetails;
    }
  }
}
