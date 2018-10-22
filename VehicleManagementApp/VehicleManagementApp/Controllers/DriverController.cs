using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models;
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

        private IRequsitionManager _requisitionManager;
        private IVehicleManager vehicleManager;
        private IDriverStatusManager driverStatusManager;
        private IVehicleStatusManager vehicleStatusManager;

        private ICommentManager commentManager;

        public DriverController(IEmployeeManager employee, IDepartmentManager department,
            IDesignationManager designation,
            IDivisionManager division, IDistrictManager district, IThanaManager thana, IRequsitionManager requisition,
            IVehicleManager vehicle, IDriverStatusManager driverStatus, IVehicleStatusManager vehicleStatus, ICommentManager comment)
        {
            this._employeeManager = employee;
            this._departmentManager = department;
            this._designationManager = designation;
            this._divisionManager = division;
            this._districtManager = district;
            this._thanaManager = thana;
            this._requisitionManager = requisition;

            this.vehicleManager = vehicle;
            this.driverStatusManager = driverStatus;
            this.vehicleStatusManager = vehicleStatus;

            this.commentManager = comment;
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
        private int GetEmployeeId()
        {
            ApplicationUser user =
                System.Web.HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employee = _employeeManager.Get(c => c.IsDeleted == false && c.UserId == user.Id);
            var employeeId = employee.Select(e => e.Id).FirstOrDefault();
            return employeeId;
        }

        //[Authorize(Roles = "Driver")]
        public ActionResult MyDutyList()
        {
            var userEmployeeId = GetEmployeeId();
           
            ViewBag.UserEmployeeId = userEmployeeId;
            List<DriverDutyViewModel> assignedList = new List<DriverDutyViewModel>();
            if (userEmployeeId != 0)
            {
                var vehicle = vehicleManager.GetAll();
                var vehicleStatus = vehicleStatusManager.Get(c => c.Status == "Assign").OrderByDescending(c => c.Id);
                var driverStatus = driverStatusManager.Get(c => c.Status == "Assign").Where(e => e.EmployeeId == userEmployeeId).OrderByDescending(c => c.Id);
                var requsition = _requisitionManager.Get(c => c.Status == "Assign").OrderByDescending(c => c.Id);

                var driverWithRequisition = from r in requsition
                                            join v in vehicleStatus on r.Id equals v.RequsitionId
                                            join driver in driverStatus on r.Id equals driver.RequsitionId
                                            select new
                                            {
                                                r.Id,
                                                r.RequsitionNumber,
                                                r.Form,
                                                r.To,
                                                //Requestor = r.EmployeeId,
                                                r.JourneyStart,
                                                r.JouneyEnd,
                                                v.VehicleId,
                                                //Driver = driver.EmployeeId

                                            };

                foreach (var allData in driverWithRequisition)
                {
                    var assignVM = new DriverDutyViewModel();
                    assignVM.Id = allData.Id;
                    assignVM.RequsitionNumber = allData.RequsitionNumber;
                    assignVM.From = allData.Form;
                    assignVM.To = allData.To;
                    assignVM.JourneyStart = allData.JourneyStart;
                    assignVM.JouneyEnd = allData.JouneyEnd;
                    assignVM.Vehicle = vehicle.Where(c => c.Id == allData.VehicleId).FirstOrDefault();
                    assignedList.Add(assignVM);
                }

            }
            return View(assignedList);
        }
        [Authorize(Roles = "Driver")]
        public ActionResult MyCompletedDuties()
        {
            var employeeId = GetEmployeeId();
            List<DriverDutyViewModel> assignedList = new List<DriverDutyViewModel>();
            if (employeeId != 0)
            {
                var vehicle = vehicleManager.GetAll();
                var vehicleStatus = vehicleStatusManager.Get(c => c.Status == "Complete").OrderByDescending(c => c.Id);
                var driverStatus = driverStatusManager.Get(c => c.Status == "Complete").Where(e => e.EmployeeId == employeeId).OrderByDescending(c => c.Id);
                var requsition = _requisitionManager.Get(c => c.Status == "Complete").OrderByDescending(c => c.Id);

                var driverWithRequisition = from r in requsition
                                            join v in vehicleStatus on r.Id equals v.RequsitionId
                                            join driver in driverStatus on r.Id equals driver.RequsitionId
                                            select new
                                            {
                                                r.Id,
                                                r.RequsitionNumber,
                                                r.Form,
                                                r.To,
                                                //Requestor = r.EmployeeId,
                                                r.JourneyStart,
                                                r.JouneyEnd,
                                                v.VehicleId,
                                                //Driver = driver.EmployeeId

                                            };

                foreach (var allData in driverWithRequisition)
                {
                    var assignVM = new DriverDutyViewModel();
                    assignVM.Id = allData.Id;
                    assignVM.RequsitionNumber = allData.RequsitionNumber;
                    assignVM.From = allData.Form;
                    assignVM.To = allData.To;
                    assignVM.JourneyStart = allData.JourneyStart;
                    assignVM.JouneyEnd = allData.JouneyEnd;
                    assignVM.Vehicle = vehicle.Where(c => c.Id == allData.VehicleId).FirstOrDefault();
                    assignedList.Add(assignVM);
                }

            }
            return View(assignedList);
        }
        [Authorize(Roles = "Driver")]
        public ActionResult AssignDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requsition requisition = _requisitionManager.GetById((int)id);
            var userEmployeeId = GetEmployeeId();
            ViewBag.UserEmployeeId = userEmployeeId;
            var driverId = driverStatusManager.Get(c => c.RequsitionId == id).Select(c => c.EmployeeId).FirstOrDefault();
            if (userEmployeeId != driverId)
            {
                TempData["msg"] = "Sorry, you have no permission to access this type of data!";
                return RedirectToAction("MyDutyList");

            }
            var vehicleId = vehicleStatusManager.Get(c => c.RequsitionId == id).Select(c => c.VehicleId).FirstOrDefault();
            
            AssignedListViewModel assignVm = new AssignedListViewModel
            {
                Requisition = requisition,
                Employee = _employeeManager.GetById((int)requisition.EmployeeId),
                Driver = _employeeManager.GetById(driverId),
                Vehicle = vehicleManager.GetById(vehicleId)
            };
            GetCommentViewModelForInsertComment(requisition, userEmployeeId, assignVm);

            //Collect the list of comment to display the list under comment
            GetCommentList(requisition, assignVm);
            return View(assignVm);
        }

        private void GetCommentViewModelForInsertComment(Requsition requisition, int userEmployeeId,
            AssignedListViewModel assignVm)
        {
            int? emplId = requisition.EmployeeId;
            string employeeNam = requisition.Employee.Name;
            var comment = new CommentViewModel
            {
                EmployeeId = (int) emplId,
                EmployeName = employeeNam,
                RequsitionId = requisition.Id,
                SenderEmployeeId = userEmployeeId,
                ReceiverEmployees = _employeeManager.Get(c => c.UserRole == "Controller")
            };
            assignVm.CommentViewModel = comment;
        }

        private void GetCommentList(Requsition requisition, AssignedListViewModel assignVm)
        {
            List<CommentViewModel> commentListViewModel = new List<CommentViewModel>();
            var commentListView = commentManager.GetCommentsByRequisition(requisition.Id);
            foreach (var item in commentListView.ToList())
            {
                var cmnt = new CommentViewModel
                {
                    Id = item.Id,
                    RequsitionId = item.RequsitionId,
                    EmployeeId = item.EmployeeId,
                    Comments = item.Comments,
                    UserName = item.UserName,
                    CommentTime = item.CommentTime,
                    IsReceiverSeen = item.IsReceiverSeen,
                    ReceiverSeenTime = item.ReceiverSeenTime,
                    SenderEmployee= item.SenderEmployee,
                    SenderEmployeeId= (int)item.SenderEmployeeId,
                    ReceiverEmployee = item.ReceiverEmployee,
                    ReceiverEmployeeId = (int)item.ReceiverEmployeeId,
                };


                commentListViewModel.Add(cmnt);
            }
            assignVm.CommentViewModels = commentListViewModel;
        }
       

        [Authorize(Roles = "Driver")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(CommentViewModel commentViewModel)
        {

            //var userId = User.Identity.GetUserId();
            var userName = User.Identity.Name;


            Comment comment = new Comment
            {
                RequsitionId = commentViewModel.RequsitionId,
                Comments = commentViewModel.Comments,
                EmployeeId = commentViewModel.EmployeeId,
                SenderEmployeeId = commentViewModel.SenderEmployeeId,
               // SenderEmployee = _employeeManager.GetById(commentViewModel.SenderEmployeeId),
                ReceiverEmployeeId = commentViewModel.ReceiverEmployeeId,
                ReceiverSeenTime= DateTime.Now,
                //ReceiverEmployee = _employeeManager.GetById(commentViewModel.SenderEmployeeId),
                UserName = userName,
                CommentTime = DateTime.Now
            };
            bool isSaved = commentManager.Add(comment);

            List<CommentViewModel> commentListViewModel = new List<CommentViewModel>();

            if (isSaved)
            {
                var userEmployeeId = GetEmployeeId();
                ViewBag.UserEmployeeId = userEmployeeId;
                //Collect the list of comment to display the list under comment
                var commentListView = commentManager.GetCommentsByRequisition(commentViewModel.RequsitionId);

                foreach (var item in commentListView.ToList())
                {
                    var cmnt = new CommentViewModel
                    {

                        Id = item.Id,
                        RequsitionId = item.RequsitionId,
                        EmployeeId = item.EmployeeId,
                        Comments = item.Comments,
                        UserName = item.UserName,
                        CommentTime = item.CommentTime,
                        IsReceiverSeen = item.IsReceiverSeen,
                        ReceiverSeenTime = item.ReceiverSeenTime,
                        SenderEmployee = item.SenderEmployee,
                        SenderEmployeeId = (int)item.SenderEmployeeId,
                        ReceiverEmployee = item.ReceiverEmployee,
                        ReceiverEmployeeId = (int)item.ReceiverEmployeeId,
                    };
                    commentListViewModel.Add(cmnt);

                }
                return PartialView("_CommentList", commentListViewModel);
            }
            return PartialView("_CommentList", commentListViewModel);
        }

        public RedirectToRouteResult CommentSeen(int? id)
        {
            var commentSeen = commentManager.GetById((int) id);
            var requisition =_requisitionManager.GetById(commentSeen.RequsitionId);
            commentSeen.ReceiverSeenTime = DateTime.Now;
            commentSeen.IsReceiverSeen = true;
            commentManager.Update(commentSeen);
            return RedirectToAction("AssignDetails", new {id= requisition.Id});
        }
    }
}