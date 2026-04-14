
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Models;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/personnel")]
    [ApiController]
    public class PersonnelController : ControllerBase
    {
        private readonly IPersonnelService _personnelService;

        public PersonnelController(IPersonnelService personnelService)
        {
            _personnelService = personnelService;
        }

        [HttpGet("{id}/credits")]
        public async Task<IActionResult> GetPersonnelCredits(int id, int? activityTypeId,int?year=null,DateTime?date=null)
        {
            var result = await _personnelService
                .GetPersonnelCreditsAsync(id, activityTypeId, year,date);

            if (result == null)
                return NotFound("Personnel not found");

            return Ok(result);
        }

        [HttpGet("list/enlistment/ete")]
        public async Task<IActionResult> GetEnlismentETEAsync([FromQuery] Personnel filter)
        {
            try
            {
                IEnumerable<Personnel> data = await _personnelService.GetEnlismentETE(filter);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery] Personnel filter)
        {
            try
            {
                IEnumerable<Personnel> data = await _personnelService.GetAllAsync(filter);
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
                Personnel? data = await _personnelService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
            [HttpPost]
            public async Task<IActionResult> InsertAsync([FromForm] Personnel    personnel, IFormFile? profileImage)
            {
                try
                {
                    Personnel? data = await _personnelService.InsertAsync(personnel, profileImage);
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            [HttpPatch("{id}")]
            public async Task<IActionResult> UpdateAsync(int id, [FromForm] Personnel personnel, IFormFile? profileImage)
            {
                try
                {
                    if (id != personnel.PersonnelId) return BadRequest("Id mismatched.");

                    Personnel? data = await _personnelService.GetByIdAsync(id);
                    if (data == null) return NotFound();

                    Personnel? updatedData = await _personnelService.UpdateAsync(personnel, profileImage);
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
                Personnel? data = await _personnelService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _personnelService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody] List<Personnel> listData)
        {
            try
            {
                IEnumerable<Personnel> data = await _personnelService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<Personnel> listData)
        {
            try
            {
                IEnumerable<Personnel> data = await _personnelService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<Personnel> listData)
        {
            try
            {
                IEnumerable<Personnel> data = await _personnelService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<Personnel> listData)
        {
            try
            {
                IEnumerable<Personnel> data = await _personnelService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
