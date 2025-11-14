namespace LibaryWithBorrowAspMvc.Models.Dtos.Book
{
    public class BookFilterOptions
    {
        public bool Descending { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
