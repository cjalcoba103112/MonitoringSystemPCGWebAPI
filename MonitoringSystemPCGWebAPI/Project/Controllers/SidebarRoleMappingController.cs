
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/sidebar-role-mapping")]
    [ApiController]
    public class SidebarRoleMappingController : ControllerBase
    {
        private readonly ISidebarRoleMappingService _sidebarRoleMappingService;

        public SidebarRoleMappingController(ISidebarRoleMappingService sidebarRoleMappingService)
        {
            _sidebarRoleMappingService = sidebarRoleMappingService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery]SidebarRoleMapping filter)
        {
            try
            {
                IEnumerable<SidebarRoleMapping> data = await _sidebarRoleMappingService.GetAllAsync(filter);
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
                SidebarRoleMapping? data = await _sidebarRoleMappingService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]SidebarRoleMapping sidebarRoleMapping)
        {
            try
            {
                SidebarRoleMapping? data = await _sidebarRoleMappingService.InsertAsync(sidebarRoleMapping);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]SidebarRoleMapping sidebarRoleMapping)
        {
            try
            {
                if(id != sidebarRoleMapping.SidebarRoleMappingId) return BadRequest("Id mismatched.");

                SidebarRoleMapping? data = await _sidebarRoleMappingService.GetByIdAsync(id);
                if (data == null) return NotFound();

                SidebarRoleMapping? updatedData = await _sidebarRoleMappingService.UpdateAsync(sidebarRoleMapping); 
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
                SidebarRoleMapping? data = await _sidebarRoleMappingService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _sidebarRoleMappingService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<SidebarRoleMapping> listData)
        {
            try
            {
                IEnumerable<SidebarRoleMapping> data = await _sidebarRoleMappingService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<SidebarRoleMapping> listData)
        {
            try
            {
                IEnumerable<SidebarRoleMapping> data = await _sidebarRoleMappingService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<SidebarRoleMapping> listData)
        {
            try
            {
                IEnumerable<SidebarRoleMapping> data = await _sidebarRoleMappingService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<SidebarRoleMapping> listData)
        {
            try
            {
                IEnumerable<SidebarRoleMapping> data = await _sidebarRoleMappingService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
