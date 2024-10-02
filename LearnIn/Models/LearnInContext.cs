using LearnIn.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace LearnIn.Models
{
    public class LearnInContext : IdentityDbContext
    {
        public DbSet<ApplicationUser> User { get; set; }
        public LearnInContext(DbContextOptions<LearnInContext> options) : base(options)
        {

        }
    }
}
