using ConsumerExample.Business;
using ConsumerExample.Shared.Dto;
using Microsoft.AspNetCore.Mvc;
using UniprExample.Shared.Dto;

namespace ConsumerExample.Controllers;

[ApiController]
[Route("webapi/[controller]/[action]")]
public class ConsumerExampleController : ControllerBase {

    private readonly IBusiness _business;
    private readonly ILogger<ConsumerExampleController> _logger;

    public ConsumerExampleController(IBusiness business, ILogger<ConsumerExampleController> logger) {
        _business = business;
        _logger = logger;
    }

    [HttpGet(Name = "GetStudentiKafka")]
    public ActionResult<IEnumerable<StudentiKafkaDto>> GetStudentiKafka() {
        return Ok(_business.GetStudentiKafka());
    }

    [HttpGet(Name = "GetStudenteByKey")]
    public async Task<ActionResult<StudenteDto>> GetStudenteByKey(int matricola) {
        StudenteDto? dto = await _business.GetStudenteByKeyAsync(matricola);
        return dto != null ? Ok(dto) : NotFound();
    }

}