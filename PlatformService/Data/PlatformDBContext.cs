using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformDBContext : DbContext
    {
        public PlatformDBContext(DbContextOptions<PlatformDBContext> options) : base(options)
        {

        }

        public DbSet<Platform> Platforms { get; set; }
    }
}
