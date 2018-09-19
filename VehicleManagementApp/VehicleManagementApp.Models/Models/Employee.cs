using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Models.Contracts;
using VehicleManagementApp.Repository.Contracts;

namespace VehicleManagementApp.Models.Models
{
    //[Table("Employee")]
    public class Employee:IModel,IDeletable
    {
        [Key]
        public int Id { get; set; }
       
        public string UserId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string ContactNo { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Address1 { get; set; }
        public string Address2 { get; set; }

        [StringLength(255)]
        public string LicenceNo { get; set; }

        public bool IsDriver { get; set; }
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
        [ForeignKey("Designation")]
        public int? DesignationId { get; set; }
        public Designation Designation { get; set; }
        [ForeignKey("Division")]
        public int? DivisionId { get; set; }
        public Division Division { get; set; }
        [ForeignKey("District")]
        public int? DistrictId { get; set; }
        public District District { get; set; }
        [ForeignKey("Thana")]
        public int? ThanaId { get; set; }
        public Thana Thana { get; set; }


        public bool IsDeleted { get; set; }
        public bool withDeleted()
        {
            return IsDeleted;
        }
    }
}
