global using CursoNet6.AccesoDatos.Datos;
global using CursoNet6.AccesoDatos.Datos.Repositorio;
global using CursoNet6.AccesoDatos.Datos.Repositorio.IRepositorio;
global using CursoNet6.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(Options =>
{
    Options.IdleTimeout = TimeSpan.FromMinutes(10);
    Options.Cookie.HttpOnly = true;
    Options.Cookie.IsEssential = true;
});

//Utilizar interfaces, y se destruyan al dejarlas de utilizarlas
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<ITipoAplicacionRepositorio, TipoAplicacionRepositorio>();
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();

builder.Services.AddScoped<IOrdenRepositorio, OrdenRepositorio>();
builder.Services.AddScoped<IOrdenDetalleRepositorio, OrdenDetalleRepositorio>();
builder.Services.AddScoped<IUsuarioAplicacionRepositorio, UsuarioAplicacionRepositorio>();


builder.Services.AddControllersWithViews();


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

//Para permitir autenticacion
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

//Para mostrar vistas tipo Razon Pages
app.MapRazorPages();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
