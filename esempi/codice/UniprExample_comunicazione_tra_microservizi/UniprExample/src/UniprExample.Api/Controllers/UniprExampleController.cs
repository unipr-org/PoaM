using Microsoft.AspNetCore.Mvc;
using UniprExample.Business;
using UniprExample.Shared.Dto;

namespace UniprExample.Api.Controllers
{
    [ApiController]
    [Route("webapi/[controller]/[action]")]
    public class UniprExampleController : ControllerBase {

        private readonly IBusiness _business;
        private readonly ILogger<UniprExampleController> _logger;

        public UniprExampleController(IBusiness business, ILogger<UniprExampleController> logger) {
            _business = business;
            _logger = logger;
        }

        [HttpGet(Name = "GetTuttiGliStudenti")]
        public IEnumerable<StudenteDto> GetTuttiGliStudenti() {
            return _business.GetStudenti();
        }

        [HttpPost("{cognome:required}")]
        public IEnumerable<StudenteDto> GetStudenti(
            [FromRoute(Name = "cognome")] string cognome,
            [FromQuery] string nome) {
            return _business.GetStudenti(cognome, nome);
        }

        [HttpGet(Name = "GetStudenti")]
        public IEnumerable<StudenteDto> GetStudenti(string cognome) {
            return _business.GetStudenti(cognome, "Mario");
        }

        [HttpGet(Name = "GetStudenteByKey")]
        public ActionResult<StudenteDto> GetStudenteByKey(int matricola) {
            StudenteDto? dto = _business.GetStudenteByKey(matricola);
            return dto != null ? Ok(dto) : NotFound();
        }

        [HttpGet(Name = "GetEsamiStudente")]
        public IEnumerable<EsameStudenteDto> GetEsamiStudente(int corsoId)
        {
            return _business.GetEsamiStudente(corsoId);
        }

        [HttpPost(Name = "InsertStudente")]
        public ActionResult<StudenteDto> InsertStudente(StudenteInsertDto insertDto) {
            return Ok(_business.InsertStudente(insertDto));
        }
    }
}