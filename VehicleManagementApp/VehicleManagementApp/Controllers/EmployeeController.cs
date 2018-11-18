using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.ViewModels;

namespace VehicleManagementApp.Controllers
{
    [Authorize(Roles = "Controller,Operator")]
    public class EmployeeController : Controller
    {
        // GET: Employee
        private IEmployeeManager _employeeManager;
        private IDepartmentManager _departmentManager;
        private IDesignationManager _designationManager;
        private IDivisionManager _divisionManager;
        private IDistrictManager _districtManager;
        private IThanaManager _thanaManager;

        public EmployeeController(IEmployeeManager employee, IDepartmentManager department,
            IDesignationManager designation,
            IDivisionManager division, IDistrictManager district, IThanaManager thana)
        {
            this._employeeManager = employee;
            this._departmentManager = department;
            this._designationManager = designation;
            this._divisionManager = division;
            this._districtManager = district;
            this._thanaManager = thana;
        }

        public ActionResult Index()
        {
            var department = _departmentManager.GetAll();
            var designation = _designationManager.GetAll();
            var employee = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false);
            var division = _divisionManager.GetAll();
            var district = _districtManager.GetAll();
            var thana = _thanaManager.GetAll();

            List<EmployeeViewModel> employeeViewList = new List<EmployeeViewModel>();
            foreach (var emploeedata in employee)
            {
                var employeeVM = new EmployeeViewModel();
                employeeVM.Id = emploeedata.Id;
                employeeVM.Name = emploeedata.Name;
                employeeVM.ContactNo = emploeedata.ContactNo;
                employeeVM.Email = emploeedata.Email;
                employeeVM.Address1 = emploeedata.Address1;
                employeeVM.Address2 = emploeedata.Address2;
                //employeeVM.LicenceNo = emploeedata.LicenceNo;
                employeeVM.Department = department.Where(x => x.Id == emploeedata.DepartmentId).FirstOrDefault();
                employeeVM.Designation = designation.Where(x => x.Id == emploeedata.DesignationId).FirstOrDefault();
                employeeVM.Division = division.Where(x => x.Id == emploeedata.DivisionId).FirstOrDefault();
                employeeVM.District = district.Where(x => x.Id == emploeedata.DistrictId).FirstOrDefault();
                employeeVM.Thana = thana.Where(x => x.Id == emploeedata.ThanaId).FirstOrDefault();

                employeeViewList.Add(employeeVM);
            }
            return View(employeeViewList);
        }

        // GET: Employee/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: Employee/Create
        public ActionResult Create()
        {
            var department = _departmentManager.GetAll();
            var designation = _designationManager.GetAll();
            var division = _divisionManager.GetAll();
            var district = _districtManager.GetAll();
            var thana = _thanaManager.GetAll();

            EmployeeViewModel employeeVM = new EmployeeViewModel
            {
                Departments = department,
                Designations = designation,
                Divisions = division,
                Districts = district,
                Thanas = thana
            };


            ViewBag.districtDropDown = new SelectListItem[] { new SelectListItem() { Value = "", Text = "Select..." } };
            ViewBag.DistrictId = new SelectListItem[] { new SelectListItem() { Value = "", Text = "Select..." } };
            ViewBag.ThanaId = new SelectListItem[] { new SelectListItem() { Value = "", Text = "Select..." } };

            //ViewBag.districtDropDown = new SelectLsistItem[] { new SelectListItem() { Value = "", Text = "Select..." } };

            return View(employeeVM);
        }
        
        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee
                {
                    //UserId = user.Id,
                    Name = employeeVM.Name,
                    ContactNo = employeeVM.ContactNo,
                    Email = employeeVM.Email,
                    Address1 = employeeVM.Address1,
                    Address2 = employeeVM.Address2,
                    Status = "employee",
                    //IsDriver = employeeVM.IsDriver,
                    DepartmentId = employeeVM.DepartmentId,
                    DesignationId = employeeVM.DesignationId,
                    DivisionId = employeeVM.DivisionId,
                    DistrictId = employeeVM.DivisionId,
                    ThanaId = employeeVM.ThanaId
                };

                bool isSaved = _employeeManager.Add(employee);
                if (isSaved)
                {
                    TempData["msg"] = "Employee Save Successfully!";
                    return RedirectToAction("Index");
                }

