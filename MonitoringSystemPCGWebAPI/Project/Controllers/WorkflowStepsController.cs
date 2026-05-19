
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/workflow-steps")]
    [ApiController]
    public class WorkflowStepsController : ControllerBase
    {
        private readonly IWorkflowStepsService _workflowStepsService;

        public WorkflowStepsController(IWorkflowStepsService workflowStepsService)
        {
            _workflowStepsService = workflowStepsService;
        }

        [HttpGet("role/{id}")]
        public async Task<IActionResult> GetByRoleId(int id)
        {
            try
            {
                WorkflowSteps? data = await _workflowStepsService.GetByRoleId(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery]WorkflowSteps filter)
        {
            try
            {
                IEnumerable<WorkflowSteps> data = await _workflowStepsService.GetAllAsync(filter);
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
                WorkflowSteps? data = await _workflowStepsService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]WorkflowSteps workflowSteps)
        {
            try
            {
                WorkflowSteps? data = await _workflowStepsService.InsertAsync(workflowSteps);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]WorkflowSteps workflowSteps)
        {
            try
            {
                if(id != workflowSteps.Id) return BadRequest("Id mismatched.");

                WorkflowSteps? data = await _workflowStepsService.GetByIdAsync(id);
                if (data == null) return NotFound();

                WorkflowSteps? updatedData = await _workflowStepsService.UpdateAsync(workflowSteps); 
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
                WorkflowSteps? data = await _workflowStepsService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _workflowStepsService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<WorkflowSteps> listData)
        {
            try
            {
                IEnumerable<WorkflowSteps> data = await _workflowStepsService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<WorkflowSteps> listData)
        {
            try
            {
                IEnumerable<WorkflowSteps> data = await _workflowStepsService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }   
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<WorkflowSteps> listData)
        {
            try
            {
                IEnumerable<WorkflowSteps> data = await _workflowStepsService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<WorkflowSteps> listData)
        {
            try
            {
                IEnumerable<WorkflowSteps> data = await _workflowStepsService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
