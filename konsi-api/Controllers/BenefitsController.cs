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
        public async Task<ActionResult<IEnumerable<Benefit>>> GetBenefits([FromQuery][Required] string cpf)
        {
            var personIdentification = new PersonIdentification(cpf);

            if (personIdentification.IsValid())
                return BadRequest("Invalid 'CPF'");

            var result = await _elasticService.GetBeneficiaryByCpf(personIdentification.GetCpf());

            if(result is not null)
                return Ok(result.Benefits);

            return NotFound();
        }

        [HttpPost]
        public ActionResult InputBenefits([FromQuery][Required] string cpf)
        {
            var personIdentification = new PersonIdentification(cpf);

            if (personIdentification.IsValid())
                return BadRequest("Invalid 'CPF'");

            var @event = new CpfSearchedEvent(personIdentification.GetCpf());

            _publishCpfService.Publish(@event);

            return Created();
        }
    }
}
