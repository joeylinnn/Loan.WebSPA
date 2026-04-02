namespace LoanServiceApp.Models;

public class LoanPaymentResponse
{
    public int LoanId { get; set; }
    public decimal Principal { get; set; }
    public decimal AnnualInterestRate { get; set; }
    public int TermYears { get; set; }
    public decimal MonthlyPayment { get; set; }
    public decimal TotalPayment => MonthlyPayment * TermYears * 12;
    public decimal TotalInterest => TotalPayment - Principal;
}