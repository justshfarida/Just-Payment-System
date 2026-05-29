using Wolverine;
using Wolverine.ErrorHandling;
using Wolverine.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// 1. Register HttpClient so our background handlers can send outbound HTTP POSTs
builder.Services.AddHttpClient();

// 2. Configure Wolverine to act as our asynchronous background worker
builder.Host.UseWolverine(opts =>
{
    // Tell Wolverine where your RabbitMQ instance is running
    var rabbitMqUri = builder.Configuration["RabbitMq:Uri"];
    opts.UseRabbitMq(new Uri(rabbitMqUri)).AutoProvision();

    opts.ListenToRabbitQueue("transaction-completed");
    opts.ListenToRabbitQueue("transaction-failed"); 

    // Resilience Policy: If an outbound HTTP call to a merchant fails (network dropout/timeout),
    // retry 3 times with a progressive cooldown before giving up.
    opts.Policies.OnException<HttpRequestException>()
        .RetryWithCooldown(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(30));
});

var app = builder.Build();

// We keep the app minimal because it mostly runs background tasks, 
// but a basic health check endpoint lets you know the service is alive.
app.MapGet("/", () => "Webhook Service is processing messages...");

app.Run();