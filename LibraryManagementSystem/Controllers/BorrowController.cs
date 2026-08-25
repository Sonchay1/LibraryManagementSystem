using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class BorrowController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BorrowController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var records = await _context.BorrowRecords
                .Include(br => br.Book)
                .Include(br => br.Member)
                .OrderByDescending(br => br.BorrowDate)
                .ToListAsync();
            return View(records);
        }

       
        public async Task<IActionResult> Create()
        {
            
            var availableBooks = await _context.Books
                .Where(b => b.AvailableCopies > 0)
                .ToListAsync();

            ViewBag.Books = new SelectList(availableBooks, "Id", "Title");
            ViewBag.Members = new SelectList(await _context.Members.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int bookId, int memberId)
        {
            var book = await _context.Books.FindAsync(bookId);

            if (book == null || book.AvailableCopies <= 0)
            {
                ModelState.AddModelError("", "এই বইটা এখন available নেই।");
                ViewBag.Books = new SelectList(await _context.Books.Where(b => b.AvailableCopies > 0).ToListAsync(), "Id", "Title");
                ViewBag.Members = new SelectList(await _context.Members.ToListAsync(), "Id", "Name");
                return View();
            }

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

            return RedirectToAction(nameof(Index));
        }

        
        [HttpPost]
        public async Task<IActionResult> ReturnBook(int id)
        {
            var record = await _context.BorrowRecords
                .Include(br => br.Book)
                .FirstOrDefaultAsync(br => br.Id == id);

            if (record == null || record.IsReturned) return NotFound();

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
            return RedirectToAction(nameof(Index));
        }
    }
}