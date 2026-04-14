
using Microsoft.AspNetCore.Mvc;
using Models;
using MonitoringSystemPCGWebAPI.Project.Models.NonTables;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/otp-verifications")]
    [ApiController]
    public class OtpVerificationsController : ControllerBase
    {
        private readonly IOtpVerificationsService _otpVerificationsService;

        public OtpVerificationsController(IOtpVerificationsService otpVerificationsService)
        {
            _otpVerificationsService = otpVerificationsService;
        }

        //[HttpGet("list")]
        //public async Task<IActionResult> GetAllAsync([FromQuery]OtpVerifications filter)
        //{
        //    try
        //    {
        //        IEnumerable<OtpVerifications> data = await _otpVerificationsService.GetAllAsync(filter);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
           
        //}
        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetByIdAsync(int id)
        //{
        //    try
        //    {
        //        OtpVerifications? data = await _otpVerificationsService.GetByIdAsync(id);
        //        if (data == null) return NoContent();

        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]OtpDTO otp)
        {
            try
            {
                OtpVerifications? data = await _otpVerificationsService.InsertAsync(otp);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPatch("{id}")]
        //public async Task<IActionResult> UpdateAsync(int id,[FromBody]OtpVerifications otpVerifications)
        //{
        //    try
        //    {
        //        if(id != otpVerifications.Id) return BadRequest("Id mismatched.");

        //        OtpVerifications? data = await _otpVerificationsService.GetByIdAsync(id);
        //        if (data == null) return NotFound();

        //        OtpVerifications? updatedData = await _otpVerificationsService.UpdateAsync(otpVerifications); 
        //        return Ok(updatedData);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteByIdAsync(int id)
        //{
        //    try
        //    {
        //        OtpVerifications? data = await _otpVerificationsService.GetByIdAsync(id);
        //        if (data == null) return NotFound();

        //        var deletedData = await _otpVerificationsService.DeleteByIdAsync(id);
        //        return Ok(deletedData);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[HttpPost("bulk")]
        //public async Task<IActionResult> BulkInsertAsync([FromBody]List<OtpVerifications> listData)
        //{
        //    try
        //    {
        //        IEnumerable<OtpVerifications> data = await _otpVerificationsService.BulkInsertAsync(listData);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[HttpPatch("bulk")]
        //public async Task<IActionResult> BulkUpdateAsync([FromBody] List<OtpVerifications> listData)
        //{
        //    try
        //    {
        //        IEnumerable<OtpVerifications> data = await _otpVerificationsService.BulkUpdateAsync(listData);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[HttpPost("bulk-upsert")]
        //public async Task<IActionResult> BulkUpsertAsync([FromBody] List<OtpVerifications> listData)
        //{
        //    try
        //    {
        //        IEnumerable<OtpVerifications> data = await _otpVerificationsService.BulkUpsertAsync(listData);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[HttpPost("bulk-merge")]
        //public async Task<IActionResult> BulkMergeAsync([FromBody] List<OtpVerifications> listData)
        //{
        //    try
        //    {
        //        IEnumerable<OtpVerifications> data = await _otpVerificationsService.BulkMergeAsync(listData);
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        
    }
}
