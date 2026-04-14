
using Microsoft.AspNetCore.Mvc;
using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/personnel-activity")]
    [ApiController]
    public class PersonnelActivityController : ControllerBase
    {
        private readonly IPersonnelActivityService _personnelActivityService;

        public PersonnelActivityController(IPersonnelActivityService personnelActivityService)
        {
            _personnelActivityService = personnelActivityService;
        }
        [HttpPost("approve")]
        public async Task<IActionResult> Approve([FromBody] ApprovalRequest request)
        {
            try
            {
                var result = await _personnelActivityService.ApproveAsync(request.Id, request.Remarks);

                if (result == null)
                    return NotFound(new { message = "Activity record not found." });

                return Ok(new { message = "Request approved and email sent successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("decline")]
        public async Task<IActionResult> Decline([FromBody] ApprovalRequest request)
        {
            try
            {
                var result = await _personnelActivityService.DeclineAsync(request.Id, request.Remarks);

                if (result == null)
                    return NotFound(new { message = "Activity record not found." });

                return Ok(new { message = "Request declined and personnel notified." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    
        [HttpGet("{personnelId}/list")]
        public async Task<ActionResult<IEnumerable<PersonnelActivity>>> GetByPersonnel(int personnelId, [FromQuery] int? year)
        {
            try
            {
                var activities = await _personnelActivityService.GetActivitiesByPersonnelAsync(personnelId, year);

                if (activities == null)
                {
                    return NotFound(new { message = "Personnel not found or no activities recorded." });
                }

                return Ok(activities);
            }
            catch (Exception ex)
            {
                // Log the error here
                return StatusCode(500, new { message = "An error occurred while fetching activities.", detail = ex.Message });
            }
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery]PersonnelActivity filter)
        {
            try
            {
                IEnumerable<PersonnelActivityDTO> data = await _personnelActivityService.GetAllAsync(filter);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                PersonnelActivity? data = await _personnelActivityService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]PersonnelActivity personnelActivity)
        {
            try
            {
                PersonnelActivity? data = await _personnelActivityService.InsertAsync(personnelActivity);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]PersonnelActivity personnelActivity)
        {
            try
            {
                if(id != personnelActivity.PersonnelActivityId) return BadRequest("Id mismatched.");

                PersonnelActivity? data = await _personnelActivityService.GetByIdAsync(id);
                if (data == null) return NotFound();

                PersonnelActivity? updatedData = await _personnelActivityService.UpdateAsync(personnelActivity); 
                return Ok(updatedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            try
            {
                PersonnelActivity? data = await _personnelActivityService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _personnelActivityService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<PersonnelActivity> listData)
        {
            try
            {
                IEnumerable<PersonnelActivity> data = await _personnelActivityService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<PersonnelActivity> listData)
        {
            try
            {
                IEnumerable<PersonnelActivity> data = await _personnelActivityService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<PersonnelActivity> listData)
        {
            try
            {
                IEnumerable<PersonnelActivity> data = await _personnelActivityService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<PersonnelActivity> listData)
        {
            try
            {
                IEnumerable<PersonnelActivity> data = await _personnelActivityService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
