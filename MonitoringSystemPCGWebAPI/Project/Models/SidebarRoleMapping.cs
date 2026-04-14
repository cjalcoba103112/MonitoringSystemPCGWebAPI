
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class SidebarRoleMapping
    {
		[Key]
		public int? SidebarRoleMappingId {get;set;}
		public int? RoleId {get;set;}
		public int? SidebarId {get;set;}

    }
}
