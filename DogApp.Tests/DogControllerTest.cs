using DogApp.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DogApp.DataObj;

namespace DogApp.Tests
{
    public class DogControllerTests
    {
        private readonly Mock<IDogService> _mockDogService;
        private readonly Mock<ILogger<DogController>> _mockLogger;
        private readonly DogController _controller;

        public DogControllerTests()
        {
            _mockDogService = new Mock<IDogService>();
            _mockLogger = new Mock<ILogger<DogController>>();
            _controller = new DogController(_mockDogService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetRandomDog_ReturnsOkResult_WhenNoExceptionsAreThrown()
        {
            // Arrange
            var dog = new Dog();
            _mockDogService.Setup(x => x.GetRandomDogAsync()).ReturnsAsync(dog);

            // Act
            var result = await _controller.GetRandomDog();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dog, okResult.Value);
        }

        [Fact]
        public async Task GetRandomDog_ReturnsInternalServerErrorResult_WhenInvalidOperationExceptionIsThrown()
        {
            // Arrange
            _mockDogService.Setup(x => x.GetRandomDogAsync()).ThrowsAsync(new InvalidOperationException());

            // Act
            var result = await _controller.GetRandomDog();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetRandomDog_ReturnsBadGatewayResult_WhenHttpRequestExceptionIsThrown()
        {
            // Arrange
            _mockDogService.Setup(x => x.GetRandomDogAsync()).ThrowsAsync(new HttpRequestException());

            // Act
            var result = await _controller.GetRandomDog();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status502BadGateway, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task GetRandomDog_ReturnsInternalServerErrorResult_WhenExceptionIsThrown()
        {
            // Arrange
            _mockDogService.Setup(x => x.GetRandomDogAsync()).ThrowsAsync(new Exception());

            // Act
            var result = await _controller.GetRandomDog();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
        }
    }
}
