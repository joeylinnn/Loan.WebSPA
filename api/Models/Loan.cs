namespace LoanServiceApp.Models;

public class Loan
{
    public int LoanId { get; set; }
    public decimal Principal { get; set; }
    public decimal AnnualInterestRate { get; set; }
    public int TermYears { get; set; }
}