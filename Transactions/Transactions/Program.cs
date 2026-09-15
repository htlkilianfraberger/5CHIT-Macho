using Transactions.Components;
using Transactions.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var transactionsConnectionString = builder.Configuration.GetConnectionString("Transactions")
    ?? throw new InvalidOperationException("Connection string 'Transactions' is missing.");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContextFactory<TransactionsContext>(options =>
    options.UseMySql(
        transactionsConnectionString,
        ServerVersion.AutoDetect(transactionsConnectionString),
        mysqlOptions => mysqlOptions.CommandTimeout(5)));
builder.Services.AddScoped<TicketRepository>();

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
