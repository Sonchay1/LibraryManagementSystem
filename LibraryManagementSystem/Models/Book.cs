using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(20)]
        public string? ISBN { get; set; }

        public int PublishedYear { get; set; }

        public int TotalCopies { get; set; } = 1;
        public int AvailableCopies { get; set; } = 1;

        // Foreign Key: One-to-Many (Author -> Books)
        public int AuthorId { get; set; }
        public Author? Author { get; set; }

        // Many-to-Many: Book <-> Category
        public ICollection<Category> Categories { get; set; } = new List<Category>();

        // One-to-Many: Book -> BorrowRecords
        public ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();
    }
}