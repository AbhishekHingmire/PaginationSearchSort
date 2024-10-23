using Microsoft.EntityFrameworkCore;
using PaginationSearchSort.Models;

namespace PaginationSearchSort.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base (options)
        {
            
        }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Person> Person { get; set; }
    }
}
