using CervezasColombia_CS_API_Mongo.DbContexts;
using CervezasColombia_CS_API_Mongo.Interfaces;
using CervezasColombia_CS_API_Mongo.Repositories;
using CervezasColombia_CS_API_Mongo.Services;


var builder = WebApplication.CreateBuilder(args);

//Aqui agregamos los servicios requeridos

//El DBContext a utilizar
builder.Services.AddSingleton<MongoDbContext>();

//Los repositorios
builder.Services.AddScoped<IResumenRepository, ResumenRepository>();
builder.Services.AddScoped<IUbicacionRepository, UbicacionRepository>();
builder.Services.AddScoped<IEstiloRepository, EstiloRepository>();
builder.Services.AddScoped<IEnvasadoRepository, EnvasadoRepository>();
builder.Services.AddScoped<ICerveceriaRepository, CerveceriaRepository>();
builder.Services.AddScoped<ICervezaRepository, CervezaRepository>();

//Aqui agregamos los servicios asociados para cada EndPoint
builder.Services.AddScoped<ResumenService>();
builder.Services.AddScoped<UbicacionService>();
builder.Services.AddScoped<EstiloService>();
builder.Services.AddScoped<EnvasadoService>();
builder.Services.AddScoped<CerveceriaService>();
builder.Services.AddScoped<CervezaService>();


// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
