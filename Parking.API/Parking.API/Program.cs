using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Parking.API.Application.Validation.Filters;
using Parking.API.Infrastructure.Data;
using Parking.API.Infrastructure.HandlerDependency;
using System.Data;
using System.Reflection;
using static Parking.API.Application.DTOS.Config.MapperConfig;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;


//Permitir el acceso a un recurso por medio del navegador, en especialmente recurso javascript
builder.Services.AddCors(op =>
{
    op.AddPolicy("MyPoliceCors", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
        .WithHeaders("authorization", "accept", "content-type", "origin")
        .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS");

        //Authorization : credenciales de autenticación : JWT
        //Accept: recibir respuestas en formatos : app/json, text/plain, txt/html, image/png, audio/mpeg
        //Content-type:enviar formularios HTML, incluir encabezados : application/x-www-form-urlencoded,
        // "application/json", "application/xml", "text/plain", "multipart/form-data"
        //Origin : 

    });
});


// Add services to the container.

//registro de dependencia fluentvalidation, Behaviour, ExceptionFilter
builder.Services.AddControllers(options =>
    options.Filters.Add<ApiExceptionFilterAttribute>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ParkingContext>(opt =>
{
    opt.UseSqlServer(config.GetConnectionString("DefaultConnection"), sqlopts =>
    {
        sqlopts.MigrationsHistoryTable("_MigrationHistory", config.GetValue<string>("ADNCeiba"));
    });
    

});
builder.Services.AddSingleton<IDbConnection>(x => new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

//Manejador de dependencias,//Config depedencia
builder.Services.AddRequestDependency(typeof(Program).Assembly); //Request
builder.Services.AddRepositoryDependency(); //Repository
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddMediatR(Assembly.Load("Parking.API.Application"), typeof(Program).Assembly);
builder.Services.AddMediatRHandlers(typeof(Program).Assembly); //IRequests
builder.Services.Configure<ApiBehaviorOptions>(options =>
    options.SuppressModelStateInvalidFilter = true);
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.AddValidators();


var app = builder.Build();
app.UseCors("MyPoliceCors");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    //.RequireCors("MyPoliceCors");
});

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();



//builder.Services.AddScoped<IRequest<List<VehicleDTO>>, VehicleRequest>()*/;
//builder.Services.AddScoped<IRepository<Vehicle>, Repository<Vehicle>>();
//builder.Services.AddTransient<IRequestHandler<VehicleRequest, List<VehicleDTO>>, VehicleQueryHandler>();
//builder.Services.AddMediatR(typeof(VehicleQueryHandler).Assembly);    

