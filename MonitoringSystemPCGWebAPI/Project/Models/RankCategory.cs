
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class RankCategory
    {
		[Key]
		public int? Id {get;set;}
		public string? Name {get;set;}
        public string? Casing { get;set;}
    }
}
