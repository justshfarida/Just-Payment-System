using Application.Common.Interfaces.Services;
using Application.DTOs;
using System.Net.Http.Json;

namespace Infrastructure.Services;

public class MerchantServiceClient : IMerchantServiceClient
{
    private const string _clientName = "merchant";
    private readonly IHttpClientFactory _httpClientFactory;

    public MerchantServiceClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<MerchantPrivateKeyDto> GetKeysAsync(string merchantId)
    {
        var httpClient = _httpClientFactory.CreateClient(_clientName);
        var merchantSettingsUrl = $"{merchantId}/privatekey";
        var settingsResponse = await httpClient.GetFromJsonAsync<MerchantPrivateKeyDto>(merchantSettingsUrl);
        if (settingsResponse == null || string.IsNullOrEmpty(settingsResponse.SecretKey))
        {
            throw new Exception("Secret key is required");
        }

        return settingsResponse;
    }
}
