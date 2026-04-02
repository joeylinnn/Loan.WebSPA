using System.ComponentModel.DataAnnotations;

namespace LoanServiceApp.Helpers;

public static class ValidationHelper
{
    public static List<string> ValidateModel<T>(T model)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model!);

        Validator.TryValidateObject(model!, context, results, validateAllProperties: true);

        return results.Select(r => r.ErrorMessage ?? "Validation error").ToList();
    }
}