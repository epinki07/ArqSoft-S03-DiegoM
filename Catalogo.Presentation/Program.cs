using CatalogoApp.Application.Services;
using CatalogoApp.Infrastructure.Repositories;
using CatalogoApp.Domain.Models;
using CatalogoApp.Domain.Interfaces;
using Catalogo.Application.Services;
using Catalogo.Domain.Interfaces;
using Catalogo.Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Ruta del archivo JSON — se guarda en la carpeta "data" del proyecto

var jsonPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "Item.json");
var usuariosPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "Usuarios.json");
var comentariosPath = Path.Combine(builder.Environment.ContentRootPath, "Data", "Comentarios.json");

// Registrar el repositorio JSON como implementación ItemRepository
builder.Services.AddSingleton<IItemRepository>(
     new JsonItemRepository(jsonPath)
);

// Registrar el repositorio de Usuarios
builder.Services.AddSingleton<IUsuarioRepository>(
     new JsonUsuarioRepository(usuariosPath)
);

// Registrar el repositorio de Comentarios
builder.Services.AddSingleton<IComentarioRepository>(
     new JsonComentarioRepository(comentariosPath)
);

// Registrar el servicio de Application
builder.Services.AddScoped<ItemService>();

// Registrar el servicio de Usuarios
builder.Services.AddScoped<UsuarioService>();

// Registrar el servicio de Comentarios
builder.Services.AddScoped<ComentarioService>();

// Registrar autorización
builder.Services.AddAuthorization();

// Registrar sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Registrar servicios MVC
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
app.UseRouting();

// Usar sesiones
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(

    name: "default",

    pattern: "{controller=Home}/{action=Index}/{id?}")

    .WithStaticAssets();

app.Run();
app.Run();