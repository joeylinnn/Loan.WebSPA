using LoanServiceApp.Models;
using LoanServiceApp.Repositories;
using LoanServiceApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

builder.Services.Configure<SmtpSettings>(
    builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddScoped<ILoanRepository,LoanRepository>();
builder.Services.AddScoped<LoanCalculator>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Build().Run();