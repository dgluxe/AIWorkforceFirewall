using AIWorkforceFirewall.DataLeakService.IServices;
using AIWorkforceFirewall.DataLeakService.Services;
using AIWorkforceFirewall.DataLeakService.Services.Detectors;
using AIWorkforceFirewall.DataLeakService.Models.Policies;
using AIWorkforceFirewall.DataLeakService.IServices.Policies;
using AIWorkforceFirewall.DataLeakService.Services.Policies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SensitiveDataPolicyOptions>(
    builder.Configuration.GetSection(
        SensitiveDataPolicyOptions.SectionName));

// Add services to the container.

builder.Services.AddScoped<IDataLeakAnalyzer, DataLeakAnalyzer>();

builder.Services.AddScoped<ISensitiveDataDetector, CpfDetector>();
builder.Services.AddScoped<ISensitiveDataDetector, CnpjDetector>();
builder.Services.AddScoped<ISensitiveDataDetector, EmailDetector>();
builder.Services.AddScoped<ISensitiveDataDetector, PhoneDetector>();
builder.Services.AddScoped<ISensitiveDataDetector, RgDetector>();
builder.Services.AddScoped<ISensitiveDataDetector, CreditCardDetector>();
builder.Services.AddScoped<ISensitiveDataDetector, ApiKeyDetector>();

builder.Services.AddSingleton<IPolicyRuleProvider, ConfigurationPolicyRuleProvider>();

builder.Services.AddSingleton<IPolicyEngine, PolicyEngine>();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
