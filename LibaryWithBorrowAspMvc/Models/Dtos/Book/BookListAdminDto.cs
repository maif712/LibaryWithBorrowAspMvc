namespace LibaryWithBorrowAspMvc.Models.Dtos.Book
{
    public class BookListAdminDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsBorrowed { get; set; }
        public string CategoryName { get; set; }
        public DateTimeOffset CreateAtByAdmin { get; set; }
    }
}
