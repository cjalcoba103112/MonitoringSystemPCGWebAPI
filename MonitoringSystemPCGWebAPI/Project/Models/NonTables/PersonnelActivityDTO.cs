using Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MonitoringSystemPCGWebAPI.Project.Models.NonTables
{
    public class PersonnelActivityDTO :PersonnelActivity
    {
        public Personnel? Personnel { get; set; }
    }
}
