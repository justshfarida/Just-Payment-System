
using System.Security.Claims;
using Api.Extensions;
using Application.Interfaces;
using Application.Interfaces.MappingProfiles;
using Application.MappingProfiles;
using Application.Services;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var keycloakClientId = builder.Configuration["Keycloak:ClientId"]!;

// Add services to the container.
builder.Services.AddDbContext<MerchantDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMerchantRepository,  MerchantRepository>();
builder.Services.AddScoped<IMerchantService, MerchantService>();
builder.Services.AddScoped<IMerchantMapper, MerchantMapper>();
builder.Services.AddSwaggerWithAuth(builder.Configuration);
builder.Services.AddAuthentication(builder.Configuration, builder.Environment.IsDevelopment());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId(keycloakClientId);
        options.OAuthUsePkce();
    });
}

// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapGet("/me", (ClaimsPrincipal user) =>
{
    return user;
});
app.Run();
