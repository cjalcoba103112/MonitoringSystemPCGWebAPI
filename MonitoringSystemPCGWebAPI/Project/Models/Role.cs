
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Role
    {
        [Key]
        public int? RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? IndexPath { get; set; }
        public bool? IsSuperAdmin { get; set; }
        public ICollection<SidebarRoleMapping>? SidebarRoleMappings { get; set; }
    }
}
