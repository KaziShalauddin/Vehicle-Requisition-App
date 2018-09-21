using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class ReplayCommentViewModel
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public int? CommntId { get; set; }
        public Comment ReCommnt { get; set; }
        public int RequsitionId { get; set; }
        public RequsitionViewModel RequsitionViewModel { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string EmployeName { get; set; }
    }
}