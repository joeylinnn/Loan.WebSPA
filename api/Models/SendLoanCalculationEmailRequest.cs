using System.ComponentModel.DataAnnotations;

namespace LoanServiceApp.Models;

public class SendLoanCalculationEmailRequest
{
    [Required]
    [EmailAddress]
    public string To { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Principal { get; set; }

    [Range(0, 100)]
    public decimal AnnualInterestRate { get; set; }

    [Range(1, 100)]
    public int TermYears { get; set; }

    [Range(0, double.MaxValue)]
    public decimal MonthlyPayment { get; set; }

    [Range(0, double.MaxValue)]
    public decimal TotalPayment { get; set; }

    [Range(0, double.MaxValue)]
    public decimal TotalInterest { get; set; }
}