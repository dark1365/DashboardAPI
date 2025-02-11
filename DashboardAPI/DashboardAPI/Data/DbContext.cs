using DashboardAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace DashboardAPI.Data
{
    public class MenuContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<MenuItem> MenuItem { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public MenuContext(DbContextOptions<MenuContext> options) : base(options) 
        {
        
        
        }
    }
}
