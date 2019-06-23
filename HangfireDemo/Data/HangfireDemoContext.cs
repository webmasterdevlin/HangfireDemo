
using HangfireDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace HangfireDemo.Data
{
    public class HangfireDemoContext : DbContext
    {
        public HangfireDemoContext (DbContextOptions<HangfireDemoContext> options)
            : base(options)
        {
        }

        public DbSet<Sale> Sale { get; set; }
    }
}
