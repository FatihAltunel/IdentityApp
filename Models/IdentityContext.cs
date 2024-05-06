using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityApp.Models{
    public class IdentityContext: IdentityDbContext<IdentityUser>{
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options){ }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
            optionsBuilder.UseSqlite("Data Source=database.db");
        }
    }
}