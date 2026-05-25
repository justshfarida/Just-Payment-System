namespace Application.Features.Transactions.IngtegrationEvents;

public record TransactionFailed(Guid TransactionId);

public class TransactionFailedHandler
{

}