using Microsoft.EntityFrameworkCore;
using UniprExample.Repository.Model;

namespace UniprExample.Repository;

public class UniprExampleDbContext : DbContext {
    public UniprExampleDbContext(DbContextOptions<UniprExampleDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // configurazione del modello tramite API fluent

        modelBuilder.Entity<Studenti>().ToTable("Studenti");
        modelBuilder.Entity<Studenti>().HasKey(s => s.Matricola);

        modelBuilder.Entity<Corsi>().HasKey(c => c.Codice);
        modelBuilder.Entity<Esami>().HasKey(e => new { e.CorsiId, e.StudentiId });
        modelBuilder.Entity<EsamiAlt>().HasKey(e => new { e.ID_CORSO, e.ID_STUDENTE });

        modelBuilder.Entity<TransactionalOutbox>().ToTable("TransactionalOutbox");
        modelBuilder.Entity<TransactionalOutbox>().HasKey(e => new { e.Id });

        modelBuilder.Entity<Studenti>()
            .HasMany(e => e.ListaEsamiAlt)
            .WithOne(e => e.Studente)
            .HasForeignKey(e => new { e.ID_STUDENTE });

        modelBuilder.Entity<Corsi>()
            .HasMany(e => e.ListaEsamiAlt)
            .WithOne(e => e.Corso)
            .HasForeignKey(e => new { e.ID_CORSO });
    }

    public DbSet<Studenti> Studenti { get; set; }
    public DbSet<Corsi> Corsi { get; set; }
    public DbSet<Esami> Esami { get; set; }
    public DbSet<EsamiAlt> EsamiAlt { get; set; }
    public DbSet<TransactionalOutbox> TransactionalOutboxList { get; set; }
}
