using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WellWiseCR.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<WellWiseCRContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WellWiseCRContext") ?? throw new InvalidOperationException("Connection string 'WellWiseCRContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

//Ruta de login
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option => {
        option.LoginPath = "/Login/IniciarSesion";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//Utilice autenticacion
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=IniciarSesion}/{id?}");

app.Run();
