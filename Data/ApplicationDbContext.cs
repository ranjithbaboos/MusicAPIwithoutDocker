using Microsoft.EntityFrameworkCore;
using MusicAPIwithoutDocker.Models;

namespace MusicAPIwithoutDocker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public  DbSet<Song> Songs { get; set; }
    }
}
