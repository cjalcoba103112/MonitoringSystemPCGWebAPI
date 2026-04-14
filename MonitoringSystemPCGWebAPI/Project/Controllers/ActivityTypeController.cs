
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/activity-type")]
    [ApiController]
    public class ActivityTypeController : ControllerBase
    {
        private readonly IActivityTypeService _activityTypeService;

        public ActivityTypeController(IActivityTypeService activityTypeService)
        {
            _activityTypeService = activityTypeService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery]ActivityType filter)
        {
            try
            {
                IEnumerable<ActivityType> data = await _activityTypeService.GetAllAsync(filter);
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
                ActivityType? data = await _activityTypeService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]ActivityType activityType)
        {
            try
            {   
                ActivityType? data = await _activityTypeService.InsertAsync(activityType);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]ActivityType activityType)
        {
            try
            {
                if(id != activityType.ActivityTypeId) return BadRequest("Id mismatched.");

                ActivityType? data = await _activityTypeService.GetByIdAsync(id);
                if (data == null) return NotFound();

                ActivityType? updatedData = await _activityTypeService.UpdateAsync(activityType); 
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
                ActivityType? data = await _activityTypeService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _activityTypeService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<ActivityType> listData)
        {
            try
            {
                IEnumerable<ActivityType> data = await _activityTypeService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<ActivityType> listData)
        {
            try
            {
                IEnumerable<ActivityType> data = await _activityTypeService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<ActivityType> listData)
        {
            try
            {
                IEnumerable<ActivityType> data = await _activityTypeService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<ActivityType> listData)
        {
            try
            {
                IEnumerable<ActivityType> data = await _activityTypeService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
