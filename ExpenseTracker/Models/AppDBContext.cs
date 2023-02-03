using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ExpenseTracker.Models
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> opt):base(opt)
        {

        }
        public DbSet<Expense> Expenses { get; set; }
    }
}
