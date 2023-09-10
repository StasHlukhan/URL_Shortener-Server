using System;
using System.ComponentModel.DataAnnotations;

public class ShortenedUrl
{
    
    public Guid Id { get; set; }

    public string LongUrl { get; set; }

    public string ShortUrl { get; set; }
    public string Code { get; set; }
}
