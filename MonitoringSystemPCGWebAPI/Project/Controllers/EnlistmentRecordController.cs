
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/enlistment-record")]
    [ApiController]
    public class EnlistmentRecordController : ControllerBase
    {
        private readonly IEnlistmentRecordService _enlistmentRecordService;
        

        public EnlistmentRecordController(IEnlistmentRecordService enlistmentRecordService)
        {
            _enlistmentRecordService = enlistmentRecordService;
        }

        [HttpPost("request-explanation")]
        public async Task<IActionResult> GetAllAsync([FromQuery] EnlistmentRecord filter)
        {
            try
            {
                IEnumerable<EnlistmentRecord> data = await _enlistmentRecordService.GetAllAsync(filter);
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
                EnlistmentRecord? data = await _enlistmentRecordService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]EnlistmentRecord enlistmentRecord)
        {
            try
            {
                EnlistmentRecord? data = await _enlistmentRecordService.InsertAsync(enlistmentRecord);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]EnlistmentRecord enlistmentRecord)
        {
            try
            {
                if(id != enlistmentRecord.EnlistmentId) return BadRequest("Id mismatched.");

                EnlistmentRecord? data = await _enlistmentRecordService.GetByIdAsync(id);
                if (data == null) return NotFound();

                EnlistmentRecord? updatedData = await _enlistmentRecordService.UpdateAsync(enlistmentRecord); 
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
                EnlistmentRecord? data = await _enlistmentRecordService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _enlistmentRecordService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<EnlistmentRecord> listData)
        {
            try
            {
                IEnumerable<EnlistmentRecord> data = await _enlistmentRecordService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<EnlistmentRecord> listData)
        {
            try
            {
                IEnumerable<EnlistmentRecord> data = await _enlistmentRecordService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<EnlistmentRecord> listData)
        {
            try
            {
                IEnumerable<EnlistmentRecord> data = await _enlistmentRecordService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<EnlistmentRecord> listData)
        {
            try
            {
                IEnumerable<EnlistmentRecord> data = await _enlistmentRecordService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