                TempData["msg"] = "Employee Not Saved!";
                return RedirectToAction("Create");
            }
            return View();
        }


        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Employee employee = _employeeManager.GetById((int)id);

            var employeeVM = new EditEmployeeViewModel
            {
                Id = employee.Id,
                //UserId = employee.UserId,
                Name = employee.Name,
                ContactNo = employee.ContactNo,
                Email = employee.Email,
                Address1 = employee.Address1,
                //Address2 = employee.Address2,
                //Status = "employee",
                DepartmentId = (int) employee.DepartmentId,
                DesignationId = (int) employee.DesignationId,
                DivisionId = (int) employee.DivisionId,
                DistrictId = (int) employee.DistrictId,
                ThanaId = (int) employee.ThanaId,
                
                Image = employee.Image,
                ImagePath = employee.ImagePath
                //UserRole = employee.UserRole
            };
            //employeeVM.IsDriver = employee.IsDriver;

            ViewBag.DepartmentId = new SelectList(_departmentManager.GetAll(), "Id", "Name", employee.DepartmentId);
            ViewBag.DesignationId = new SelectList(_designationManager.GetAll(), "Id", "Name", employee.DesignationId);
            ViewBag.DivisionId = new SelectList(_divisionManager.GetAll(), "Id", "Name", employee.DivisionId);
            ViewBag.DistrictId = new SelectList(_districtManager.GetAll(), "Id", "Name", employee.DistrictId);
            ViewBag.ThanaId = new SelectList(_thanaManager.GetAll(), "Id", "Name", employee.ThanaId);

            return View(employeeVM);
        }
        public static string GetFileNameToSave(string s)
        {
            return s
                .Replace("\\", "")
                .Replace("/", "")
                .Replace("\"", "")
                .Replace("*", "")
                .Replace(":", "")
                .Replace("?", "")
                .Replace("<", "")
                .Replace(">", "")
                .Replace("|", "");
        }

        private string _imagePath;
        private byte[] GetImageData(string imgName)
        {
            byte[] imageData = null;

            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase imgFile = Request.Files["Image"];
                if (imgFile != null && imgFile.ContentLength > 0)
                {

                    var fileName = Regex.Replace(imgName, @"\s+", "");
                    
                 
                    _imagePath = "~/EmployeeImages/" + fileName +
                                DateTime.Now.ToString("ddMMyyhhmmsstt") + Path.GetExtension(imgFile.FileName);
                    imgFile.SaveAs(Server.MapPath(_imagePath));
                   // imgFile.SaveAs(Server.MapPath(Path.Combine()));

                    using (var binary = new BinaryReader(imgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(imgFile.ContentLength);
                    }
                }
            }
            return imageData;
        }
        public bool HasFile(byte[] file)
        {

            return file != null;
        }

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Image")]EditEmployeeViewModel employeeVm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var employee = _employeeManager.GetById((int)employeeVm.Id);

                    //employee.Id = employeeVm.Id;
                    employee.Name = employeeVm.Name;
                    employee.ContactNo = employeeVm.ContactNo;
                    employee.Email = employeeVm.Email;
                    employee.Address1 = employeeVm.Address1;
                    employee.DepartmentId = employeeVm.DepartmentId;
                    employee.DesignationId = employeeVm.DesignationId;
                    employee.DivisionId = employeeVm.DivisionId;
                    employee.DistrictId = employeeVm.DistrictId;
                    employee.ThanaId = employeeVm.ThanaId;
                    var imageData = GetImageData(employeeVm.Name);
                    if (imageData != null)
                    {

                        var _user =UserManager.FindById(employee.UserId);
                       
                        if (HasFile(imageData))
                        {
                            if (_user != null)
                            {
                                _user.UserPhoto = imageData;
                                UserManager.Update(_user);
                            }

                            employee.Image = imageData;
                            employee.ImagePath = _imagePath;
                        }
                    }
                    
                   
                    bool isUpdated = _employeeManager.Update(employee);
                    if (isUpdated)
                    {
                        TempData["msg"] = "Employee Update Successful!";
                        return RedirectToAction("Index");
                    }

                }
                #region 
                //var employee = new Employee
                //{
                //    Id = employeeVM.Id,
                //    UserId = employeeVM.UserId,
                //    Name = employeeVM.Name,
                //    ContactNo = employeeVM.ContactNo,
                //    Email = employeeVM.Email,
                //    Address1 = employeeVM.Address1,
                //    //Address2 = employeeVM.Address2,
                //    //Status = "employee",
                //    DepartmentId = employeeVM.DepartmentId,
                //    DesignationId = employeeVM.DesignationId,
                //    DivisionId = employeeVM.DivisionId,
                //    DistrictId = employeeVM.DistrictId,
                //    ThanaId = employeeVM.ThanaId,

                //    Image = employeeVM.Image,
                //    ImagePath = employeeVM.ImagePath,
                //    UserRole = employeeVM.UserRole
                //};

                // _employeeManager.Update(employee);
