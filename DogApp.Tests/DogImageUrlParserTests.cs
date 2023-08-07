
namespace DogApp.Tests
{
    public class DogImageUrlParserTests
    {
        [Fact]
        public void ExtractBreedFromUrl_ValidUrl_ReturnsBreed()
        {
            // Arrange
            var imageUrl = "https://images.dog.ceo/breeds/snoop/dog.jpg";

            // Act
            var breed = DogImageUrlParser.ExtractBreedFromUrl(imageUrl, 4);

            // Assert
            Assert.Equal("snoop", breed);
        }

        [Fact]
        public void ExtractBreedFromUrl_InvalidSegment_ThrowsFormatException()
        {
            // Arrange
            var imageUrl = "https://images.dog.ceo/breeds/snoop/dog.jpg";

            // Act & Assert
            Assert.Throws<FormatException>(() => DogImageUrlParser.ExtractBreedFromUrl(imageUrl, 10));
        }

        [Fact]
        public void ExtractBreedFromUrl_NullUrl_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            Assert.Throws<ArgumentNullException>(() => DogImageUrlParser.ExtractBreedFromUrl(null, 4));
        }

        [Fact]
        public void ExtractBreedFromUrl_EmptyUrl_ThrowsArgumentNullException()
        {
            // Arrange
            var imageUrl = string.Empty;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => DogImageUrlParser.ExtractBreedFromUrl(imageUrl, 4));
        }
    }
}
