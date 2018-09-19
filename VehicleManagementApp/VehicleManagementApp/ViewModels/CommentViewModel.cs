using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public int? CommntId { get; set; }
        public Comment Commnt { get; set; }
        public IEnumerable<Comment> Commentss { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public IEnumerable<Employee> Employees { get; set; }

        public int RequsitionViewModelId { get; set; }
        public RequisitionViewModel RequisitionViewModel { get; set; }
      
       
    }
}