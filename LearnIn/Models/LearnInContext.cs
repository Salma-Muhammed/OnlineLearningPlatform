using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LearnIn.Models
{
    public class LearnInContext : IdentityDbContext<ApplicationUser>
    {
        public LearnInContext() :base()
        {
            
        }

        public LearnInContext(DbContextOptions options): base(options) 
        {
        
        }
    }
}
