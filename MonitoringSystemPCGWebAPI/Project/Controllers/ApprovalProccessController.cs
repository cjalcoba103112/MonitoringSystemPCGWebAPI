
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/approval-proccess")]
    [ApiController]
    public class ApprovalProccessController : ControllerBase
    {
        private readonly IApprovalProccessService _approvalProccessService;


        public ApprovalProccessController(IApprovalProccessService approvalProccessService)
        {
            _approvalProccessService = approvalProccessService;
        }

        [HttpPost("cmaa")]
        public async Task<ActionResult<ApprovalProccess>> UpdateByCMAA([FromBody] ApprovalProccess data)
        {
            var result = await _approvalProccessService.UpdateByCMAA(data);
            if (result == null) return NotFound("Approval record not found.");

            return Ok(result);
        }

        [HttpPost("oic")]
        public async Task<ActionResult<ApprovalProccess>> UpdateByOIC([FromBody] ApprovalProccess data)
        {
            var result = await _approvalProccessService.UpdateByOIC(data);
            if (result == null) return NotFound("Approval record not found.");

            return Ok(result);
        }

        [HttpPost("csg")]
        public async Task<ActionResult<ApprovalProccess>> UpdateByCSG([FromBody] ApprovalProccess data)
        {
            var result = await _approvalProccessService.UpdateByCSG(data);
            if (result == null) return NotFound("Approval record not found.");

            return Ok(result);
        }

        [HttpPost("co/{personnelActivityId}")]
        public async Task<ActionResult<ApprovalProccess>> UpdateByCO([FromBody] ApprovalProccess data, int personnelActivityId)
        {
            // The CO method includes the extra personnelActivityId parameter as per your Task signature
            var result = await _approvalProccessService.UpdateByCO(data, personnelActivityId);
            if (result == null) return NotFound("Approval record or Activity ID not found.");

            return Ok(result);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery]ApprovalProccess filter)
        {
            try
            {
                IEnumerable<ApprovalProccess> data = await _approvalProccessService.GetAllAsync(filter);
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
                ApprovalProccess? data = await _approvalProccessService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]ApprovalProccess approvalProccess)
        {
            try
            {
                ApprovalProccess? data = await _approvalProccessService.InsertAsync(approvalProccess);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]ApprovalProccess approvalProccess)
        {
            try
            {
                if(id != approvalProccess.Id) return BadRequest("Id mismatched.");

                ApprovalProccess? data = await _approvalProccessService.GetByIdAsync(id);
                if (data == null) return NotFound();

                ApprovalProccess? updatedData = await _approvalProccessService.UpdateAsync(approvalProccess); 
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
                ApprovalProccess? data = await _approvalProccessService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _approvalProccessService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<ApprovalProccess> listData)
        {
            try
            {
                IEnumerable<ApprovalProccess> data = await _approvalProccessService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<ApprovalProccess> listData)
        {
            try
            {
                IEnumerable<ApprovalProccess> data = await _approvalProccessService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<ApprovalProccess> listData)
        {
            try
            {
                IEnumerable<ApprovalProccess> data = await _approvalProccessService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<ApprovalProccess> listData)
        {
            try
            {
                IEnumerable<ApprovalProccess> data = await _approvalProccessService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
