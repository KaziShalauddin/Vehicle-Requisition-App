using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.ViewModels;

namespace VehicleManagementApp.Controllers
{
    public class DriverController : Controller
    {
        // GET: Driver
        private IEmployeeManager _employeeManager;
        private IDepartmentManager _departmentManager;
        private IDesignationManager _designationManager;
        private IDivisionManager _divisionManager;
        private IDistrictManager _districtManager;
        private IThanaManager _thanaManager;

        public DriverController(IEmployeeManager employee, IDepartmentManager department,
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
            var driver = _employeeManager.Get(c=> c.IsDriver ==true && c.IsDeleted == false);
            var division = _divisionManager.GetAll();
            var district = _districtManager.GetAll();
            var thana = _thanaManager.GetAll();

            List<DriverViewModel> driverViewList = new List<DriverViewModel>();
            foreach (var driverdata in driver)
            {
                var driverVm = new DriverViewModel();
                driverVm.Id = driverdata.Id;
                driverVm.Name = driverdata.Name;
                driverVm.ContactNo = driverdata.ContactNo;
                driverVm.Email = driverdata.Email;
                driverVm.Address1 = driverdata.Address1;
                driverVm.Address2 = driverdata.Address2;
                driverVm.LicenceNo = driverdata.LicenceNo;
                driverVm.Department = department.Where(x => x.Id == driverdata.DepartmentId).FirstOrDefault();
                driverVm.Designation = designation.Where(x => x.Id == driverdata.DesignationId).FirstOrDefault();
                driverVm.Division = division.Where(x => x.Id == driverdata.DivisionId).FirstOrDefault();
                driverVm.District = district.Where(x => x.Id == driverdata.DistrictId).FirstOrDefault();
                driverVm.Thana = thana.Where(x => x.Id == driverdata.ThanaId).FirstOrDefault();

                driverViewList.Add(driverVm);
            }
            return View(driverViewList);
        }
        // GET: Driver/Added
        public ActionResult Added()
        {
            var department = _departmentManager.GetAll();
            var designation = _designationManager.GetAll();
            var division = _divisionManager.GetAll();
            var district = _districtManager.GetAll();
            var thana = _thanaManager.GetAll();

            DriverViewModel driverVm = new DriverViewModel
            {
                Departments = department,
                Designations = designation,
                Divisions = division,
                Districts = district,
                Thanas = thana,
                IsDriver = true
            };

            ViewBag.districtDropDown = new SelectListItem[] { new SelectListItem() { Value = "", Text = "Select..." } };
            ViewBag.DistrictId = new SelectListItem[] { new SelectListItem() { Value = "", Text = "Select..." } };
            ViewBag.ThanaId = new SelectListItem[] { new SelectListItem() { Value = "", Text = "Select..." } };

            return View(driverVm);
        }
        [HttpPost]
        public ActionResult Added(DriverViewModel driverVm)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee();
                employee.Name = driverVm.Name;
                employee.ContactNo = driverVm.ContactNo;
                employee.Email = driverVm.Email;
                employee.Address1 = driverVm.Address1;
                employee.Address2 = driverVm.Address2;
                employee.LicenceNo = driverVm.LicenceNo;
                employee.IsDriver = driverVm.IsDriver;
                employee.DepartmentId = driverVm.DepartmentId;
                employee.DesignationId = driverVm.DesignationId;
                employee.DivisionId = driverVm.DivisionId;
                employee.DistrictId = driverVm.DistrictId;
                employee.ThanaId = driverVm.ThanaId;
                
                bool isSaved = _employeeManager.Add(employee);
                if (isSaved)
                {
                    TempData["msg"] = "Driver Save Successfully.";
                    return RedirectToAction("Index","Driver");
                }
                    TempData["msg"] = "Driver Not Saved Successfully.";
                    return RedirectToAction("Added");
            }
            return View();
        }
        // GET: Driver/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id==null)
            {
                return HttpNotFound();
            }
            Employee driver = _employeeManager.GetById((int)id);

            EditDriverViewModel driverVm = new EditDriverViewModel();
            driverVm.Id = driver.Id;
            driverVm.Name = driver.Name;
            driverVm.ContactNo = driver.ContactNo;
            driverVm.Email = driver.Email;
            driverVm.Address1 = driver.Address1;
            driverVm.Address2 = driver.Address2;
            driverVm.LicenceNo = driver.LicenceNo;
            driverVm.IsDriver = driver.IsDriver;
            driverVm.DepartmentId = (int)driver.DepartmentId;
            driverVm.DesignationId = (int)driver.DesignationId;
            driverVm.DivisionId = (int)driver.DivisionId;
            driverVm.DistrictId = (int)driver.DistrictId;
            driverVm.ThanaId = (int)driver.ThanaId;

            ViewBag.DepartmentId = new SelectList(_departmentManager.GetAll(), "Id", "Name", driver.DepartmentId);
            ViewBag.DesignationId = new SelectList(_designationManager.GetAll(), "Id", "Name", driver.DesignationId);
            ViewBag.DivisionId = new SelectList(_divisionManager.GetAll(), "Id", "Name", driver.DivisionId);
            ViewBag.DistrictId = new SelectList(_districtManager.GetAll(), "Id", "Name", driver.DistrictId);
            ViewBag.ThanaId = new SelectList(_thanaManager.GetAll(), "Id", "Name", driver.ThanaId);

            return View(driverVm);
        }
        // POST: Driver/Edit/5
        [HttpPost]
        public ActionResult Edit(EditDriverViewModel driverVm)
        {
            try
            {
                Employee employee = new Employee();
                employee.Id = driverVm.Id;
                employee.Name = driverVm.Name;
                employee.ContactNo = driverVm.ContactNo;
                employee.Email = driverVm.Email;
                employee.Address1 = driverVm.Address1;
                employee.Address2 = driverVm.Address2;
                employee.LicenceNo = driverVm.LicenceNo;
                employee.IsDriver = driverVm.IsDriver;
                employee.DepartmentId = driverVm.DepartmentId;
                employee.DesignationId = driverVm.DesignationId;
                employee.DivisionId = driverVm.DivisionId;
                employee.DistrictId = driverVm.DistrictId;
                employee.ThanaId = driverVm.ThanaId;

                _employeeManager.Update(employee);
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
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var department = _departmentManager.GetAll();
            var designation = _designationManager.GetAll();
            var division = _divisionManager.GetAll();
            var district = _districtManager.GetAll();
            var thana = _thanaManager.GetAll();
            Employee driver = _employeeManager.GetById((int) id);
            DriverViewModel driverVm = new DriverViewModel()
            {
                Id = driver.Id,
                Name = driver.Name,
                ContactNo = driver.ContactNo,
                Email = driver.Email,
                Address1 = driver.Address1,
                Address2 = driver.Address2,
                LicenceNo = driver.LicenceNo,
                Department = department.Where(x => x.Id == driver.DepartmentId).FirstOrDefault(),
                Designation = designation.Where(x => x.Id == driver.DesignationId).FirstOrDefault(),
                Division = division.Where(x => x.Id == driver.DivisionId).FirstOrDefault(),
                District = district.Where(x => x.Id == driver.DistrictId).FirstOrDefault(),
                Thana = thana.Where(x => x.Id == driver.ThanaId).FirstOrDefault()
            };
            return View(driverVm);
        }
        // GET: Driver/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id==null)
            {
                return HttpNotFound();
            }
            Employee driver = _employeeManager.GetById((int)id);
            _employeeManager.Remove(driver);
            return View();
        }
        // POST: Driver/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: ADD delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
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