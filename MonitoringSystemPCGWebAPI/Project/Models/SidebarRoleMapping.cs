
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class SidebarRoleMapping
    {
		[Key]
		public int? SidebarRoleMappingId {get;set;}
		public int? RoleId {get;set;}
		public int? SidebarId {get;set;}
        [ForeignKey("SidebarId")]
        public Sidebar? Sidebar { get; set; }
    }
}
