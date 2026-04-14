using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;
using MonitoringSystemPCGWebAPI.Project.Services.Interfaces;
using Services.Classes;
using Services.Interfaces;

namespace MonitoringSystemPCGWebAPI.Project.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("personnel-on-going-activities")]
        public async Task<IActionResult> GetPersonnelOnGoingActivities()
        {
            try
            {
                IEnumerable<Personnel> data = await _dashboardService.GetPersonnelOngoingActivities();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("personnel-by-activity-type")]
        public async Task<IActionResult> GetPersonnelByActivityType([FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
        {
            try
            {
                IEnumerable<PersonnelByActivityType> data = await _dashboardService.GetPersonnelByActivityType(startDate,endDate);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("personnel-by-department")]
        public async Task<IActionResult> GetPersonnelByDepartment()
        {
            try
            {
                IEnumerable<PersonnelByDepartment> data = await _dashboardService.GetPersonnelByDepartment();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("personnel-by-department-and-activity")]
        public async Task<IActionResult> GetPersonnelByDepartmentAndActivityAsync(DateTime? startDate = null, DateTime? endDate = null)
        {   
            try
            {
                IEnumerable<PersonnelByDepartmentAndActivity> data = await _dashboardService.GetPersonnelByDepartmentAndActivity();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
