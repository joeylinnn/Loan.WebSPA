namespace LoanServiceApp.Services;

public class LoanCalculator
{
    public decimal CalculateMonthlyPayment(decimal principal, decimal annualInterestRate, int termYears)
    {
        int totalMonths = termYears * 12;

        if (totalMonths <= 0)
            throw new ArgumentException("TermYears must be greater than zero.");

        if (annualInterestRate == 0)
            return Math.Round(principal / totalMonths, 2);

        decimal monthlyRate = annualInterestRate / 100 / 12;
        double pow = Math.Pow((double)(1 + monthlyRate), totalMonths);

        decimal monthlyPayment =
            principal * monthlyRate * (decimal)pow / ((decimal)pow - 1);

        return Math.Round(monthlyPayment, 2);
    }
}