using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.SqlClient;
using UB.BLL.Repositories.Interface.IInvoice;
using UB.BLL.Repositories.Interface.IProduct;
using UB.BLL.Repositories.Service.Invoice;
using UB.BLL.Repositories.Service.Product;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // MVC support
builder.Services.AddRazorPages();          // Razor Pages support


// Register repositories (Dependency Injection)
builder.Services.AddScoped<ISetup_Product, Setup_Product>();
builder.Services.AddScoped<IInven_Invoice, Invoice>();


// Add configuration for database connection
builder.Services.AddSingleton((sp) =>
    builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();