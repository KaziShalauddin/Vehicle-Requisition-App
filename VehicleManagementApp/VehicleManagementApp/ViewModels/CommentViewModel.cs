using System;
using System.Collections.Generic;
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
        public RequsitionViewModel RequsitionViewModel { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string EmployeName { get; set; }
        public IEnumerable<ReplayCommentViewModel> ReplayCommentViewModels { get; set; }
        public ReplayCommentViewModel ReplayCommentViewModel { get; set; }

        public User User { get; set; }

    }
}