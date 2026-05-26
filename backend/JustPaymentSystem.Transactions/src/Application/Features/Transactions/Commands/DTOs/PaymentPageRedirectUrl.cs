namespace Application.Features.Transactions.Commands.DTOs;

public class PaymentPageRedirectUrl
{
    public string RedirectUrl { get; set; } = null!;

    public PaymentPageRedirectUrl(string redirectUrl)
    {
        RedirectUrl = redirectUrl;
    }
}
