namespace DogApp
{
    public static class DogImageUrlParser
    {
        /// <summary>
        /// Look up breed by segment index after string split '/' \n
        /// <para>Example 'https:(s0)/(s1)/images.dog.ceo(s2)/breeds(s3)/malinois(s4)/n02105162_5800.jpg(s5)' ",</para>
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="segmentNumber">  /</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
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
