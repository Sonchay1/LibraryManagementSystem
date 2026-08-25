using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context) => _context = context;

        // GET: Dashboard 
        public async Task<IActionResult> Index()
        {
            var viewModel = new DashboardViewModel
            {
                Authors = await _context.Authors.Include(a => a.Books).ToListAsync(),
                Books = await _context.Books.Include(b => b.Author).Include(b => b.Categories).ToListAsync(),
                Categories = await _context.Categories.ToListAsync(),
                Members = await _context.Members.ToListAsync(),
                BorrowRecords = await _context.BorrowRecords
                    .Include(br => br.Book)
                    .Include(br => br.Member)
                    .OrderByDescending(br => br.BorrowDate)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        //  Author Quick Add 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAuthor(string name, string? bio)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _context.Authors.Add(new Author { Name = name, Bio = bio });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        //  Category Quick Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                _context.Categories.Add(new Category { Name = name });
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        //  Book Quick Add 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddBook(string title, string? isbn, int publishedYear, int totalCopies, int authorId, int[]? selectedCategories)
        {
            if (!string.IsNullOrWhiteSpace(title) && authorId > 0)
            {
                var book = new Book
                {
                    Title = title,
                    ISBN = isbn,
                    PublishedYear = publishedYear,
                    TotalCopies = totalCopies,
                    AvailableCopies = totalCopies,
                    AuthorId = authorId
                };

                if (selectedCategories != null && selectedCategories.Length > 0)
                {
                    var categoryList = selectedCategories.ToList();
                    var categories = await _context.Categories
                        .Where(c => categoryList.Contains(c.Id))
                        .ToListAsync();

                    foreach (var cat in categories)
                        book.Categories.Add(cat);
                }

                _context.Books.Add(book);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        //  Member Quick Add 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMember(string name, string email, string? phone, string? photoBase64)
        {
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(email))
            {
                var member = new Member
                {
                    Name = name,
                    Email = email,
                    Phone = phone
                };

                if (!string.IsNullOrEmpty(photoBase64))
                {
                    member.PhotoBytes = Convert.FromBase64String(photoBase64);
                }

                _context.Members.Add(member);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // নতুন মেথডটাও যোগ করো (না থাকলে)
        public async Task<IActionResult> MemberPhoto(int id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member?.PhotoBytes == null || member.PhotoBytes.Length == 0)
                return NotFound();

            return File(member.PhotoBytes, "image/jpeg");
        }
        //  Borrow Quick Add 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BorrowBook(int bookId, int memberId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book != null && book.AvailableCopies > 0)
            {
                var record = new BorrowRecord
                {
                    BookId = bookId,
                    MemberId = memberId,
                    BorrowDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(14),
                    IsReturned = false
                };

                book.AvailableCopies -= 1;
                _context.BorrowRecords.Add(record);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        //  Return Book 
        [HttpPost]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var record = await _context.BorrowRecords
                .Include(br => br.Book)
                .FirstOrDefaultAsync(br => br.Id == id);

            if (record != null && !record.IsReturned)
            {
                record.ReturnDate = DateTime.Now;
                record.IsReturned = true;

                if (record.ReturnDate > record.DueDate)
                {
                    int lateDays = (record.ReturnDate.Value - record.DueDate).Days;
                    record.FineAmount = lateDays * 10;
                }

                if (record.Book != null)
                    record.Book.AvailableCopies += 1;

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        //  Delete Actions
        [HttpPost]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author != null) _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null) _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null) _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}