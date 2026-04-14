
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class Usertbl
    {
		[Key]
		public int? UserId {get;set;}
		public string? FullName { get; set; }
		public string? UserName {get;set;}
		[JsonIgnore]
		public string? Salt {get;set;}
        [JsonIgnore]
        public string? HashedPassword {get;set;}

		public  int? PersonnelId { get; set; }
        public string? Email { get; set; }
        [ForeignKey("PersonnelId")]
		public Personnel? Personnel { get; set; }

    }
}
