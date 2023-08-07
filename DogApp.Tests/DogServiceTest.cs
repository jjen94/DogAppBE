using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;

namespace DogApp.Tests
{
    public class DogServiceTests
    {
        private readonly Mock<ILogger<DogService>> _loggerMock;
        private readonly IOptions<DogApiOptions> options;
        private readonly string _dogApiUrl;

        public DogServiceTests()
        {
            _loggerMock = new Mock<ILogger<DogService>>();
            options = Options.Create(new DogApiOptions()
            {
                DogApiBaseUrl = "http://dogCEO"
            });
            _dogApiUrl = "http://dogCEO";
        }

        [Fact]
        public async Task GetRandomDogAsync_Success()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            
            mockHttp.When(_dogApiUrl)
                .Respond("application/json", "{\"message\": \"http://dogCEO/dog/pitbull/mrworldwide.jpg\", \"status\": \"success\"}");

            var httpClient= new HttpClient(mockHttp);
            IDogService dogService = new DogService(httpClient, _loggerMock.Object, options);

            // Act
            var result = await dogService.GetRandomDogAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal("pitbull", result.Breed);
            Assert.Equal("http://dogCEO/dog/pitbull/mrworldwide.jpg", result.ImageUrl);
        }

        [Fact]
        public async Task GetRandomDogAsync_ApiReturnsBadRequest_ThrowsException()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            // Setting up a BadRequest response for the expected URL.
            mockHttp.When(_dogApiUrl).Respond(req => new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            var httpClient = new HttpClient(mockHttp);
            IDogService dogService = new DogService(httpClient, _loggerMock.Object, options);
            
            // Act and Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => dogService.GetRandomDogAsync());
            Assert.Equal("Failed to get dog image", exception.Message);
        }
    }
}