using Microsoft.EntityFrameworkCore;
using UploadImageNET.Models;

namespace UploadImageNET.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        public DbSet<ImageData> ImgData { get; set; }
    }
}
