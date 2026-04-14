
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/sidebar")]
    [ApiController]
    public class SidebarController : ControllerBase
    {
        private readonly ISidebarService _sidebarService;

        public SidebarController(ISidebarService sidebarService)
        {
            _sidebarService = sidebarService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery]Sidebar filter)
        {
            try
            {
                IEnumerable<Sidebar> data = await _sidebarService.GetAllAsync(filter);
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
                Sidebar? data = await _sidebarService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]Sidebar sidebar)
        {
            try
            {
                Sidebar? data = await _sidebarService.InsertAsync(sidebar);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]Sidebar sidebar)
        {
            try
            {
                if(id != sidebar.SidebarId) return BadRequest("Id mismatched.");

                Sidebar? data = await _sidebarService.GetByIdAsync(id);
                if (data == null) return NotFound();

                Sidebar? updatedData = await _sidebarService.UpdateAsync(sidebar); 
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
                Sidebar? data = await _sidebarService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _sidebarService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<Sidebar> listData)
        {
            try
            {
                IEnumerable<Sidebar> data = await _sidebarService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<Sidebar> listData)
        {
            try
            {
                IEnumerable<Sidebar> data = await _sidebarService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<Sidebar> listData)
        {
            try
            {
                IEnumerable<Sidebar> data = await _sidebarService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<Sidebar> listData)
        {
            try
            {
                IEnumerable<Sidebar> data = await _sidebarService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
