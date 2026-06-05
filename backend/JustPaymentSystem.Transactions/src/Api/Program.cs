using Api.Extensions;
using Api.Middlewares;
using Application;
using Application.Common.Models;
using Application.Features.Transactions.IntegrationEvents;
using Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Wolverine;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
var keycloakClientId = builder.Configuration["Keycloak:ClientId"]!;

builder.Services.AddDbContext<TransactionDbContext>(op
    => op.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();
builder.Services.Configure<ClientOptions>(builder.Configuration.GetSection("Client"));
builder.Services.RegisterServices();
builder.Services.RegisterRepositories();
builder.Services.RegisterMapper();
builder.Services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient("merchant", client =>
{
    client.BaseAddress = new Uri("/api/merchants");
    client.Timeout = TimeSpan.FromSeconds(30);

    client.DefaultRequestHeaders.Add("Accept", "application/json");
});
builder.Services.AddSwaggerWithAuth(builder.Configuration);
builder.Host.UseWolverine(opts =>
{
    opts.Discovery.IncludeAssembly(typeof(ApplicationAssembly).Assembly);
    opts.UseRabbitMq(con =>
    {
        con.UserName = "admin";
        con.Password = "admin123";
    }).AutoProvision();

    opts.PublishMessage<TransactionCompletedIntegrationEvent>()
        .ToRabbitQueue("transaction-completed");

    opts.ListenToRabbitQueue("callback-failed")
    .DefaultIncomingMessage<CallbackFailedIntegrationEvent>();

});
builder.Services.AddAuthentication(builder.Configuration, builder.Environment.IsDevelopment());

builder.Services.AddAuthorization();

var app = builder.Build();

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

app.MapControllers();

app.Run();