#endregion
                TempData["msg"] = "Employee Update Failed!";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Employee employee = _employeeManager.GetById((int)id);
            _employeeManager.Remove(employee);
            return View();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = _departmentManager.GetAll();
            var designation = _designationManager.GetAll();
            var division = _divisionManager.GetAll();
            var district = _districtManager.GetAll();
            var thana = _thanaManager.GetAll();
            Employee employee = _employeeManager.GetById((int)id);
            EmployeeViewModel employeeVM = new EmployeeViewModel()
            {
                Id = employee.Id,
                Name = employee.Name,
                ContactNo = employee.ContactNo,
                Email = employee.Email,
                Address1 = employee.Address1,
                Address2 = employee.Address2,
                //LicenceNo = employee.LicenceNo,
                Department = department.Where(x => x.Id == employee.DepartmentId).FirstOrDefault(),
                Designation = designation.Where(x => x.Id == employee.DesignationId).FirstOrDefault(),
                Division = division.Where(x => x.Id == employee.DivisionId).FirstOrDefault(),
                District = district.Where(x => x.Id == employee.DistrictId).FirstOrDefault(),
                Thana = thana.Where(x => x.Id == employee.ThanaId).FirstOrDefault(),
                ImagePath = employee.ImagePath

            };
            return View(employeeVM);
        }
        //public ActionResult Driver()
        //{
        //    var employee = _employeeManager.Get(c => c.IsDriver == true && c.IsDeleted == false);
        //    var department = _departmentManager.GetAll();
        //    var designation = _designationManager.GetAll();
        //    var division = _divisionManager.GetAll();
        //    var district = _districtManager.GetAll();
        //    var thana = _thanaManager.GetAll();

        //    List<EmployeeViewModel> employeeViewList = new List<EmployeeViewModel>();
        //    foreach (var emploeedata in employee)
        //    {
        //        var employeeVM = new EmployeeViewModel();
        //        employeeVM.Id = emploeedata.Id;
        //        employeeVM.Name = emploeedata.Name;
        //        employeeVM.ContactNo = emploeedata.ContactNo;
        //        employeeVM.Email = emploeedata.Email;
        //        employeeVM.Address1 = emploeedata.Address1;
        //        employeeVM.Address2 = emploeedata.Address2;
        //        employeeVM.LicenceNo = emploeedata.LicenceNo;
        //        employeeVM.Department = department.Where(x => x.Id == emploeedata.DepartmentId).FirstOrDefault();
        //        employeeVM.Designation = designation.Where(x => x.Id == emploeedata.DesignationId).FirstOrDefault();
        //        employeeVM.Division = division.Where(x => x.Id == emploeedata.DivisionId).FirstOrDefault();
        //        employeeVM.District = district.Where(x => x.Id == emploeedata.DistrictId).FirstOrDefault();
        //        employeeVM.Thana = thana.Where(x => x.Id == emploeedata.ThanaId).FirstOrDefault();

        //        employeeViewList.Add(employeeVM);
        //    }
        //    return View(employeeViewList);
        //}

        public JsonResult GetEmployePhoneNo(int? employeeId)
        {
            if (employeeId == null)
            {
                return null;
            }

            var employee = _employeeManager.GetAll();
            var employeeNumber = employee.Where(c => c.Id == employeeId).ToList();
            return Json(employeeNumber, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DriverList()
        {
            var driverList = _employeeManager.Get(c => c.IsDriver == true && c.IsDeleted == false);
            var department = _departmentManager.GetAll();
            var designation = _designationManager.GetAll();

            List<DriverViewModel> AllDriverList = new List<DriverViewModel>();
            foreach (var emploeedata in driverList)
            {
                var driverVm = new DriverViewModel();
                driverVm.Id = emploeedata.Id;
                driverVm.Name = emploeedata.Name;
                driverVm.ContactNo = emploeedata.ContactNo;
                driverVm.Email = emploeedata.Email;
                driverVm.Address1 = emploeedata.Address1;
                driverVm.Address2 = emploeedata.Address2;
                driverVm.LicenceNo = emploeedata.LicenceNo;
                driverVm.IsDriver = emploeedata.IsDriver;
                driverVm.Department = department.Where(x => x.Id == emploeedata.DepartmentId).FirstOrDefault();
                driverVm.Designation = designation.Where(x => x.Id == emploeedata.DesignationId).FirstOrDefault();

                AllDriverList.Add(driverVm);
            }
            ViewBag.TotalDriver = driverList.Count;
            return View(AllDriverList);
        }

        public JsonResult IsNameExist(string ContactNo)
        {
            var contact = _employeeManager.IsContactAlreadyExist(ContactNo);
            return Json(contact, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsEmailExist(string Email)
        {
            var email = _employeeManager.IsEmailAlreadyExist(Email);
            return Json(email, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsLicenceExist(string LicenceNo)
        {
            var licence = _employeeManager.IsLicenceAlreadyExist(LicenceNo);
            return Json(licence, JsonRequestBehavior.AllowGet);
        }
    }
}
