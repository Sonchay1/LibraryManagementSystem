namespace LibraryManagementSystem.Models
{
    public class DashboardViewModel
    {
        public List<Author> Authors { get; set; } = new();
        public List<Book> Books { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Member> Members { get; set; } = new();
        public List<BorrowRecord> BorrowRecords { get; set; } = new();
    }
}