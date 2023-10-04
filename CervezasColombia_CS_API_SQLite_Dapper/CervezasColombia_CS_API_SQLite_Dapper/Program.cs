using CervezasColombia_CS_API_SQLite_Dapper.DbContexts;
using CervezasColombia_CS_API_SQLite_Dapper.Interfaces;
using CervezasColombia_CS_API_SQLite_Dapper.Repositories;
using CervezasColombia_CS_API_SQLite_Dapper.Services;

var builder = WebApplication.CreateBuilder(args);

//Aqui agregamos los servicios requeridos

//El DBContext a utilizar
builder.Services.AddSingleton<SQLiteDbContext>();

//Los repositorios
builder.Services.AddScoped<IEstiloRepository, EstiloRepository>();
builder.Services.AddScoped<IResumenRepository, ResumenRepository>();
builder.Services.AddScoped<ICervezaRepository, CervezaRepository>();
builder.Services.AddScoped<ICerveceriaRepository, CerveceriaRepository>();
builder.Services.AddScoped<IUbicacionRepository, UbicacionRepository>();
builder.Services.AddScoped<IEnvasadoRepository, EnvasadoRepository>();
builder.Services.AddScoped<IIngredienteRepository, IngredienteRepository>();
builder.Services.AddScoped<IUnidadVolumenRepository, UnidadVolumenRepository>();

//Aqui agregamos los servicios asociados para cada EndPoint
builder.Services.AddScoped<EstiloService>();
builder.Services.AddScoped<ResumenService>();
builder.Services.AddScoped<CervezaService>();
builder.Services.AddScoped<CerveceriaService>();
builder.Services.AddScoped<UbicacionService>();
builder.Services.AddScoped<EnvasadoService>();
builder.Services.AddScoped<IngredienteService>();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
