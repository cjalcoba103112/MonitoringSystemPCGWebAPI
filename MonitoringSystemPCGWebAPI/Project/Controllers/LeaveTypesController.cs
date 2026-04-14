
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/leave-types")]
    [ApiController]
    public class LeaveTypesController : ControllerBase
    {
        private readonly ILeaveTypesService _leaveTypesService;

        public LeaveTypesController(ILeaveTypesService leaveTypesService)
        {
            _leaveTypesService = leaveTypesService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery]LeaveTypes filter)
        {
            try
            {
                IEnumerable<LeaveTypes> data = await _leaveTypesService.GetAllAsync(filter);
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
                LeaveTypes? data = await _leaveTypesService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]LeaveTypes leaveTypes)
        {
            try
            {
                LeaveTypes? data = await _leaveTypesService.InsertAsync(leaveTypes);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]LeaveTypes leaveTypes)
        {
            try
            {
                if(id != leaveTypes.LeaveTypeID) return BadRequest("Id mismatched.");

                LeaveTypes? data = await _leaveTypesService.GetByIdAsync(id);
                if (data == null) return NotFound();

                LeaveTypes? updatedData = await _leaveTypesService.UpdateAsync(leaveTypes); 
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
                LeaveTypes? data = await _leaveTypesService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _leaveTypesService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<LeaveTypes> listData)
        {
            try
            {
                IEnumerable<LeaveTypes> data = await _leaveTypesService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<LeaveTypes> listData)
        {
            try
            {
                IEnumerable<LeaveTypes> data = await _leaveTypesService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<LeaveTypes> listData)
        {
            try
            {
                IEnumerable<LeaveTypes> data = await _leaveTypesService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<LeaveTypes> listData)
        {
            try
            {
                IEnumerable<LeaveTypes> data = await _leaveTypesService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
