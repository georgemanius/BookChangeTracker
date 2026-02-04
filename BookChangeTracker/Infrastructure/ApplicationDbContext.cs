using BookChangeTracker.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookChangeTracker.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<BookAuthor> BookAuthors { get; set; }
    public DbSet<BookChangeLog> BookChangeLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Book configuration
        modelBuilder.Entity<Book>()
            .HasKey(b => b.Id);

        modelBuilder.Entity<Book>()
            .Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<Book>()
            .Property(b => b.Description)
            .HasMaxLength(500);

        // Author configuration
        modelBuilder.Entity<Author>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<Author>()
            .Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        // BookAuthor configuration
        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ba => ba.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        // BookChangeLog configuration
        modelBuilder.Entity<BookChangeLog>()
            .HasKey(bcl => bcl.Id);

        modelBuilder.Entity<BookChangeLog>()
            .Property(bcl => bcl.FieldName)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<BookChangeLog>()
            .Property(bcl => bcl.Description)
            .HasMaxLength(500);

        modelBuilder.Entity<BookChangeLog>()
            .Property(bcl => bcl.ChangedBy)
            .HasMaxLength(100);

        modelBuilder.Entity<BookChangeLog>()
            .Property(bcl => bcl.ChangeType)
            .HasConversion<string>();

        modelBuilder.Entity<BookChangeLog>()
            .HasIndex(bcl => bcl.BookId);

        modelBuilder.Entity<BookChangeLog>()
            .HasIndex(bcl => bcl.ChangedAt);

        modelBuilder.Entity<BookChangeLog>()
            .HasIndex(bcl => bcl.FieldName);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        CapturePropertyChanges();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void CapturePropertyChanges()
    {
        var entries = ChangeTracker.Entries<Book>()
            .Where(e => e.State == EntityState.Modified)
            .ToList();

        foreach (var entry in entries)
        {
            var bookId = entry.Entity.Id;
            var changedBy = "System"; // Default value - TODO: replace with actual user info once authorization is implemented

            RecordPropertyChange(entry, nameof(Book.Title), bookId, changedBy);
            RecordPropertyChange(entry, nameof(Book.Description), bookId, changedBy);
            RecordPropertyChange(entry, nameof(Book.PublishDate), bookId, changedBy);
        }
    }

    private void RecordPropertyChange(EntityEntry<Book> entry, string propertyName, int bookId, string changedBy)
    {
        var property = entry.Property(propertyName);
        if (!property.IsModified)
            return;

        var changeLog = new BookChangeLog
        {
            BookId = bookId,
            FieldName = propertyName,
            OldValue = property.OriginalValue?.ToString(),
            NewValue = property.CurrentValue?.ToString(),
            Description = $"{propertyName} changed to '{property.CurrentValue}'",
            ChangedAt = DateTime.UtcNow,
            ChangedBy = changedBy,
            ChangeType = ChangeType.PropertyChanged
        };
        BookChangeLogs.Add(changeLog);
    }
}
