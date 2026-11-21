using AIWorkforceFirewall.Web.Models;

namespace AIWorkforceFirewall.Web.Services;

public class DataLeakApiClient
{
    private readonly HttpClient _httpClient;

    public DataLeakApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DataLeakAnalyzeResponse> AnalyzeAsync(
        DataLeakAnalyzeRequest request,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "api/dataleak/analyze",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<DataLeakAnalyzeResponse>(
            cancellationToken: cancellationToken);

        return result ?? throw new InvalidOperationException(
            "O serviço retornou uma resposta vazia.");
    }
}
