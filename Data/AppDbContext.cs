using gql_netcore.Models;
using Microsoft.EntityFrameworkCore;

namespace gql_netcore.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Platform> Platforms { get; set; }
    }
}