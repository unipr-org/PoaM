using Microsoft.AspNetCore.Mvc;
using Pagamenti.Business.Abstraction;
using Pagamenti.Shared;

namespace Anagrafiche.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PagamentiController : ControllerBase
    {

        private readonly IBusiness _business;
        private readonly ILogger<PagamentiController> _logger;

        public PagamentiController(IBusiness business, ILogger<PagamentiController> logger)
        {
            _business = business;
            _logger = logger;
        }

        [HttpPost(Name = "CreateVersamento")]
        public async Task<ActionResult> CreateVersamento(VersamentoInsertDto versamentoInsertDto)
        {
            await _business.CreateVersamento(versamentoInsertDto);

            return Ok();
        }
    }
}
