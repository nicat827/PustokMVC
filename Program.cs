using Microsoft.EntityFrameworkCore;
using PustokApp.DAL;
using PustokApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("MSSQL"));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<LayoutService>();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
        "area",
        "{area:exists}/{controller=dashboard}/{action=index}/{id?}"
);
app.MapControllerRoute(
        "default",
        "{controller=home}/{action=index}/{id?}"
);

app.Run();
