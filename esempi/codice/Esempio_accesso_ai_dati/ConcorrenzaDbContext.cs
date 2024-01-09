using Microsoft.EntityFrameworkCore;

namespace AccessoAiDati
{
    public class Prodotti
    {
        public required string CodiceProdotto { get; set; }

        public int Quantita { get; set; }

        public byte[] Version { get; set; } = null!;
    }

    public class ProdottiAlt
    {
        public required string CodiceProdotto { get; set; }

        public int Quantita { get; set; }

        public string Version { get; set; } = null!;
    }

    public class ConcorrenzaDbContext : DbContext
    {
        public ConcorrenzaDbContext(DbContextOptions<ConcorrenzaDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configurazione del modello tramite API fluent

            modelBuilder.Entity<Prodotti>().HasKey(s => s.CodiceProdotto);
            modelBuilder.Entity<ProdottiAlt>().HasKey(s => s.CodiceProdotto);


            /// Gestione ottimistica della concorrenza su SQL Server:
            /// su DB il campo Version è di tipo timestamp ed è compito del DB aggiornare
            /// il suo valore ad ogni modifica del record.
            /// EF Core rileva modifiche concorrenti allo stesso record.
            modelBuilder.Entity<Prodotti>().Property(p => p.Version).IsRowVersion();


            /// Gestione ottimistica della concorrenza su DB alternativi:
            /// su DB il campo Version è di tipo VARCHAR(50) ed è compito del
            /// programmatore aggiornare il suo valore ad ogni modifica del record.
            /// EF Core rileva modifiche concorrenti allo stesso record.
            modelBuilder.Entity<ProdottiAlt>().Property(p => p.Version).IsConcurrencyToken();
        }

        public DbSet<Prodotti> Prodotti { get; set; }
        public DbSet<ProdottiAlt> ProdottiAlt { get; set; }
    }
}
