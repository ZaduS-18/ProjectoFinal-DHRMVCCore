using DHRMVCCore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static DHRMVCCore.Controllers.ActumsController;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BdDhrContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("conexion")));



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Usuarios/Login";
});

var app = builder.Build();

// Definición del endpoint de la API
app.MapGet("/listarActas", async (BdDhrContext context) =>
{
    var actumsList = await context.Acta
        // ... tus includes aquí ...
        .ToListAsync();

    var singleActum = actumsList.FirstOrDefault();
    return Results.Ok(new Tuple<IEnumerable<Actum>, Actum>(actumsList, singleActum));
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=RedirectToListarActums}/{id?}");

app.Run();
