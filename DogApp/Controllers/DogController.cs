using Microsoft.AspNetCore.Mvc;

namespace DogApp.Controllers
{
    [ApiController]
    [Route("api/dogs")]
    public class DogController : ControllerBase
    {
        private readonly IDogService _dogService;
        private readonly ILogger<DogController> _logger;

        public DogController(IDogService dogService, ILogger<DogController> logger)
        {
            _dogService = dogService;
            _logger = logger;
        }

        [HttpGet("random")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status502BadGateway)]
        public async Task<IActionResult> GetRandomDog()
        {
            try
            {
                var dog = await _dogService.GetRandomDogAsync();
                return Ok(dog);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, $"An error occurred while processing the dog data: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "An error occurred while processing the dog data."
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving a random dog. The Dog API might be down or not reachable: {ex.Message}");
                return StatusCode(StatusCodes.Status502BadGateway, new ProblemDetails
                {
                    Status = StatusCodes.Status502BadGateway,
                    Title = "Bad Gateway",
                    Detail = "The Dog API might be down or not reachable."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while retrieving a random dog: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred."
                });
            }
        }
    }
}