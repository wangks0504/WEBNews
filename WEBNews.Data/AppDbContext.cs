using Microsoft.EntityFrameworkCore;
using WEBNews.Models.Entities;

namespace WEBNews.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<News> News => Set<News>();
    public DbSet<Attachment> Attachments => Set<Attachment>();
    public DbSet<NewsReceiver> NewsReceivers => Set<NewsReceiver>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.UserId);
            e.HasIndex(x => x.UserName).IsUnique();
            e.Property(x => x.UserName).HasMaxLength(50).IsRequired();
            e.Property(x => x.Email).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<News>(e =>
        {
            e.HasKey(x => x.NewsId);
            e.Property(x => x.Title).HasMaxLength(200).IsRequired();
            e.HasOne(x => x.Author)
                .WithMany(u => u.News)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Attachment>(e =>
        {
            e.HasKey(x => x.AttachmentId);
            e.Property(x => x.FileName).HasMaxLength(255).IsRequired();
            e.Property(x => x.FilePath).HasMaxLength(1024).IsRequired();
            e.HasOne(x => x.News)
                .WithMany(n => n.Attachments)
                .HasForeignKey(x => x.NewsId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<NewsReceiver>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.News)
                .WithMany(n => n.Receivers)
                .HasForeignKey(x => x.NewsId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Receiver)
                .WithMany(u => u.ReceivedNews)
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => new { x.NewsId, x.ReceiverId }).IsUnique();
        });
    }
}
