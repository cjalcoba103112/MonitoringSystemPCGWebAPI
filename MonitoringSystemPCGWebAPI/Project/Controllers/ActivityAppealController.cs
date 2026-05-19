
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/activity-appeal")]
    [ApiController]
    public class ActivityAppealController : ControllerBase
    {
        private readonly IActivityAppealService _activityAppealService;


        public ActivityAppealController(IActivityAppealService activityAppealService)
        {
            _activityAppealService = activityAppealService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery]ActivityAppeal filter)
        {
            try
            {
                IEnumerable<ActivityAppeal> data = await _activityAppealService.GetAllAsync(filter);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }
        [HttpGet("token/{token}")]
        public async Task<IActionResult> GetByTokenAsync(string token)
        {
            try
            {
                ActivityAppeal? data = await _activityAppealService.GetByTokenAsync(token);
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
                ActivityAppeal? data = await _activityAppealService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]ActivityAppeal activityAppeal)
        {
            try
            {
                ActivityAppeal? data = await _activityAppealService.InsertAsync(activityAppeal);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("submit/{id}")]
        public async Task<IActionResult> SubmitAsync(int id, [FromBody] ActivityAppeal activityAppeal)
        {
            try
            {
                if (id != activityAppeal.Id) return BadRequest("Id mismatched.");   

                ActivityAppeal? data = await _activityAppealService.GetByIdAsync(id);
                if (data == null) return NotFound();

                ActivityAppeal? updatedData = await _activityAppealService.SubmitAsync(activityAppeal);
                return Ok(updatedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]ActivityAppeal activityAppeal)
        {
            try
            {
                if(id != activityAppeal.Id) return BadRequest("Id mismatched.");

                ActivityAppeal? data = await _activityAppealService.GetByIdAsync(id);
                if (data == null) return NotFound();

                ActivityAppeal? updatedData = await _activityAppealService.SubmitAsync  (activityAppeal); 
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
                ActivityAppeal? data = await _activityAppealService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _activityAppealService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<ActivityAppeal> listData)
        {
            try
            {
                IEnumerable<ActivityAppeal> data = await _activityAppealService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<ActivityAppeal> listData)
        {
            try
            {
                IEnumerable<ActivityAppeal> data = await _activityAppealService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<ActivityAppeal> listData)
        {
            try
            {
                IEnumerable<ActivityAppeal> data = await _activityAppealService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<ActivityAppeal> listData)
        {
            try
            {
                IEnumerable<ActivityAppeal> data = await _activityAppealService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
