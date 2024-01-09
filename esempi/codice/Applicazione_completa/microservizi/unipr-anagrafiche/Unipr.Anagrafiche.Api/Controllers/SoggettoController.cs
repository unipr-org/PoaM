using Anagrafiche.Business.Abstraction;
using Anagrafiche.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Anagrafiche.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class SoggettoController : ControllerBase
    {

        private readonly IBusiness _business;
        private readonly ILogger<SoggettoController> _logger;

        public SoggettoController(IBusiness business, ILogger<SoggettoController> logger)
        {
            _business = business;
            _logger = logger;
        }

        [HttpPost(Name = "CreateSoggetto")]
        public async Task<ActionResult> CreateSoggetto(SoggettoDto soggettoDto)
        {
            await _business.CreateSoggetto(soggettoDto);

            return Ok("ho fatto");
        }

        [HttpGet(Name = "ReadSoggetto")]
        public async Task<ActionResult<SoggettoDto?>> ReadSoggetto(int idAnagrafica)
        {
            var soggetto = await _business.ReadSoggetto(idAnagrafica);

            return new JsonResult(soggetto);
        }

        [HttpGet(Name = "GetFromDemografico")]
        public async Task<SoggettoDto> GetFromDemografico(string codiceFiscale)
        {
            return await _business.GetFromDemografico(codiceFiscale);
        }
    }
}
