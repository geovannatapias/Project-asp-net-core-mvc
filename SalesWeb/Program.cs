using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWeb.Data;
using SalesWeb.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SalesWebContext>(options =>
   options.UseMySql(builder.Configuration.GetConnectionString("SalesWebContext"),
   new MySqlServerVersion(new Version(8, 0, 38)), // Ajuste para sua versão do MySQL
    mysqlOptions => mysqlOptions.MigrationsAssembly("SalesWeb")));


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<SeedingService>(); // registrar injeção de dependencia 
builder.Services.AddTransient <SellerService>(); //serviço pode ser injetado em outras classes
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<SalesRecordService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
var enUS = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = new List<CultureInfo> { enUS },
    SupportedUICultures = new List<CultureInfo> { enUS }
};

app.UseRequestLocalization(localizationOptions); 

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<SeedingService>();
        
        seeder.Seed(); 
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
