using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessoAiDati
{
    public class Studenti
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Matricola { get; set; }
        public required string Cognome { get; set; }
        public required string Nome { get; set; }
        public DateTime DataDiNascita { get; set; }
        public List<Esami> ListaEsami { get; set; } = new List<Esami>();
        public List<EsamiAlt> ListaEsamiAlt { get; set; } = new List<EsamiAlt>();
    }

    public class Corsi
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Codice { get; set; }
        public required string Titolo { get; set; }
        public required string Docente { get; set; }
        public List<Esami> ListaEsami { get; set; } = new List<Esami>();
        public List<EsamiAlt> ListaEsamiAlt { get; set; } = new List<EsamiAlt>();
    }

    public class Esami
    {
        public int CorsiId { get; set; }
        public int StudentiId { get; set; }

        public int Voto { get; set; }
        public bool Lode { get; set; }
        public Corsi Corso { get; set; }
        public Studenti Studente { get; set; }
    }

    public class EsamiAlt
    {
        public int ID_CORSO { get; set; }
        public int ID_STUDENTE { get; set; }
        public int Voto { get; set; }
        public bool Lode { get; set; }
        public Corsi Corso { get; set; }
        public Studenti Studente { get; set; }
    }

    public class UniprDbContext : DbContext
    {
        public UniprDbContext(DbContextOptions<UniprDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configurazione del modello tramite API fluent

            modelBuilder.Entity<Studenti>().ToTable("Studenti");

            modelBuilder.Entity<Studenti>().HasKey(s => s.Matricola);
            modelBuilder.Entity<Corsi>().HasKey(c => c.Codice);
            modelBuilder.Entity<Esami>().HasKey(e => new { e.CorsiId, e.StudentiId });
            modelBuilder.Entity<EsamiAlt>().HasKey(e => new { e.ID_CORSO, e.ID_STUDENTE });

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
    }
}
