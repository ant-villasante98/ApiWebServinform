using Microsoft.EntityFrameworkCore;
using Servirform.DataAcces;
using Servirform;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);



// Conexcion con la base de datos
string CONNECTION_NAME = "Servinform";
var connectionString = builder.Configuration.GetConnectionString(CONNECTION_NAME);



// Add Context
builder.Services.AddDbContext<ServinformContext>(option =>
option.UseNpgsql(connectionString));


// Add Services of JWT Authorization
builder.Services.AddJwtTokenServices(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configuracion de Swagger par que tome el jwt
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorizarion",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Jwt Authorization Header using Bearer Scheme"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme{
            Reference = new OpenApiReference{
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[]{}
        }
    });
});

// Habilitar el cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy", builder =>
    {
        builder.AllowAnyOrigin();
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Le decimos a la aplicacion que use el cors
app.UseCors("CorsPolicy");

app.Run();
