using Anagrafiche.Repository.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml;

namespace Anagrafiche.Repository
{
    public class AnagraficheDbContext : DbContext
    {
        public AnagraficheDbContext(DbContextOptions<AnagraficheDbContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Soggetto>().HasKey(x => x.Id);
            modelBuilder.Entity<Recapito>().HasKey(x => x.Id);
            modelBuilder.Entity<Soggetto>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Recapito>().Property(e => e.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Soggetto>().HasMany(p => p.ListaRecapiti).WithOne(p => p.Soggetto).HasForeignKey(p => p.IdSoggetto);
        }

        public DbSet<Soggetto> Soggetti { get; set; }
        public DbSet<Recapito> Recapiti { get; set; }
    }
}
