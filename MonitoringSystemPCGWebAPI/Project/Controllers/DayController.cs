using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections;
using Utilities.Interfaces;

namespace MonitoringSystemPCGWebAPI.Project.Controllers
{
    [Route("api/day")]
    [ApiController]
    public class DayController : ControllerBase
    {
        private readonly IDayUtility _dayUtility;
        public DayController(IDayUtility dayUtility)
        {
            _dayUtility = dayUtility;
        }
        [HttpGet("count")]
        public async Task<IActionResult> Compute(DateTime start, DateTime end, bool isMandatory = false)
        {
            try
            {
                decimal days =  _dayUtility.CountDays(start,end,isMandatory);

                return Ok(days);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
