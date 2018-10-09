using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
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
        [Display(Name = "User Name")]
        //[EmailAddress]
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
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
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
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
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
    
    public class EmployeeRegisterWithRoleViewModel
    {
       

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        //public string ConfirmPassword { get; set; }

       
       

        [Required]
        //[Remote("SuggestUserName", "Account", HttpMethod = "POST", AdditionalFields = "ContactNo")]
        public string Name { get; set; }

        //[Required]
        //[EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required(ErrorMessage = "You must provide a mobile/phone number")]
        [Display(Name = "Contact No")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(?:\+88|01)?(?:\d{11}|\d{13})$", ErrorMessage = "Not a valid phone number")]
        [Remote("IsNameExist", "Employee", HttpMethod = "POST", ErrorMessage = "Contact No Already Exist, Try Another")]
        public string ContactNo { get; set; }


        [Display(Name = "User Photo")]
        //[Remote("UserAlreadyExists", "Account", HttpMethod = "POST", ErrorMessage = "User Name Already Exist, Try Another")]
        public byte[] UserPhoto { get; set; }


        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }



        [Display(Name = "Designation")]
        public int DesignationId { get; set; }
        public IEnumerable<Designation> Designations { get; set; }

         [Required]
        [Remote("UserAlreadyExists", "Account", HttpMethod = "POST", ErrorMessage = "User Name Already Exist, Try Another")]
        public string UserName { get; set; }
       


        [Display(Name = "Division")]
        public int DivisionId { get; set; }
        public IEnumerable<District> Districts { get; set; }

        [Display(Name = "District")]
        public int DistrictId { get; set; }


        [Display(Name = "Thana/Upzilla")]
        public int ThanaId { get; set; }
        public IEnumerable<Thana> Thanas { get; set; }


        [Required]
        [Display(Name = "Address")]
        public string Address1 { get; set; }


        public string Role { get; set; }
       
        

    }
}
