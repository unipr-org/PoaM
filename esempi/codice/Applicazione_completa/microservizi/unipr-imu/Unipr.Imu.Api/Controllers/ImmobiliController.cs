using Imu.Business.Abstraction;
using Imu.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Imu.Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ImmobiliController : ControllerBase
    {

        private readonly IBusiness _business;
        private readonly ILogger<ImmobiliController> _logger;

        public ImmobiliController(IBusiness business, ILogger<ImmobiliController> logger)
        {
            _business = business;
            _logger = logger;
        }

        [HttpPost(Name = "AssociaAnagraficaImmobile")]
        public async Task<ActionResult> AssociaAnagraficaImmobile(AssociaAnagraficaImmobileDto associaAnagraficaImmobileDto)
        {
            await _business.AssociaAnagraficaImmobile(associaAnagraficaImmobileDto);

            return Ok();
        }

        [HttpPost(Name = "CreateCategoriaCatastale")]
        public async Task<ActionResult> CreateCategoriaCatastale(CategoriaCatastaleInsertDto categoriaCatastaleInsertDto)
        {
            await _business.CreateCategoriaCatastale(categoriaCatastaleInsertDto);

            return Ok();
        }

        [HttpPost(Name = "CreateImmobile")]
        public async Task<ActionResult> CreateImmobile(ImmobileInsertDto immobileInsertDto)
        {
            await _business.CreateImmobile(immobileInsertDto);

            return Ok();
        }
    }
}
