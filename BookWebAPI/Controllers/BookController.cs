using BookWebAPI.Data;
using BookWebAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly BooksDbContext _context;

        //lambda statement to define _contenxt
        public BookController(BooksDbContext context) => _context = context;

        //get records outside of the controller
        [HttpGet]
        public async Task<IEnumerable<Book>> Get()
            => await _context.Books.ToListAsync();

        [HttpGet("{id}")]
        //Specifying the possible response from the given method
        [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            //Linq query with Entity Framework
            var Book = await _context.Books.FindAsync(id);
            //there can be record with id or there may not be. 
            return Book == null ? NotFound() : Ok(Book);
        }

        [HttpGet("search/{title}")]
        [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByTitle(string title)
        {
            var Book = await _context.Books.SingleOrDefaultAsync(c => c.Title == title);
            return Book == null ? NotFound() : Ok(Book);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //If an unsuccessful save happens while creat
        public async Task<IActionResult> Create(Book book)
        {

            await _context.Books.AddAsync(book);
   //SaveChanges() reflects the changes or refreshes the database. SaveChangesAsync() is asynchronized version of SaveChanges()
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = book.BookId }, book);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, Book book)
        {
            if (id != book.BookId) return BadRequest();

            _context.Entry(book).State = EntityState.Modified;//Tracking the history of record
            await _context.SaveChangesAsync();

    //NoContent vs Sending back updated. We don't return anything if we update a record in database, Yes its response is returned
            return NoContent();
        }


        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var bookToDelete = await _context.Books.FindAsync(id);
            if (bookToDelete == null) return NotFound();

            //Books.Remove is LinQ to delete the bookToDelete from our Books Collection. It is equivalent to Drop query of SQL
            _context.Books.Remove(bookToDelete); 
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
