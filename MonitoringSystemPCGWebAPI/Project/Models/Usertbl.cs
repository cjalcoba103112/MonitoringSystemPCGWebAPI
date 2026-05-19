
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
        [NotMapped]
        public string? Password {get;set;}
        public int? PersonnelId { get; set; }
        public Personnel? Personnel { get; set; }

        public string? Email { get; set; }
        public bool? IsDefaultPassword { get; set; }
        public string? ChangePasswordToken { get; set; }
        public bool? IsActive { get; set; }
        public int? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role? Role { get; set; }

    }
}
