using AutoMapper;
using UniprExample.Business.Factory;
using UniprExample.Repository;
using UniprExample.Repository.Model;
using UniprExample.Shared.Dto;

namespace UniprExample.Business {
    public class Business : IBusiness {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        public Business(IRepository repository, IMapper mapper) {
            _repository = repository;
            _mapper = mapper;
        }

        public List<StudenteDto> GetStudenti() {
            return GetListStudenteDto(_repository.Read().AsEnumerable());
        }

        public List<StudenteDto> GetStudenti(string cognome, string nome) {
            return GetListStudenteDto(_repository.Read(cognome, nome).AsEnumerable());
        }

        public StudenteDto? GetStudenteByKey(int matricola) {
            Studenti? studenti = _repository.GetStudenteByKey(matricola);
            return studenti != null ? _mapper.Map<StudenteDto>(studenti) : null;
        }

        private List<StudenteDto> GetListStudenteDto(IEnumerable<Studenti> studenti) {
            return studenti.Select(_mapper.Map<StudenteDto>).ToList();
        }

        public IEnumerable<EsameStudenteDto> GetEsamiStudente(int corsoId) {
            return _repository.GetEsamiStudenti(corsoId).AsEnumerable().Select(p => {
                EsameStudenteDto dto = _mapper.Map<EsameStudenteDto>(p.Item1);
                _mapper.Map(p.Item2, dto);
                _mapper.Map(p.Item3, dto);
                return dto;
            });
        }

        public StudenteDto InsertStudente(StudenteInsertDto insertDto) {

            StudenteDto dto = new();
            _repository.CreateTransaction(() => {
                Studenti model = _mapper.Map<Studenti>(insertDto);
                _repository.InsertStudente(model);
                _repository.SaveChanges();

                dto = _mapper.Map<StudenteDto>(model);
                _repository.InsertTransactionalOutbox(TransactionalOutboxFactory.CreateInsert(dto));
                _repository.SaveChanges();
            });

            return dto;
        }
    }
}
