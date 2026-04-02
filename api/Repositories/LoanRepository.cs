using System.Text.Json;
using LoanServiceApp.Models;

namespace LoanServiceApp.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly List<Loan> _loans;

    public LoanRepository()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "loans.json");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Loan data file not found: {filePath}");
        }

        var json = File.ReadAllText(filePath);
        _loans = JsonSerializer.Deserialize<List<Loan>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? new List<Loan>();
    }

    public Task<IEnumerable<Loan>> GetAllLoansAsync()
    {
        return Task.FromResult(_loans.AsEnumerable());
    }

    public Task<Loan?> GetLoanByIdAsync(int id)
    {
        var loan = _loans.FirstOrDefault(x => x.LoanId == id);
        return Task.FromResult(loan);
    }
}