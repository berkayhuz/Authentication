using System.Text.RegularExpressions;

namespace Authentication.Lib.Helpers
{
    public static class SlugHelper
    {
        public static string GenerateSlug(string input)
        {
            var slug = RemoveTurkishCharacters(input).Replace(' ', '-');
            slug = RemoveSpecialCharacters(slug);
            return RemoveConsecutiveHyphens(slug).ToLower();
        }

        private static string RemoveTurkishCharacters(string input)
        {
            var turkishChars = new[] { "ğ", "ü", "ş", "ı", "i", "ö", "ç", "Ğ", "Ü", "Ş", "İ", "Ö", "Ç" };
            var englishChars = new[] { "g", "u", "s", "i", "i", "o", "c", "G", "U", "S", "I", "O", "C" };

            for (var i = 0; i < turkishChars.Length; i++) input = input.Replace(turkishChars[i], englishChars[i]);

            return input;
        }

        private static string RemoveSpecialCharacters(string input)
        {
            return Regex.Replace(input, @"[^a-zA-Z0-9\-]", "");
        }

        private static string RemoveConsecutiveHyphens(string input)
        {
            return Regex.Replace(input, @"-+", "-");
        }
    }
}
