using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class BorrowRecord
    {
        public int Id { get; set; }

        
        public int BookId { get; set; }
        public Book? Book { get; set; }

       
        public int MemberId { get; set; }
        public Member? Member { get; set; }

        public DateTime BorrowDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public bool IsReturned { get; set; } = false;

        [DataType(DataType.Currency)]
        public decimal FineAmount { get; set; } = 0;
    }
}