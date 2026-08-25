using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BookApiController(ApplicationDbContext context) => _context = context;

        // GET: api/BookApi
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var books = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Categories)
                .ToListAsync();

            return Ok(books);
        }

        // GET: api/BookApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Categories)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();
            return Ok(book);
        }

        // POST: api/BookApi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCreateDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                ISBN = dto.ISBN,
                PublishedYear = dto.PublishedYear,
                TotalCopies = dto.TotalCopies,
                AvailableCopies = dto.TotalCopies,
                AuthorId = dto.AuthorId
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        // PUT: api/BookApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BookCreateDto dto)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            book.Title = dto.Title;
            book.ISBN = dto.ISBN;
            book.PublishedYear = dto.PublishedYear;
            book.TotalCopies = dto.TotalCopies;
            book.AuthorId = dto.AuthorId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/BookApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null) return NotFound();

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }

    // DTO ক্লাস — API রিকোয়েস্ট বডির structure ঠিক করার জন্য
    public class BookCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? ISBN { get; set; }
        public int PublishedYear { get; set; }
        public int TotalCopies { get; set; }
        public int AuthorId { get; set; }
    }
}