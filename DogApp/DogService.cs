using System;
using System.Text.Json;
using DogApp.DataObj;
using Microsoft.Extensions.Options;

namespace DogApp
{
    public interface IDogService
    {
        Task<Dog> GetRandomDogAsync();
    }

    public class DogService : IDogService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IOptions<DogApiOptions> _options;

        public DogService(HttpClient httpClient, ILogger<DogService> logger, IOptions<DogApiOptions> options)
        {
            _httpClient = httpClient;
            _logger = logger;
            _options = options;
        }

        public async Task<Dog> GetRandomDogAsync()
        {
            var dogApiResponse = await FetchDogApiResponseAsync();
            var processedDogApiResponse = await ProcessDogApiResponseAsync(dogApiResponse);
            return CreateDogFromApiResponse(processedDogApiResponse);
        }

        private async Task<HttpResponseMessage> FetchDogApiResponseAsync()
        {
            try
            {
                var uri = ValidateAndBuildUri(_options.Value.DogApiBaseUrl, _options.Value.RandomDogEndpoint);

                var response = await _httpClient.GetAsync(uri);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get dog image with status code {StatusCode}", response.StatusCode);
                    throw new InvalidOperationException("Failed to get dog image");
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error occurred while making the HTTP request to the Dog API.");
                throw;  
            }
        }

        private Uri ValidateAndBuildUri(string baseUrl, string endpoint)
        {
            if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(endpoint))
            {
                _logger.LogError("Invalid configuration: Base URL or endpoint is null or empty.");
                throw new InvalidOperationException("Invalid configuration: Base URL or endpoint is null or empty.");
            }

            UriBuilder uriBuilder = new UriBuilder(baseUrl);
            uriBuilder.Path += endpoint;

            Uri finalUri;
            if (!Uri.TryCreate(uriBuilder.ToString(), UriKind.Absolute, out finalUri))
            {
                _logger.LogError("Invalid URI: {Uri}", uriBuilder.ToString());
                throw new InvalidOperationException("Invalid URI.");
            }

            return finalUri;
        }


        private async Task<DogApiResponse> ProcessDogApiResponseAsync(HttpResponseMessage response)
        {
            try
            {
                var dogApiResponse = await response.Content.ReadFromJsonAsync<DogApiResponse>();
                if (dogApiResponse == null)
                {
                    _logger.LogError("Failed to deserialize dog API response or received an empty response.");
                    throw new InvalidOperationException("Failed to deserialize dog API response or received an empty response.");
                }

                if (dogApiResponse.Status != "success")
                {
                    _logger.LogError("The API response indicated a failure with status: {Status}", dogApiResponse?.Status);
                    throw new InvalidOperationException($"The API response indicated a failure with status: {dogApiResponse?.Status}");
                }

                return dogApiResponse;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error occurred while deserializing the Dog API response.");
                throw; 
            }
        }

        private Dog CreateDogFromApiResponse(DogApiResponse dogApiResponse)
        {
            if (string.IsNullOrEmpty(dogApiResponse?.Message))
            {
                _logger.LogError("Received null or empty message in dog API response");
                throw new InvalidOperationException("Received null or empty message in dog API response");
            }
            var breed = DogImageUrlParser.ExtractBreedFromUrl(dogApiResponse.Message, 4);
            return new Dog
            {
                ImageUrl = dogApiResponse.Message,
                Breed = breed
            };
        }
    }

}