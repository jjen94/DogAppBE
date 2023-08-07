namespace DogApp
{
    public class DogImageUrlParser
    {
        public static string ExtractBreedFromUrl(string imageUrl, uint segmentNumber)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new ArgumentNullException(nameof(imageUrl));
            }
            // Ensure URL contains separators
            if (!imageUrl.Contains('/'))
            {
                throw new FormatException("Provided imageUrl does not contain any segments.");
            }
            var urlSegments = imageUrl.Split('/');

            if (urlSegments.Length <= segmentNumber)
            {
                throw new FormatException($"Invalid segment number: {segmentNumber} for the provided imageUrl.");
            }

            return urlSegments[segmentNumber];
        }
    }
}
