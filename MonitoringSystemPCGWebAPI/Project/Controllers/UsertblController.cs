
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/usertbl")]
    [ApiController]
    public class UsertblController : ControllerBase
    {
        private readonly IUsertblService _usertblService;

        public UsertblController(IUsertblService usertblService)
        {
            _usertblService = usertblService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery]Usertbl filter)
        {
            try
            {
                IEnumerable<Usertbl> data = await _usertblService.GetAllAsync(filter);
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
                Usertbl? data = await _usertblService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]Usertbl usertbl)
        {
            try
            {
                Usertbl? data = await _usertblService.InsertAsync(usertbl);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]Usertbl usertbl)
        {
            try
            {
                if(id != usertbl.UserId) return BadRequest("Id mismatched.");

                Usertbl? data = await _usertblService.GetByIdAsync(id);
                if (data == null) return NotFound();

                Usertbl? updatedData = await _usertblService.UpdateAsync(usertbl); 
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
                Usertbl? data = await _usertblService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _usertblService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<Usertbl> listData)
        {
            try
            {
                IEnumerable<Usertbl> data = await _usertblService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<Usertbl> listData)
        {
            try
            {
                IEnumerable<Usertbl> data = await _usertblService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<Usertbl> listData)
        {
            try
            {
                IEnumerable<Usertbl> data = await _usertblService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<Usertbl> listData)
        {
            try
            {
                IEnumerable<Usertbl> data = await _usertblService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
