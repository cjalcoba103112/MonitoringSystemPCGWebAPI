
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class PersonnelPromotion
    {
		[Key]
		public int? Id {get;set;}
		public DateTime? PromotionDate {get;set;}
		public int? PersonnelId {get;set;}

    }
}
