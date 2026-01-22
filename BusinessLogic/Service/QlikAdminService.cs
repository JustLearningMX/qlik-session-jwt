using BusinessLogic.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class QlikAdminService : ExceptionService
    {
        private readonly Models.QlikCloudSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<QlikAdminService> _logger;

        public QlikAdminService(
            IOptions<Models.QlikCloudSettings> options,
            HttpClient httpClient,
            ILogger<QlikAdminService> logger
        )
        {
            _settings = options.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<GenericResult<AdminTokenDtoResponse>> generate()
        {
            GenericResult<AdminTokenDtoResponse> result = new GenericResult<AdminTokenDtoResponse>();

            try
            {
                var token = await this.GetM2MTokenAsync(_settings.TenantUrl, _settings.ClientID, _settings.ClientSecret);

                var resp = new AdminTokenDtoResponse
                {
                    access_token = token.access_token,
                    scope = token.scope,
                    token_type = token.token_type,
                    expires_at = token.expires_at,
                    expires_in = token.expires_in
                };

                result.Resultado = resp;
                result.CreacionExitosa();
                return result;
            }
            catch (Exception e)
            {
                result = this.GeneraError<AdminTokenDtoResponse>(e);
            }

            return result;
        }

        private async Task<AdminTokenDtoResponse> GetM2MTokenAsync(string _apiTenant, string _clientId, string _clientSecret)
        {
            try
            {
                var tokenUrl = $"{_apiTenant}/oauth/token";

                var requestBody = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", _clientId },
                    { "client_secret", _clientSecret }
                };

                var content = new FormUrlEncodedContent(requestBody);

                var response = await _httpClient.PostAsync(tokenUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error obteniendo token M2M: {StatusCode} - {Error}",
                        response.StatusCode, errorText);
                    throw new HttpRequestException(
                        $"Error obteniendo token: HTTP {response.StatusCode}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<AdminTokenDtoResponse>(
                    jsonResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                _logger.LogInformation("Token M2M obtenido exitosamente");
                return tokenResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener token M2M de Qlik");
                throw;
            }
        }
    }
}
