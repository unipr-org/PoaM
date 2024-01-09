using Imu.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace Imu.Repository
{
    public class ImuDbContext : DbContext
    {
        public ImuDbContext(DbContextOptions<ImuDbContext> dbContextOptions)
            : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnagraficaImmobile>().HasKey(x => x.Id);
            modelBuilder.Entity<CategoriaCatastale>().HasKey(x => x.Id);
            modelBuilder.Entity<Immobile>().HasKey(x => x.Id);
            modelBuilder.Entity<TransactionalOutbox>().HasKey(e => new { e.Id });
            modelBuilder.Entity<VersamentoKafka>().HasKey(s => s.Id);

            modelBuilder.Entity<CategoriaCatastale>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Immobile>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<TransactionalOutbox>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<VersamentoKafka>().Property(e => e.Id).ValueGeneratedNever();

            modelBuilder.Entity<CategoriaCatastale>().HasMany(p => p.Immobili).WithOne(p => p.CategoriaCatastale).HasForeignKey(p => p.IdCategoriaCatastale);
            modelBuilder.Entity<Immobile>().HasMany(p => p.AnagraficheImmobile).WithOne(p => p.Immobile).HasForeignKey(p => p.IdImmobile);
        }

        public DbSet<AnagraficaImmobile> AnagraficheImmobili { get; set; }
        public DbSet<CategoriaCatastale> CategorieCatastali { get; set; }
        public DbSet<Immobile> Immobili { get; set; }
        public DbSet<VersamentoKafka> VersamentiKafka { get; set; }
        public DbSet<TransactionalOutbox> TransactionalOutboxList { get; set; }
    }
}
