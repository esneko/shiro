using Microsoft.EntityFrameworkCore;

namespace Shiro.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
    }
}
