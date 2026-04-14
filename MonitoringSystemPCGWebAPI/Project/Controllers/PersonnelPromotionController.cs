
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

namespace ApiControllers
{
    [Route("api/personnel-promotion")]
    [ApiController]
    public class PersonnelPromotionController : ControllerBase
    {
        private readonly IPersonnelPromotionService _personnelPromotionService;

        public PersonnelPromotionController(IPersonnelPromotionService personnelPromotionService)
        {
            _personnelPromotionService = personnelPromotionService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllAsync([FromQuery]PersonnelPromotion filter)
        {
            try
            {
                IEnumerable<PersonnelPromotion> data = await _personnelPromotionService.GetAllAsync(filter);
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
                PersonnelPromotion? data = await _personnelPromotionService.GetByIdAsync(id);
                if (data == null) return NoContent();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertAsync([FromBody]PersonnelPromotion personnelPromotion)
        {
            try
            {
                PersonnelPromotion? data = await _personnelPromotionService.InsertAsync(personnelPromotion);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]PersonnelPromotion personnelPromotion)
        {
            try
            {
                if(id != personnelPromotion.Id) return BadRequest("Id mismatched.");

                PersonnelPromotion? data = await _personnelPromotionService.GetByIdAsync(id);
                if (data == null) return NotFound();

                PersonnelPromotion? updatedData = await _personnelPromotionService.UpdateAsync(personnelPromotion); 
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
                PersonnelPromotion? data = await _personnelPromotionService.GetByIdAsync(id);
                if (data == null) return NotFound();

                var deletedData = await _personnelPromotionService.DeleteByIdAsync(id);
                return Ok(deletedData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkInsertAsync([FromBody]List<PersonnelPromotion> listData)
        {
            try
            {
                IEnumerable<PersonnelPromotion> data = await _personnelPromotionService.BulkInsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPatch("bulk")]
        public async Task<IActionResult> BulkUpdateAsync([FromBody] List<PersonnelPromotion> listData)
        {
            try
            {
                IEnumerable<PersonnelPromotion> data = await _personnelPromotionService.BulkUpdateAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-upsert")]
        public async Task<IActionResult> BulkUpsertAsync([FromBody] List<PersonnelPromotion> listData)
        {
            try
            {
                IEnumerable<PersonnelPromotion> data = await _personnelPromotionService.BulkUpsertAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("bulk-merge")]
        public async Task<IActionResult> BulkMergeAsync([FromBody] List<PersonnelPromotion> listData)
        {
            try
            {
                IEnumerable<PersonnelPromotion> data = await _personnelPromotionService.BulkMergeAsync(listData);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
