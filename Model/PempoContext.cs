using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pempo_backend.Model
{
    public class PempoContext : DbContext
    {
        public PempoContext (DbContextOptions<PempoContext> options)
            : base(options)
        {

        }
        public DbSet<Admin> tblAdmin { get; set; }
        public DbSet<Post> tblPost { get; set;  }
        public DbSet<Media> tblMedia { get; set; }
        public DbSet<Comment> tblComments { get; set; }
        public DbSet<Likes> tblLikes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-J2HSJ9J\\SQLEXPRESS;initial catalog=pempo;User ID=omo;Password=1234;MultipleActiveResultSets=True;App=EntityFramework");
        }
    }
}
