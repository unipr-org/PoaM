using AutoMapper;
using Microsoft.Extensions.Logging;
using Pagamenti.Business.Abstraction;
using Pagamenti.Business.Factory;
using Pagamenti.Repository.Abstraction;
using Pagamenti.Shared;

namespace Pagamenti.Business
{
    public class Business : IBusiness
    {
        private readonly IRepository _repository;
        private readonly ILogger<Business> _logger;
        private readonly IMapper _mapper;

        public Business(IRepository repository, ILogger<Business> logger, IMapper map)
        {
            _repository = repository;
            _logger = logger;
            _mapper = map;
        }

        public async Task CreateVersamento(VersamentoInsertDto versamentoInsertDto, CancellationToken cancellationToken = default)
        {
            await _repository.CreateTransaction(async (CancellationToken cancellation) =>
            {
                var versamento = await _repository.CreateVersamento(versamentoInsertDto, cancellation);

                await _repository.SaveChangesAsync(cancellation); // Save intermedia per recuperare l'Id: è autogenerato dal DB e al termine di questa chiamata viene compilato automaticamente sul record

                var newVersamentoRecord = _mapper.Map<VersamentoReadDto>(versamento); // travasiamo il record su un dto (newVersamentoRecord conterrà anche l'Id)

                await _repository.InsertTransactionalOutbox(TransactionalOutboxFactory.CreateInsert(newVersamentoRecord), cancellation);

                await _repository.SaveChangesAsync(cancellation);
            }, cancellationToken);
        }
    }
}
