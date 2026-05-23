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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuth(builder.Configuration);
builder.Host.UseWolverine(opts =>
{
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

app.MapGet("/transactions", async (IMessageBus bus) =>
{
    return await bus.InvokeAsync<List<TransactionResponse>>(new GetTransactionsQuery(1, 10, null, null, null));
});

app.Run();