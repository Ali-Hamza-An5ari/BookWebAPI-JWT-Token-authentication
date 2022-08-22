using BookWebAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace BookWebAPI.Data
{
    public class BooksDbContext: DbContext
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options):base(options)
        {

        }
        //collection storing all the records of a table. Books
        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
