using Microsoft.EntityFrameworkCore;
using ConsumerExample.Repository.Model;

namespace ConsumerExample.Repository;

public class ConsumerExampleDbContext : DbContext {
    public ConsumerExampleDbContext(DbContextOptions<ConsumerExampleDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // configurazione del modello tramite API fluent

        modelBuilder.Entity<StudentiKafka>().ToTable("StudentiKafka");
        modelBuilder.Entity<StudentiKafka>().HasKey(s => s.Matricola);
    }

    public DbSet<StudentiKafka> StudentiKafkaList { get; set; }
}