using Microsoft.EntityFrameworkCore;

namespace URL_Shortener_Server.Services
{
    public class UrlShorteningService
    {

        public const int NumberOfCharsInShortLink =  7;
        private  const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private readonly Random _random = new ();

        private readonly DataContext _context;


        public UrlShorteningService(DataContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateUniqueCode()
        {
            var codeChars = new char[NumberOfCharsInShortLink];

            while (true)
            {
                for (var i = 0; i < NumberOfCharsInShortLink; i++)
                {
                    var randomIndex = _random.Next(Alphabet.Length - 1);
                    codeChars[i] = Alphabet[randomIndex];
                }
                var code = new string(codeChars);
                if (!await _context.ShortenedUrls.AnyAsync(s => s.Code == code))
                {
                    return code;

                }
            }

          
        }
    }
}
