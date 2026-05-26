using Api.Endpoints;
using Api.Extensions;
using Api.Middlewares;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
var keycloakClientId = builder.Configuration["Keycloak:ClientId"]!;

builder.Services.AddDbContext<TransactionDbContext>(op
    => op.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();
builder.Services.RegisterServices();
builder.Services.RegisterRepositories();
builder.Services.RegisterMapper();
builder.Services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();
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
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandler = context.RequestServices.GetRequiredService<IExceptionHandler>();
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        if (exception != null)
        {
            await exceptionHandler.TryHandleAsync(context, exception, context.RequestAborted);
        }
    });
});
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();


app.MapGroup("/transactions")
    .MapTransactions()
    .WithTags("Transactions");

app.MapControllers();

app.Run();