using konsi_api.Models;
using konsi_api.Models.Events;
using konsi_api.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace konsi_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BenefitsController : ControllerBase
    {
        private readonly IPublishCpfService _publishCpfService;
        private readonly IElasticService _elasticService;

        public BenefitsController(IPublishCpfService publishCpfService, IElasticService elasticService)
        {
            _publishCpfService = publishCpfService;
            _elasticService = elasticService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Benefit>>> GetBenefits([FromQuery][Required][MaxLength(14)][MinLength(11)] string cpf)
        {
            var result = await _elasticService.GetBeneficiaryByCpf(cpf);

            if(result is not null)
            {
                return Ok(result.Benefits);
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult InputBenefits([FromQuery][Required][MaxLength(14)][MinLength(11)] string cpf)
        {
            var @event = new CpfSearchedEvent(cpf);

            _publishCpfService.Publish(@event);

            return Created();
        }
    }
}
