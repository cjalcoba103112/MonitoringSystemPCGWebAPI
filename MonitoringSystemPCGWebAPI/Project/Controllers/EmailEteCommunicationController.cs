
using Microsoft.AspNetCore.Mvc;
using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/email-ete-communication")]
    [ApiController]
    public class EmailEteCommunicationController : ControllerBase
    {
        private readonly IEmailEteCommunicationService _emailEteCommunicationService;

        public EmailEteCommunicationController(IEmailEteCommunicationService emailEteCommunicationService)
        {
            _emailEteCommunicationService = emailEteCommunicationService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery]EmailEteCommunication filter)
        {
            try
            {
                IEnumerable<EmailEteCommunication> data = await _emailEteCommunicationService.GetAllAsync(filter);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpGet("personnel/{personnelId}")]
        public async Task<IActionResult> GetByPersonnelId(int personnelId, [FromQuery] DateTime nextETE)
        {
            try
            {
                var data = await _emailEteCommunicationService.GetByPersonnelId(personnelId, nextETE);

                if (data == null)
                    return NotFound(new { message = "No communication record found for this personnel and ETE date." });

                return Ok(data);
            }
            catch (Exception ex)
            {
                // 3. Return the actual error message for debugging
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("token/{token}")]
        public async Task<IActionResult> GetByToken(string token)
        {
            try
            {
                EmailEteCommunication? data = await _emailEteCommunicationService.GetByToken(token);
                if (data == null) return BadRequest();

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
                EmailEteCommunication? data = await _emailEteCommunicationService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]EmailEteCommunication emailEteCommunication)
        {
            try
            {
                EmailEteCommunication? data = await _emailEteCommunicationService.InsertAsync(emailEteCommunication);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("token/{token}")]
        public async Task<IActionResult> UpdateByTokenAsync(string token, [FromForm] EteSubmissionDto model)
        {
            try
            {
                var existingData = await _emailEteCommunicationService.GetByToken(token);
                if (existingData == null) return NotFound("Invalid or expired link.");

               
                existingData.Response = model.Explanation;
                existingData.ResponseDateTime = DateTime.UtcNow;
                existingData.IsAccessed = true;

                await _emailEteCommunicationService.UpdateAsync(existingData,model.File);
                return Ok(new { message = "Explanation and files saved successfully." });
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
                EmailEteCommunication? data = await _emailEteCommunicationService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _emailEteCommunicationService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<EmailEteCommunication> listData)
        {
            try
            {
                IEnumerable<EmailEteCommunication> data = await _emailEteCommunicationService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<EmailEteCommunication> listData)
        {
            try
            {
                IEnumerable<EmailEteCommunication> data = await _emailEteCommunicationService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<EmailEteCommunication> listData)
        {
            try
            {
                IEnumerable<EmailEteCommunication> data = await _emailEteCommunicationService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<EmailEteCommunication> listData)
        {
            try
            {
                IEnumerable<EmailEteCommunication> data = await _emailEteCommunicationService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
