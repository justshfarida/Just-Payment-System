using Api.Endpoints;
using Api.Extensions;
using Application;
using Application.Features.Transactions.Queries;
using Application.Features.Transactions.Queries.DTOs;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
var keycloakClientId = builder.Configuration["Keycloak:ClientId"]!;

builder.Services.AddDbContext<TransactionDbContext>(op 
    => op.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.RegisterRepositories();
builder.Services.RegisterMapper();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuth(builder.Configuration);
builder.Host.UseWolverine(opts =>
{
    opts.Discovery.IncludeAssembly(typeof(ApplicationAssembly).Assembly);
});
builder.Services.AddAuthentication(builder.Configuration, builder.Environment.IsDevelopment());

builder.Services.AddAuthorization();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.MapGroup("/transactions")
    .MapTransactions()
    .WithTags("Transactions");

app.Run();