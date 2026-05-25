using Application.Features.Transactions.Commands;
using Wolverine;

namespace Api.Endpoints;

public static class TransactionsEndpoints
{
    extension(RouteGroupBuilder route)
    {
        public RouteGroupBuilder MapTransactions()
        {
            route.MapPost("/transactions", async (CreateTransactionAndGetRedirectUrlCommand command, IMessageBus bus) =>
            {
                await bus.InvokeAsync(command);
            });
            return route;
        }
    }
}
