
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/rank-category")]
    [ApiController]
    public class RankCategoryController : ControllerBase
    {
        private readonly IRankCategoryService _rankCategoryService;

        public RankCategoryController(IRankCategoryService rankCategoryService)
        {
            _rankCategoryService = rankCategoryService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery]RankCategory filter)
        {
            try
            {
                IEnumerable<RankCategory> data = await _rankCategoryService.GetAllAsync(filter);
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
                RankCategory? data = await _rankCategoryService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]RankCategory rankCategory)
        {
            try
            {
                RankCategory? data = await _rankCategoryService.InsertAsync(rankCategory);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]RankCategory rankCategory)
        {
            try
            {
                if(id != rankCategory.Id) return BadRequest("Id mismatched.");

                RankCategory? data = await _rankCategoryService.GetByIdAsync(id);
                if (data == null) return NotFound();

                RankCategory? updatedData = await _rankCategoryService.UpdateAsync(rankCategory); 
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
                RankCategory? data = await _rankCategoryService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _rankCategoryService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<RankCategory> listData)
        {
            try
            {
                IEnumerable<RankCategory> data = await _rankCategoryService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<RankCategory> listData)
        {
            try
            {
                IEnumerable<RankCategory> data = await _rankCategoryService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<RankCategory> listData)
        {
            try
            {
                IEnumerable<RankCategory> data = await _rankCategoryService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<RankCategory> listData)
        {
            try
            {
                IEnumerable<RankCategory> data = await _rankCategoryService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
