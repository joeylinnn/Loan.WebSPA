using LoanServiceApp.Models;

namespace LoanServiceApp.Repositories;

public interface ILoanRepository
{
    Task<Loan?> GetLoanByIdAsync(int id);
    Task<IEnumerable<Loan>> GetAllLoansAsync();
}