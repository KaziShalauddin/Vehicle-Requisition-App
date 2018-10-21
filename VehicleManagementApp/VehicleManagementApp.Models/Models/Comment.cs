using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using VehicleManagementApp.Models.Contracts;
using VehicleManagementApp.Repository.Contracts;

namespace VehicleManagementApp.Models.Models
{
    //[Table("Comments")]
    public class Comment : IModel, IDeletable
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Comments { get; set; }
        public string UserName { get; set; }
        public DateTime CommentTime { get; set; }
        public int? CommntId { get; set; }
        public Comment Commnt { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }

        [ForeignKey("Requsition")]
        public int RequsitionId { get; set; }
        public Requsition Requsition { get; set; }
        public bool IsDeleted { get; set; }

        public bool withDeleted()
        {
            return IsDeleted;
        }

        public int? SenderEmployeeId { get; set; }
        public virtual Employee SenderEmployee { get; set; }

        public int? ReceiverEmployeeId { get; set; }
        public virtual Employee ReceiverEmployee { get; set; }
        public bool IsReceiverSeen { get; set; }

        public DateTime ReceiverSeenTime { get; set; }
    }



}
