using HeThongNganHang.Models;
using HeThongNganHang.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

/* Khoi gan ket noi database */
string connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"].ToString();
builder.Services.AddDbContext<DatabaseContext>(option => option.UseLazyLoadingProxies().UseSqlServer(connectionString));

builder.Services.AddScoped<AccountService, AccountServiceImpl>();
builder.Services.AddScoped<TransactionService, TransactionServiceImpl>();


builder.Services.AddSession();

var app = builder.Build();

app.UseSession();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "myareas",
    pattern: "{area:exists}/{controller}/{action}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}");

app.Run();
