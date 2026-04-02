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

// builder.Services.Configure<SmtpSettings>(
//     builder.Configuration.GetSection("SmtpSettings"));
builder.Services.Configure<SmtpSettings>(options =>
{
    options.Host = builder.Configuration["SmtpHost"] ?? "";
    options.Port = int.TryParse(builder.Configuration["SmtpPort"], out var port) ? port : 0;
    options.UserName = builder.Configuration["SmtpUserName"] ?? "";
    options.Password = builder.Configuration["SmtpPassword"] ?? "";
    options.FromEmail = builder.Configuration["SmtpFromEmail"] ?? "";
    options.FromName = builder.Configuration["SmtpFromName"] ?? "";
});

builder.Services.AddScoped<ILoanRepository,LoanRepository>();
builder.Services.AddScoped<LoanCalculator>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Build().Run();