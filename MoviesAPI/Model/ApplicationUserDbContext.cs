using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MoviesAPI.Model
{
    public class ApplicationUserDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options) : base(options) { }
    }
}
