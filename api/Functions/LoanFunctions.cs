using System.Text;
using LoanServiceApp.Helpers;
using LoanServiceApp.Models;
using LoanServiceApp.Repositories;
using LoanServiceApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace LoanServiceApp.Function;

public class LoanFunctions
{
    private readonly ILoanRepository _loanRepository;
    private readonly LoanCalculator _loanCalculator;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<LoanFunctions> _logger;

    public LoanFunctions(ILoanRepository loanRepository,
        LoanCalculator loanCalculator,
        IEmailSender emailSender,
        ILogger<LoanFunctions> logger)
    {
        _loanRepository = loanRepository;
        _loanCalculator = loanCalculator;
        _emailSender = emailSender;
        _logger = logger;
    }

[Function("GetLoanById")]
    public async Task<IActionResult> GetLoanById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "loans/{id:int}")]
        HttpRequest req,
        int id)
    {
        var loan = await _loanRepository.GetLoanByIdAsync(id);

        if (loan is null)
        {
            return new NotFoundObjectResult(new
            {
                Message = $"Loan with ID {id} not found."
            });
        }

        var monthlyPayment = _loanCalculator.CalculateMonthlyPayment(
            loan.Principal,
            loan.AnnualInterestRate,
            loan.TermYears);

        var result = new LoanPaymentResponse
        {
            LoanId = loan.LoanId,
            Principal = loan.Principal,
            AnnualInterestRate = loan.AnnualInterestRate,
            TermYears = loan.TermYears,
            MonthlyPayment = monthlyPayment
        };

        return new OkObjectResult(result);
    }

    [Function("GetAllLoans")]
    public async Task<IActionResult> GetAllLoans(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "loans")]
        HttpRequest req)
    {
        var loans = await _loanRepository.GetAllLoansAsync();
        return new OkObjectResult(loans);
    }

    [Function("SendLoanCalculationEmail")]
    public async Task<IActionResult> SendLoanCalculationEmail(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "loans/sendEmail")]
        HttpRequest req)
    {
        var request = await req.ReadFromJsonAsync<SendLoanCalculationEmailRequest>();

        if (request is null)
        {
            return new BadRequestObjectResult(new
            {
                Message = "Request body is required."
            });
        }

        var subject = $"Loan Calculation Result - {request.Principal:C} over {request.TermYears} years";

        var bodyBuilder = new StringBuilder();
        bodyBuilder.AppendLine("Hello,");
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine("Here is your loan calculation result:");
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine($"Principal: {request.Principal:C}");
        bodyBuilder.AppendLine($"Annual Interest Rate: {request.AnnualInterestRate}%");
        bodyBuilder.AppendLine($"Term (Years): {request.TermYears}");
        bodyBuilder.AppendLine($"Monthly Payment: {request.MonthlyPayment:C}");
        bodyBuilder.AppendLine($"Total Payment: {request.TotalPayment:C}");
        bodyBuilder.AppendLine($"Total Interest: {request.TotalInterest:C}");
        bodyBuilder.AppendLine();
        bodyBuilder.AppendLine("Sent from Zhengyuan Lin's Loan Demo Project.");
        bodyBuilder.AppendLine();

        await _emailSender.SendAsync(request.To, subject, bodyBuilder.ToString());

        return new OkObjectResult(new
        {
            Message = "Loan calculation email sent successfully."
        });
    }
}