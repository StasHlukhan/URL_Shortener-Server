using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using URL_Shortener_Server.Models;
using URL_Shortener_Server.Services;

namespace YourNamespace.Controllers
{
    [Route("api")]
    public class UrlShortenerController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly UrlShorteningService _urlShorteningService;

        public UrlShortenerController(DataContext context, UrlShorteningService urlShorteningService)
        {
            _context = context;
            _urlShorteningService = urlShorteningService;
        }
        [HttpGet("shortenedurls")]
        public async Task<IActionResult> GetAllShortenedUrls()
        {
            var shortenedUrls = await _context.ShortenedUrls.ToListAsync();

            if (shortenedUrls == null || shortenedUrls.Count == 0)
            {
                return NotFound("No shortened URLs found.");
            }

            return Ok(shortenedUrls);
        }
        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] ShortenUrlRequest request)
        {
            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out Uri longUri))
            {
                return BadRequest("Url is invalid");
            }

            // Перевірка на дублювання довгого URL
            var existingUrl = await _context.ShortenedUrls
                .FirstOrDefaultAsync(s => s.LongUrl == request.Url);

            if (existingUrl != null)
            {
                return BadRequest(new { message = "This long url is already exists" });
            }

            var code = await _urlShorteningService.GenerateUniqueCode();

            var shortenedUrl = new ShortenedUrl
            {
                Id = Guid.NewGuid(),
                LongUrl = request.Url,
                Code = code,
                ShortUrl = $"{Request.Scheme}://{Request.Host}/api/{code}"
            };

            _context.ShortenedUrls.Add(shortenedUrl);
            await _context.SaveChangesAsync();

            var response = new
            {
                LongUrl = shortenedUrl.LongUrl,
                ShortUrl = shortenedUrl.ShortUrl
            };

            return Ok(response);
        }


        [HttpGet("{code}")]
        public async Task<IActionResult> RedirectUrl(string code)
        {
            var shortenedUrl = await _context.ShortenedUrls
                .FirstOrDefaultAsync(s => s.Code == code);

            if (shortenedUrl == null)
            {
                return NotFound();
            }

            return Redirect(shortenedUrl.LongUrl);
        }
        [HttpDelete("shortenedurls/{id}")]
        public async Task<IActionResult> DeleteShortenedUrl(Guid id)
        {
            var shortenedUrl = await _context.ShortenedUrls.FindAsync(id);

            if (shortenedUrl == null)
            {
                return NotFound();
            }

            _context.ShortenedUrls.Remove(shortenedUrl);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}
