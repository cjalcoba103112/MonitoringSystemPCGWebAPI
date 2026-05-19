
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Sidebar
    {
		[Key]
		public int? SidebarId {get;set;}
		public string? SidebarName {get;set;}
        public string? Path { get; set; }
        public string? KeyName { get; set; }
    }
}
