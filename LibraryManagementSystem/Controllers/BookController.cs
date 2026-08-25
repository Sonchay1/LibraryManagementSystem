using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BookController(ApplicationDbContext context) => _context = context;

        // GET: Book (Author আর Category সহ দেখানো)
        public async Task<IActionResult> Index()
        {
            List<Book> books = await _context.Books
                .Include(b => b.Author)      // Related Author একসাথে আনা (Eager Loading)
                .Include(b => b.Categories)  // Related Categories একসাথে আনা
                .ToListAsync();
            return View(books);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Authors = new SelectList(await _context.Authors.ToListAsync(), "Id", "Name");
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, int[] selectedCategories)
        {
            if (ModelState.IsValid)
            {
                book.AvailableCopies = book.TotalCopies; // শুরুতে সব কপি available থাকবে

                // সিলেক্ট করা Category গুলো যোগ করা
                if (selectedCategories != null && selectedCategories.Length > 0)
                {
                    var selectedCategoriesList = selectedCategories?.ToList() ?? new List<int>();

                    var categories = await _context.Categories
                        .Where(c => selectedCategoriesList.Contains(c.Id))
                        .ToListAsync();

                    foreach (var cat in categories)
                        book.Categories.Add(cat);
                }

                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Authors = new SelectList(await _context.Authors.ToListAsync(), "Id", "Name", book.AuthorId);
            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(book);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Categories)
                .Include(b => b.BorrowRecords)
                    .ThenInclude(br => br.Member) // Nested Include: BorrowRecord এর ভেতরের Member
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null) return NotFound();
            return View(book);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null) _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}