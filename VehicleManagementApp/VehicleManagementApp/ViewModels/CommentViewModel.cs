using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public int? CommntId { get; set; }
        public Comment Commnt { get; set; }
        public int RequsitionId { get; set; }
        public Requsition Requsition { get; set; }
        public RequsitionViewModel RequsitionViewModel { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string EmployeName { get; set; }
        public IEnumerable<ReplayCommentViewModel> ReplayCommentViewModels { get; set; }
        public ReplayCommentViewModel ReplayCommentViewModel { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CommentTime { get; set; }

        [Display(Name = "To")]
        public int? ReceiverEmployeeId { get; set; }
        public Employee ReceiverEmployee { get; set; }
        public IEnumerable<Employee> ReceiverEmployees { get; set; }
        public bool IsReceiverSeen { get; set; }
        public DateTime ReceiverSeenTime { get; set; }

        public int SenderEmployeeId { get; set; }
        public Employee SenderEmployee { get; set; }

       
        public string ReceiverForControllerComment { get; set; }
       
    }
}