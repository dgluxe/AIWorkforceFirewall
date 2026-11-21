using AIWorkforceFirewall.Web.Components;
using AIWorkforceFirewall.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var dataLeakServiceBaseUrl = builder.Configuration["DataLeakService:BaseUrl"];

if (string.IsNullOrWhiteSpace(dataLeakServiceBaseUrl))
{
    throw new InvalidOperationException(
        "A configuração DataLeakService:BaseUrl não foi definida.");
}

builder.Services.AddHttpClient<DataLeakApiClient>(client =>
{
    client.BaseAddress = new Uri(dataLeakServiceBaseUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
