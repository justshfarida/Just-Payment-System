using Api.Extensions;
using Application;
using Application.Common.Interfaces;
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
    opts.UseRuntimeCompilation();
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

app.MapGet("users/me", (ClaimsPrincipal user) =>
{
    return Results.Ok(new
    {
        UserId = user.FindFirstValue(ClaimTypes.NameIdentifier),
        Email = user.FindFirstValue(ClaimTypes.Email),
        Name = user.FindFirstValue("preferred_username"),
        Claims = user.Claims.Select(c => new { c.Type, c.Value })
    });
})
.RequireAuthorization();

app.MapGet("/transactions", async (ITransactionReadRepository bus) =>
{
    var res = await bus.GetAllWithPaginationAsync(1, 1, null, null, null);
    return Results.Ok(res);
});

app.Run();