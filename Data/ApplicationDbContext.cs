using JavaExamFinal.Models;
using Microsoft.EntityFrameworkCore;
namespace JavaExamFinal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CaThi>? CaThi { get; set; }
        public DbSet<Student>? Student { get; set; }
        public DbSet<DangKyThi>? DangKyThi { get; set; }
    }
}
