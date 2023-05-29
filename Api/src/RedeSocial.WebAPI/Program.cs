using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using RedeSocial.Application.Contacts;
using RedeSocial.Application.Implementations.Services;
using RedeSocial.Domain.Contracts.Repositories;
using RedeSocial.Infrastructure.Data;
using RedeSocial.Infrastructure.Repositories;
using RedeSocial.WebAPI.Security;
using RedeSocial.WebAPI.Security.Interface;
using RedeSocial.Application.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "My Authorize API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAmizadeService, AmizadeService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICurtidaService, CurtidaService>();
builder.Services.AddScoped<IComentarioService, ComentarioService>();

builder.Services.AddScoped<AuthenticationUtils>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IComentarioRepository, ComentarioRepository>();
builder.Services.AddScoped<ICurtidaRepository, CurtidaRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IAmizadeRepository, AmizadeRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenSettings.SecretKey)),
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(b => b.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();
