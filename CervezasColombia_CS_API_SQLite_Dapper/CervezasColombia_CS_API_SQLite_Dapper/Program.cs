using CervezasColombia_CS_API_SQLite_Dapper.Cervecerias;
using CervezasColombia_CS_API_SQLite_Dapper.Cervezas;
using CervezasColombia_CS_API_SQLite_Dapper.Estilos;
using CervezasColombia_CS_API_SQLite_Dapper.Helpers;
using CervezasColombia_CS_API_SQLite_Dapper.Resumen;
using CervezasColombia_CS_API_SQLite_Dapper.Ubicaciones;
using CervezasColombia_CS_API_SQLite_Dapper.Unidades;
using CervezasColombia_CS_API_SQLite_Dapper.Ingredientes;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//Aqui agregamos los servicios requeridos

//El DBContext a utilizar
builder.Services.AddSingleton<SQLiteDbContext>();

//Los repositorios
builder.Services.AddScoped<IResumenRepository, ResumenRepository>();
builder.Services.AddScoped<IUbicacionRepository, UbicacionRepository>();
builder.Services.AddScoped<ICerveceriaRepository, CerveceriaRepository>();
builder.Services.AddScoped<ICervezaRepository, CervezaRepository>();
builder.Services.AddScoped<IEstiloRepository, EstiloRepository>();
builder.Services.AddScoped<IIngredienteRepository, IngredienteRepository>();
builder.Services.AddScoped<IUnidadRepository, UnidadRepository>();
//builder.Services.AddScoped<IEnvasadoRepository, EnvasadoRepository>();

//Aqui agregamos los servicios asociados para cada EndPoint
builder.Services.AddScoped<ResumenService>();
builder.Services.AddScoped<UbicacionService>();
builder.Services.AddScoped<CerveceriaService>();
builder.Services.AddScoped<CervezaService>();
builder.Services.AddScoped<EstiloService>();
builder.Services.AddScoped<UnidadService>();
builder.Services.AddScoped<IngredienteService>();
//builder.Services.AddScoped<EnvasadoService>();


// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Cervezas Artesanales de Colombia - SQLite Version",
        Description = "API para la gesti�n de cervezas artesanales de Colombia"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Modificamos el encabezado de las peticiones para ocultar el web server utilizado
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Server", "CraftBeerServer");
    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
