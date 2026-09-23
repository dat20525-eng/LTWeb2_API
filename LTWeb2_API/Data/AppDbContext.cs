using LTWeb2_API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LTWeb2_API.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(
			DbContextOptions<AppDbContext> dbContextOptions)
			: base(dbContextOptions)
		{
			// Constructor
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Book - Book_Author
			modelBuilder.Entity<Book_Author>()
				.HasOne(b => b.Book)
				.WithMany(ba => ba.Book_Authors)
				.HasForeignKey(bi => bi.BookId);

			// Author - Book_Author
			modelBuilder.Entity<Book_Author>()
				.HasOne(b => b.Author)
				.WithMany(ba => ba.Book_Authors)
				.HasForeignKey(bi => bi.AuthorId);
		}

		public DbSet<Book> Books { get; set; }

		public DbSet<Author> Authors { get; set; }

		public DbSet<Book_Author> Books_Authors { get; set; }

		public DbSet<Publisher> Publishers { get; set; }
	}
}