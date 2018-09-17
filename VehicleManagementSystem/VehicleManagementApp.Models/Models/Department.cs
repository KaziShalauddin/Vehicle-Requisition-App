using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Models.Contracts;
using VehicleManagementApp.Repository.Contracts;

namespace VehicleManagementApp.Models.Models
{
    public class Department:IModel,IDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Organaization")]
        public int OrganaizationId { get; set; }
        public Organaization Organaization { get; set; }
        public bool IsDeleted { get; set; }
        public bool withDeleted()
        {
            return IsDeleted;
        }
    }
}
