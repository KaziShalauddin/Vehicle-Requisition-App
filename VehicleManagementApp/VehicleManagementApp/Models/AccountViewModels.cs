using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

   
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class EmployeeRegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string ContactNo { get; set; }
        

        [Required]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string LicenceNo { get; set; }
        public bool IsDriver { get; set; }

        [Display(Name = "Department")]
        [Required]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public IEnumerable<Department> Departments { get; set; }

        [Display(Name = "Designation")]
        [Required]
        public int DesignationId { get; set; }
        public Designation Designation { get; set; }
        public IEnumerable<Designation> Designations { get; set; }

        [Display(Name = "Division")]
        public int DivisionId { get; set; }
        public Division Division { get; set; }
        public IEnumerable<Division> Divisions { get; set; }

        [Display(Name = "District")]
        public int DistrictId { get; set; }
        public District District { get; set; }
        public IEnumerable<District> Districts { get; set; }

        [Display(Name = "Thana/Upzilla")]
        public int ThanaId { get; set; }
        public Thana Thana { get; set; }
        public IEnumerable<Thana> Thanas { get; set; }
    }
}
