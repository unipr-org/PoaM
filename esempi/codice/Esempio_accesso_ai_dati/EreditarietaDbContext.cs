using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccessoAiDati
{
    public class Scuola
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Nome { get; set; }

        public List<Docente> Docenti { get; set; }

        public List<Studente> Studenti { get; set; }
    }


    public abstract class Persona
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Cognome { get; set; } = string.Empty;
    }

    public class Studente : Persona
    {
        public int ScuolaId { get; set; }

        public virtual Scuola Scuola { get; set; }
    }

    public class Docente : Persona
    {
        public int ScuolaId { get; set; }

        public virtual Scuola Scuola { get; set; }
    }

    public class EreditarietaDbContext : DbContext
    {

        public EreditarietaDbContext(DbContextOptions<EreditarietaDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configurazione del modello tramite API fluent

            modelBuilder.Entity<Scuola>().HasKey(t => new { t.Id });
            modelBuilder.Entity<Scuola>().ToTable("Scuola");

            modelBuilder.Entity<Persona>().HasKey(t => new { t.Id });
            modelBuilder.Entity<Persona>().ToTable("Persona");
            //modelBuilder.Entity<Persona>().HasDiscriminator<string>("Discriminator");

            modelBuilder.Entity<Studente>()
                .Property(e => e.ScuolaId)
                .HasColumnName(nameof(Studente.ScuolaId));

            modelBuilder.Entity<Docente>()
                .Property(e => e.ScuolaId)
                .HasColumnName(nameof(Docente.ScuolaId));
        }

        public DbSet<Scuola> Scuole { get; set; }
        public DbSet<Studente> Studenti { get; set; }
        public DbSet<Docente> Docenti { get; set; }
    }
}
