using Anagrafiche.Repository.Abstraction;
using Anagrafiche.Repository.Model;
using Microsoft.EntityFrameworkCore;

namespace Anagrafiche.Repository
{
    public class Repository : IRepository
    {
        private AnagraficheDbContext _anagraficeDbContext;
        public Repository(AnagraficheDbContext anagraficheDbContext)
        {
            _anagraficeDbContext = anagraficheDbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _anagraficeDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task CreateSoggetto(string nome, string cognome, string codiceFiscale, DateTime dataDiNascita, CancellationToken cancellationToken = default)
        {
            Soggetto soggetto = new Soggetto();
            soggetto.Nome = nome;
            soggetto.Cognome = cognome;
            soggetto.CodiceFiscale = codiceFiscale;
            soggetto.DataDiNascita = dataDiNascita;

            await _anagraficeDbContext.Soggetti.AddAsync(soggetto, cancellationToken);
        }

        public async Task<Soggetto?> ReadSoggetto(int idAnagrafica, CancellationToken cancellationToken = default)
        {
            return await _anagraficeDbContext.Soggetti.Where(p => p.Id == idAnagrafica).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
