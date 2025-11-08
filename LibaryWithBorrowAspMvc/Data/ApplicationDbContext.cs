using LibaryWithBorrowAspMvc.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibaryWithBorrowAspMvc.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
