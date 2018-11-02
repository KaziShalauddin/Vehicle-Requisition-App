using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foolproof;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.ViewModels
{
    public class EditRequisitionViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Starting Place")]
        public string Form { get; set; }

        [Required]
        [Display(Name = "Destination")]
        public string To { get; set; }

        [Required]
        public string Description { get; set; }
       
        [Display(Name = "Journey Start")]
        //[DataType(DataType.Date)]
        public DateTime JourneyStart { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        //[DataType(DataType.Date)]
        public DateTime? JourneyStartDate { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public DateTime JourneyStartTime { get; set; }

        [Display(Name = "Journey End")]
        //[DataType(DataType.Date)]
        public DateTime JourneyEnd { get; set; }

        [Required]
        [Display(Name = "End Date")]
        //[DataType(DataType.Date)]
        //[GreaterThan("JourneyStartDate")]
        public DateTime? JouneyEndDate { get; set; }


        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public DateTime JouneyEndTime { get; set; }


        public string RequestType { get; set; }
        public List<SelectListItem> RequestTypes { get; set; }

        [Display(Name = "Request For Other")]
        public bool RequestForOther { get; set; }
        

        [Display(Name = "Employee Name")]
       // [Remote("IsEmployeeIdProvided", "Requsition", HttpMethod = "POST", AdditionalFields = "RequestForOther", ErrorMessage = "Please Select Employee !")]
        [RequiredIfTrue("RequestForOther", ErrorMessage = "Please, Select An Employee!")]
        public int? EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        

    }
}