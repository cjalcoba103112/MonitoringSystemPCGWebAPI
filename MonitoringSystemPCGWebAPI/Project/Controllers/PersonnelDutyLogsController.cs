
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/personnel-duty-logs")]
    [ApiController]
    public class PersonnelDutyLogsController : ControllerBase
    {
        private readonly IPersonnelDutyLogsService _personnelDutyLogsService;

        public PersonnelDutyLogsController(IPersonnelDutyLogsService personnelDutyLogsService)
        {
            _personnelDutyLogsService = personnelDutyLogsService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery]PersonnelDutyLogs filter)
        {
            try
            {
                IEnumerable<PersonnelDutyLogs> data = await _personnelDutyLogsService.GetAllAsync(filter);
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
                PersonnelDutyLogs? data = await _personnelDutyLogsService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]PersonnelDutyLogs personnelDutyLogs)
        {
            try
            {
                PersonnelDutyLogs? data = await _personnelDutyLogsService.InsertAsync(personnelDutyLogs);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]PersonnelDutyLogs personnelDutyLogs)
        {
            try
            {
                if(id != personnelDutyLogs.Id) return BadRequest("Id mismatched.");

                PersonnelDutyLogs? data = await _personnelDutyLogsService.GetByIdAsync(id);
                if (data == null) return NotFound();

                PersonnelDutyLogs? updatedData = await _personnelDutyLogsService.UpdateAsync(personnelDutyLogs); 
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
                PersonnelDutyLogs? data = await _personnelDutyLogsService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _personnelDutyLogsService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<PersonnelDutyLogs> listData)
        {
            try
            {
                IEnumerable<PersonnelDutyLogs> data = await _personnelDutyLogsService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<PersonnelDutyLogs> listData)
        {
            try
            {
                IEnumerable<PersonnelDutyLogs> data = await _personnelDutyLogsService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<PersonnelDutyLogs> listData)
        {
            try
            {
                IEnumerable<PersonnelDutyLogs> data = await _personnelDutyLogsService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<PersonnelDutyLogs> listData)
        {
            try
            {
                IEnumerable<PersonnelDutyLogs> data = await _personnelDutyLogsService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
