using Microsoft.EntityFrameworkCore;
using URL_Shortener_Server.Models;
using URL_Shortener_Server.Services;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.Property(s => s.Code).HasMaxLength(UrlShorteningService.NumberOfCharsInShortLink);

            builder.HasIndex(s => s.Code).IsUnique();
        });
    }

}
