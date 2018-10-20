using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Reporting.WebForms;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Models.ReportViewModel;
using VehicleManagementApp.Repository.Contracts;
using VehicleManagementApp.ViewModels;
using VehicleManagementApp.com.onnorokomsms.api2;
using VehicleManagementApp.Models;
using Requsition = VehicleManagementApp.Models.Models.Requsition;

namespace VehicleManagementApp.Controllers
{
    [Authorize(Roles = "Controller")]
    public class ManagerController : Controller
    {
        private IManagerManager managerManager;
        private IRequsitionManager _requisitionManager;
        private IEmployeeManager _employeeManager;
        private IVehicleManager vehicleManager;
        private IDriverStatusManager driverStatusManager;
        private IVehicleStatusManager vehicleStatusManager;
        private IVehicleTypeManager vehicleTypeManager;

        public ManagerController(IRequsitionManager requisition, IEmployeeManager employee, IManagerManager manager,
            IVehicleManager vehicle, IVehicleTypeManager vehicleType, IDriverStatusManager driverStatus, IVehicleStatusManager vehicleStatus)
        {
            this._employeeManager = employee;
            this._requisitionManager = requisition;
            this.managerManager = manager;
            this.vehicleManager = vehicle;
            this.driverStatusManager = driverStatus;
            this.vehicleStatusManager = vehicleStatus;
            this.vehicleTypeManager = vehicleType;
        }

        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult New()
        {
            Requsition requsitions = new Requsition();
            var employee = _employeeManager.GetAll();
            var requsition = _requisitionManager.GetAllByNull(requsitions.Status = null).OrderByDescending(c => c.Id);

            List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            foreach (var data in requsition)
            {
                var requsitionVM = new RequsitionViewModel()
                {
                    Id = data.Id,
                    Form = data.Form,
                    To = data.To,
                    RequsitionNumber = data.RequsitionNumber,
                    Description = data.Description,
                    JourneyStart = data.JourneyStart,
                    JouneyEnd = data.JouneyEnd,
                    Employee = employee.Where(x => x.Id == data.EmployeeId).FirstOrDefault()
                };
                requsitionViewModels.Add(requsitionVM);
            }
            return View(requsitionViewModels);
        }//details
        public ActionResult View(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employee = _employeeManager.GetAll();
            var requsition = _requisitionManager.GetById((int)id);

            RequsitionViewModel requisitionVm = new RequsitionViewModel()
            {
                Id = requsition.Id,
                Form = requsition.Form,
                To = requsition.To,
                RequsitionNumber = requsition.RequsitionNumber,
                Description = requsition.Description,
                JourneyStart = requsition.JourneyStart,
                JouneyEnd = requsition.JouneyEnd,
                Employee = employee.Where(x => x.Id == requsition.EmployeeId).FirstOrDefault()
            };
            //requsition.Status = "Seen";
            //_requisitionManager.Update(requsition);
            return View(requisitionVm);
        }
        public ActionResult RequsitionAssign(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employee = _employeeManager.GetAll();
            var requsition = _requisitionManager.GetById((int)id);

            RequsitionViewModel requisitionVm = new RequsitionViewModel()
            {
                Id = requsition.Id,
                Form = requsition.Form,
                To = requsition.To,
                Description = requsition.Description,
                JourneyStart = requsition.JourneyStart,
                JouneyEnd = requsition.JouneyEnd,
                Employee = employee.Where(x => x.Id == requsition.EmployeeId).FirstOrDefault()
            };

            requsition.Status = "Assign";
            bool assign = _requisitionManager.Update(requsition);
            if (assign)
            {
                return RedirectToAction("Assign");
            }
            //return View(requisitionVm);
            return null;
        }

        [HttpGet]
        public ActionResult Assign(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requsition requsition = _requisitionManager.GetById((int)id);
            Manager manager = new Manager();
            var employees = _employeeManager.Get(c => c.IsDriver == true && c.Status == null || c.Status == "NULL" || c.Status == "Assigned" && c.IsDeleted == false);
            var assignVehicle = vehicleManager.Get(c => c.Status == null || c.Status == "NULL" || c.Status == "Assigned" && c.IsDeleted == false);

            ManagerViewModel managerVM = new ManagerViewModel();
            managerVM.Id = manager.Id;
            managerVM.RequsitionId = requsition.Id;
            managerVM.Requsition = requsition;
            managerVM.Employees = employees;
            managerVM.Vehicles = assignVehicle;

            if (assignVehicle != null)
            {
                var vehicleDropDownList = SetVehicleDropDown(assignVehicle);

                ViewBag.Vehicles = new SelectList(vehicleDropDownList, "Id", "VehicleDetails", manager.EmployeeId);
            }

            ViewBag.EmployeeId = new SelectList(employees, "Id", "Name", manager.EmployeeId);


            return View(managerVM);
        }
        [HttpPost]
        public ActionResult Assign(ManagerViewModel managerViewModel)
        {
            Vehicle vehicle = new Vehicle();
            Requsition requsition = new Requsition();
            Manager manager = new Manager();
            var requsitions = _requisitionManager.GetById(managerViewModel.RequsitionId);
            var startdateTime = requsitions.JourneyStart;
            var endDateTime = requsitions.JouneyEnd;

            manager.Id = managerViewModel.Id;
            manager.DriverNo = managerViewModel.DriverNo;
            manager.RequsitionId = managerViewModel.RequsitionId;
            manager.EmployeeId = managerViewModel.EmployeeId;
            manager.VehicleId = managerViewModel.VehicleId;
            

            manager.StartDate = startdateTime;
            manager.EndDate = endDateTime;

            //Email Sending Methon start
            SendingEmailDriver(managerViewModel.EmployeeId, managerViewModel.RequsitionId);
            SendingEmailEmployee(managerViewModel.EmployeeId, managerViewModel.RequsitionId);
            //Email Sending Methon end


            //Mobile SMS Send start

            SendSMSToMobile(managerViewModel.EmployeeId, managerViewModel.VehicleId);
            //var driverMobileNo = _employeeManager.Get(c=>c.IsDriver==managerViewModel.EmployeeId);
            //var sms = new SendSms();
            //string returnValue = sms.NumberSms("0689b8c0-e", "Vehicle Name: "+ managerViewModel.VehicleId+" Driver Name: "+ managerViewModel.EmployeeId, managerViewModel.DriverNo, "text", "", "BCC");
            //Mobile SMS Send End


            bool isSaved = managerManager.Add(manager);
            RequsitionAssign(managerViewModel.Id);
            VehicleStatusChange(managerViewModel.VehicleId);
            DriverAssigned(managerViewModel.EmployeeId);
            AddDataToDriverStatusTable(managerViewModel.EmployeeId, managerViewModel.Id);
            if (isSaved)
            {
                TempData["msg"] = "Requisition Assign Successfully";
                return RedirectToAction("New");
            }

            return View();
        }

        private bool AddDataToDriverStatusTable(int? employeeId, int? id)
        {
            if (employeeId == null || id == null)
            {
                return false;
            }
            var requsition = _requisitionManager.GetById((int)id);
            DriverStatus dv = new DriverStatus
            {
                RequsitionId = id,
                StartTime = requsition.JourneyStart,
                EndTime = requsition.JouneyEnd,
                EmployeeId = (int)employeeId,
                Status = "Assign"
            };
            bool isSaved = driverStatusManager.Add(dv);
            if (isSaved)
            {
                return true;
            }
            return false;
        }
        private bool AddDataToVehicleStatusTable(int? vehicleId, int? id)
        {
            if (vehicleId == null || id == null)
            {
                return false;
            }
            var requsition = _requisitionManager.GetById((int)id);
            VehicleStatus vs = new VehicleStatus
            {
                RequsitionId = id,
                StartTime = requsition.JourneyStart,
                EndTime = requsition.JouneyEnd,
                VehicleId = (int)vehicleId,
                Status = "Assign"
            };
            bool isSaved = vehicleStatusManager.Add(vs);
            if (isSaved)
            {
                return true;
            }
            return false;
        }
        public bool RequisitionAssign(int? id)
        {
            if (id == null)
            {
                return false;
            }

            var requisition = _requisitionManager.GetById((int)id);

            requisition.Status = "Assign";
            bool assign = _requisitionManager.Update(requisition);
            if (assign)
            {
                return true;
            }

            return false;
        }

        public ActionResult AssignedList()
        {

            var employee = _employeeManager.GetAll();
            var vehicle = vehicleManager.GetAll();
            var vehicleStatus = vehicleStatusManager.Get(c => c.Status == "Assign").OrderByDescending(c => c.Id);
            var driverStatus = driverStatusManager.Get(c => c.Status == "Assign").OrderByDescending(c => c.Id);

            var requsition = _requisitionManager.Get(c => c.Status == "Assign").OrderByDescending(c => c.Id);

            var vehicleStatusWithRequisition = from r in requsition
                                               join v in vehicleStatus on r.Id equals v.RequsitionId
                                               join driver in driverStatus on r.Id equals driver.RequsitionId
                                               select new
                                               {
                                                   r.Id,
                                                   r.RequsitionNumber,
                                                   Requestor = r.EmployeeId,
                                                   r.JourneyStart,
                                                   r.JouneyEnd,
                                                   v.VehicleId,
                                                   Driver = driver.EmployeeId

                                               };
            List<AssignedListViewModel> assignedList = new List<AssignedListViewModel>();
            foreach (var allData in vehicleStatusWithRequisition)
            {
                var assignVM = new AssignedListViewModel();
                assignVM.Id = allData.Id;
                assignVM.Employee = employee.Where(c => c.Id == allData.Requestor).FirstOrDefault();
                assignVM.Vehicle = vehicle.Where(c => c.Id == allData.VehicleId).FirstOrDefault();
                assignVM.Driver = employee.Where(c => c.Id == allData.Driver).FirstOrDefault();
                assignVM.Requisition = requsition.Where(c => c.Id == allData.Id).FirstOrDefault();
                assignedList.Add(assignVM);
            }

            return View(assignedList);

        }
        public ActionResult AssignDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requsition requisition = _requisitionManager.GetById((int)id);
            var driverId = driverStatusManager.Get(c => c.RequsitionId == id).Select(c => c.EmployeeId).FirstOrDefault();
            var vehicleId = vehicleStatusManager.Get(c => c.RequsitionId == id).Select(c => c.VehicleId).FirstOrDefault();

            AssignedListViewModel assignVm = new AssignedListViewModel
            {
                Requisition = requisition,
                Employee = _employeeManager.GetById((int)requisition.EmployeeId),
                Driver = _employeeManager.GetById(driverId),
                Vehicle = vehicleManager.GetById(vehicleId)
            };
            return View(assignVm);
        }
        [HttpGet]
        public ActionResult AdvanceAssign(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requsition requsition = _requisitionManager.GetById((int)id);

            var availableDriverList = driverStatusManager.Get(c => c.EndTime < requsition.JourneyStart).GroupBy(x => x.EmployeeId).Select(x => x.First());

            List<Employee> availableDrivers = new List<Employee>();

            foreach (var driver in availableDriverList)
            {
                var driverItem = _employeeManager.GetById(driver.EmployeeId);
                availableDrivers.Add(driverItem);
            }

            List<Vehicle> availableVehicles = new List<Vehicle>();

            var availableVehicleList = vehicleStatusManager.Get(c => c.EndTime < requsition.JourneyStart).GroupBy(x => x.VehicleId).Select(x => x.First());
            foreach (var vehicle in availableVehicleList)
            {
                var vehicleItem = vehicleManager.GetById(vehicle.VehicleId);
                availableVehicles.Add(vehicleItem);
            }

            AssignViewModel assignVm = new AssignViewModel
            {
                RequsitionId = requsition.Id,
                Requsition = requsition,
                Employees = availableDrivers,
                Vehicles = availableVehicles
            };


            //if (availableVehicles != null)
            //{
            //    var vehicleDropDownList = SetVehicleDropDown(availableVehicles);

            //    ViewBag.Vehicles = new SelectList(vehicleDropDownList, "Id", "VehicleDetails");
            //}

            ViewBag.Vehicles = new SelectList(availableVehicles, "Id", "Name");
            return View(assignVm);
        }

        [HttpPost]
        public ActionResult AdvanceAssign(AssignViewModel assignVm)
        {

            bool isRequisitionAssigned = RequisitionAssign(assignVm.Id);
            //VehicleStatusChange(assignVm.VehicleId);
            bool isDriverAssigned = AddDataToDriverStatusTable(assignVm.EmployeeId, assignVm.Id);
            bool isVehicleAssigned = AddDataToVehicleStatusTable(assignVm.VehicleId, assignVm.Id);
            if (isRequisitionAssigned && isDriverAssigned && isVehicleAssigned)
            {
                TempData["msg"] = "Requisition Assigned Successfully!";

                //Email Sending Method start
                //SendingEmailDriver(assignVm.EmployeeId, assignVm.RequsitionId);
                //SendingEmailEmployee(assignVm.EmployeeId, assignVm.RequsitionId);
                //Email Sending Method end

                return RedirectToAction("New");
            }

            //if (!isRequisitionAssigned && !isDriverAssigned)
            //{
            //    TempData["msg"] = "Requisition Not Assigned";
            //    return View(assignVm);
            //}

            TempData["msg"] = "Requisition Not Assigned";
            return View(assignVm);
        }
        private static List<VehicleDropDownViewModel> SetVehicleDropDown(ICollection<Vehicle> assignVehicle)
        {
            List<VehicleDropDownViewModel> vehicleDropDownList = new List<VehicleDropDownViewModel>();

            foreach (var item in assignVehicle)
            {
                VehicleDropDownViewModel vehicleDrop = new VehicleDropDownViewModel
                {
                    Id = item.Id,
                    VehicleDetails =
                        item.VehicleName + "->" + item.VModel + "->" +
                        item.VRegistrationNo
                };
                vehicleDropDownList.Add(vehicleDrop);
            }

            return vehicleDropDownList;
        }

        [HttpGet]
        public ActionResult ReAssign(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manager managerById = managerManager.GetById((int)id);
            //Requsition requsition = _requisitionManager.GetById((int)id);
            Manager manager = new Manager();
            var employees = _employeeManager.Get(c => c.IsDriver == true && c.Status == "Available" && c.IsDeleted == false);
            var assignVehicle = vehicleManager.Get(c => c.Status == "NULL");

            ManagerViewModel managerVM = new ManagerViewModel();
            managerVM.Id = manager.Id;
            managerVM.RequsitionId = managerById.RequsitionId;

            managerVM.Employees = employees;
            managerVM.Vehicles = assignVehicle;


            //ViewBag.EmployeeId = new SelectList(employees,"Id","Name", managerById.EmployeeId);
            //ViewBag.VehicleId = new SelectList(assignVehicle,"Id","VehicleName", managerById.VehicleId);


            return View(managerVM);
        }

        [HttpPost]
        public ActionResult ReAssign(ManagerViewModel managerViewModel)
        {
            Requsition requsition = new Requsition();
            Manager manager = new Manager();
            manager.Id = managerViewModel.Id;
            manager.DriverNo = managerViewModel.DriverNo;
            manager.RequsitionId = managerViewModel.RequsitionId;
            manager.EmployeeId = managerViewModel.EmployeeId;
            manager.VehicleId = managerViewModel.VehicleId;

            //Email Sending Methon start
            //SendingEmailDriver(managerViewModel.EmployeeId, managerViewModel.RequsitionId);
            //SendingEmailEmployee(managerViewModel.EmployeeId, managerViewModel.RequsitionId);
            //Email Sending Methon end

            bool isSaved = managerManager.Add(manager);
            RequsitionAssign(managerViewModel.Id);
            VehicleStatusChange(managerViewModel.VehicleId);
            DriverAssigned(managerViewModel.EmployeeId);

            if (isSaved)
            {
                TempData["msg"] = "Requisition Assign Successfully";
                return RedirectToAction("New");
            }

            return View();
        }

        //Mobile SMS Send Method
        public void SendSMSToMobile(int? employeeId, int? vehicleId)
        {
            if (employeeId == null && vehicleId == null)
            {
                return;
            }
            var driverMobileNo = _employeeManager.GetById((int)employeeId);
            var vehicleName = vehicleManager.GetById((int)vehicleId);
            if (driverMobileNo.ContactNo == null)
            {
                return;
            }

            var sms = new SendSms();
            string returnValue = sms.NumberSms("0689b8c0-e", "Vehicle Name: " + vehicleName.VehicleName + "." + " Driver Name: " + driverMobileNo.Name, driverMobileNo.ContactNo, "text", "", "BCC");

        }

        public void SendingEmailDriver(int? EmployeeId, int? requsitionId)
        {
            if (EmployeeId == null && requsitionId == null)
            {
                return;
            }
            var requsition = _requisitionManager.GetById((int)requsitionId);
            var employee = _employeeManager.GetById((int)EmployeeId);
            if (employee.Email == null)
            {
                return;
            }
            var dod = "<span><strong>Employee Name</strong> :" + " " + requsition.Employee.Name + "</span>" + "<br/>"
                    + "<span> <strong>Employee Number</strong> :" + " " + requsition.Employee.ContactNo + "</span>" + "<br/>"
                    + "<span> <strong>Department Name</strong> :" + " " + requsition.Employee.Department.Name + "</span>" + "<br/>"
                    + "<span> <strong>Designation Name</strong> :" + " " + requsition.Employee.Designation.Name + "</span>" + "<br/>";

            var body = dod;
            var message = new MailMessage();
            message.To.Add(new MailAddress(employee.Email));  // replace with valid value 
            message.From = new MailAddress("mohammadziaulm62@gmail.com");  // replace with valid value
            message.Subject = "For Your Have A New Car Assign";
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "mohammadziaulm62@gmail.com", // replace with valid value
                    Password = "01915982924" // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
                return;
            }
        }

        public void SendingEmailEmployee(int? EmployeeId, int? RequsitionId)
        {
            if (EmployeeId == null && RequsitionId == null)
            {
                return;
            }
            var requsition = _requisitionManager.GetById((int)RequsitionId);
            var employee = _employeeManager.GetById((int)EmployeeId);
            if (employee.Email == null)
            {
                return;
            }
            var dod = "<span><strong>Driver Name</strong> :" + " " + employee.Name + "</span>" + "<br/>"
                      + "<span> <strong>Phone Number</strong> :" + " " + employee.ContactNo + "</span>" + "<br/>";


            var body = dod;
            var message = new MailMessage();
            message.To.Add(new MailAddress(requsition.Employee.Email));  // replace with valid value 
            message.From = new MailAddress("mohammadziaulm62@gmail.com");  // replace with valid value
            message.Subject = "Your Requsition Assign Successfully";
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "mohammadziaulm62@gmail.com", // replace with valid value
                    Password = "01915982924" // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
                return;
            }
        }


        private void DriverAssigned(int? employeeId)
        {
            if (employeeId == null)
            {
                return;
            }
            var driver = _employeeManager.GetById((int)employeeId);
            EmployeeViewModel employeeViewModel = new EmployeeViewModel()
            {
                Id = driver.Id,
                Name = driver.Name,
                ContactNo = driver.ContactNo,
                Email = driver.Email,
                Address1 = driver.Address1,
                Address2 = driver.Address2,
                DepartmentId = (int)driver.DepartmentId,
                DesignationId = (int)driver.DesignationId
            };
            if (driver.Status == null)
            {
                driver.Status = "Assigned";
            }
            else if (driver.Status == "Assigned")
            {
                driver.Status = "NULL";
            }
            else if (driver.Status == "NULL")
            {
                driver.Status = "Assigned";
            }

            _employeeManager.Update(driver);
        }

        private void VehicleStatusChange(int? vehicleId)
        {
            if (vehicleId == null)
            {
                return;
            }
            var vehicles = vehicleManager.GetById((int)vehicleId);
            if (vehicles.Status == null)
            {
                vehicles.Status = "Assigned";
            }
            else if (vehicles.Status == "Assigned")
            {
                vehicles.Status = "NULL";
            }
            else if (vehicles.Status == "NULL")
            {
                vehicles.Status = "Assigned";
            }
            vehicleManager.Update(vehicles);

        }
        public ActionResult AssignIndex()
        {
            Manager manager = new Manager();
            var employee = _employeeManager.GetAll();
            var vehicle = vehicleManager.GetAll();
            var managers = managerManager.Get(c => c.Status == null).OrderByDescending(c => c.Id);
            var requsition = _requisitionManager.GetAll();

            List<ManagerViewModel> managerViewModels = new List<ManagerViewModel>();
            foreach (var allData in managers)
            {
                var managerVM = new ManagerViewModel();
                managerVM.Id = allData.Id;
                managerVM.Employee = employee.Where(c => c.Id == allData.EmployeeId).FirstOrDefault();
                managerVM.Vehicle = vehicle.Where(c => c.Id == allData.VehicleId).FirstOrDefault();
                managerVM.Employee = employee.Where(c => c.Id == allData.EmployeeId).FirstOrDefault();
                managerVM.Requsition = requsition.Where(c => c.Id == allData.RequsitionId).FirstOrDefault();
                managerViewModels.Add(managerVM);
            }

            return View(managerViewModels);
        }
        public ActionResult DriverAndCar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var manager = managerManager.GetAll();
            Requsition requsition = _requisitionManager.GetById((int)id);
            var vehicle = vehicleManager.GetAll();
            var employee = _employeeManager.GetAll();

            ManagerViewModel managerVM = new ManagerViewModel();
            managerVM.RequsitionId = requsition.Id;
            managerVM.Vehicle = vehicle.Where(c => c.Id == managerVM.VehicleId).FirstOrDefault();
            managerVM.Employee = employee.Where(c => c.Id == managerVM.EmployeeId).FirstOrDefault();

            return View();
        }
        public ActionResult DriverMessage(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var manager = managerManager.GetById((int)id);
            var employee = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false);

            ManagerViewModel managerVM = new ManagerViewModel()
            {
                Id = manager.Id,
                Employee = employee.Where(c => c.Id == manager.EmployeeId).FirstOrDefault()
            };

            ViewBag.EmployeName = employee;
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var manager = managerManager.GetById((int)id);
            var requsition = _requisitionManager.GetAll();
            var employee = _employeeManager.GetAll();
            var vehicle = vehicleManager.GetAll();

            ManagerViewModel requisitionVm = new ManagerViewModel()
            {
                Id = manager.Id,
                Requsition = requsition.FirstOrDefault(c => c.Id == manager.RequsitionId),
                Vehicle = vehicle.FirstOrDefault(c => c.Id == manager.VehicleId),
                Employee = employee.FirstOrDefault(x => x.Id == manager.EmployeeId)
            };

            return View(requisitionVm);
        }
        public ActionResult RequsitionEmployeeName(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int ids = (int)id;
            var manager = managerManager.GetById((int)id);
            Manager managers = new Manager();
            manager.Id = ids;
            var requsition = _requisitionManager.GetAll();
            var data = requsition.Where(c => c.Id == manager.RequsitionId).FirstOrDefault();

            return View();

        }
        public ActionResult Car()
        {
            var vehicle = vehicleManager.GetAll();
            var vehicleType = vehicleTypeManager.GetAll();

            List<VehicleViewModel> vehicleViewModels = new List<VehicleViewModel>();
            foreach (var data in vehicle)
            {
                var vehicleVM = new VehicleViewModel();
                vehicleVM.Id = data.Id;
                vehicleVM.VehicleName = data.VehicleName;
                vehicleVM.VModel = data.VModel;
                vehicleVM.Description = data.Description;
                vehicleVM.VRegistrationNo = data.VRegistrationNo;
                vehicleVM.VChesisNo = data.VChesisNo;
                vehicleVM.VCapacity = data.VCapacity;
                vehicleVM.Status = data.Status;
                vehicleVM.VehicleType = vehicleType.Where(c => c.Id == data.VehicleTypeId).FirstOrDefault();
                vehicleViewModels.Add(vehicleVM);
            }
            ViewBag.TotalCar = vehicle.Count;
            return View(vehicleViewModels);
        }
        public ActionResult AssignCar()
        {
            var assignVehicle = vehicleManager.Get(c => c.Status == "Assigned");

            List<VehicleViewModel> vehicleViewModels = new List<VehicleViewModel>();
            foreach (var vehicle in assignVehicle)
            {
                var vehicleVM = new VehicleViewModel()
                {
                    Id = vehicle.Id,
                    VehicleName = vehicle.VehicleName,
                    VModel = vehicle.VModel,
                    Description = vehicle.Description,
                    VRegistrationNo = vehicle.VRegistrationNo
                };
                vehicleViewModels.Add(vehicleVM);
            }
            return View(vehicleViewModels);
        }
        public ActionResult NonAssignCar()
        {
            var assignVehicle = vehicleManager.Get(c => c.Status == "Null" || c.Status == null);

            List<VehicleViewModel> vehicleViewModels = new List<VehicleViewModel>();
            foreach (var vehicle in assignVehicle)
            {
                var vehicleVM = new VehicleViewModel()
                {
                    Id = vehicle.Id,
                    VehicleName = vehicle.VehicleName,
                    VModel = vehicle.VModel,
                    Description = vehicle.Description,
                    VRegistrationNo = vehicle.VRegistrationNo
                };
                vehicleViewModels.Add(vehicleVM);
            }
            return View(vehicleViewModels);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var manager = managerManager.GetById((int)id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            bool isDeleted = managerManager.Remove(manager);
            if (isDeleted)
            {
                return RedirectToAction("CompleteRequsition");
            }
            return View();
        }
        public void RequsitionComplete(int? id)
        {
            if (id == null)
            {
                return;
            }
            var employee = _employeeManager.GetAll();
            var requsition = _requisitionManager.GetById((int)id);

            RequsitionViewModel requisitionVm = new RequsitionViewModel()
            {
                Id = requsition.Id,
                Form = requsition.Form,
                To = requsition.To,
                Description = requsition.Description,
                JourneyStart = requsition.JourneyStart,
                JouneyEnd = requsition.JouneyEnd,
                Employee = employee.Where(x => x.Id == requsition.EmployeeId).FirstOrDefault()
            };

            requsition.Status = "Complete";
            bool assign = _requisitionManager.Update(requsition);
            return;
        }
        public ActionResult CheckIn(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var AssignManager = managerManager.GetById((int)id);

            AssignManager.Status = "RequsitionComplete";
            bool isUpdate = managerManager.Update(AssignManager);
            RequsitionComplete(AssignManager.RequsitionId);
            VehicleStatusChange(AssignManager.VehicleId);
            DriverAssigned(AssignManager.EmployeeId);

            if (isUpdate)
            {
                return RedirectToAction("AssignIndex");
            }


            return View();
        }
      
        public ActionResult CheckInUpdate(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var AssignManager = managerManager.GetById((int)id);

            //Manager manager = new Manager();
            //manager.Id = AssignManager.Id;
            //manager.RequsitionId= AssignManager.RequsitionId;
            //manager.VehicleId = AssignManager.VehicleId;
            //manager.EmployeeId = AssignManager.EmployeeId;
            AssignManager.Status = null;

            bool isUpdate = managerManager.Update(AssignManager);

            if (isUpdate)
            {
                return RedirectToAction("CheckOut");
            }


            return View();
        }

        public ActionResult CheckInConfirm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requsition requsition = new Requsition();
            var manager = managerManager.GetById((int)id);

            var emloyee = _employeeManager.Get(c => c.IsDriver == true && c.IsDeleted == false);
            var Driver = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false);
            var requsitions = _requisitionManager.Get(c => c.Status == "Assign");
            var vehicle = vehicleManager.GetAll();

            ManagerViewModel managerViewModel = new ManagerViewModel();
            managerViewModel.Id = manager.Id;
            managerViewModel.RequsitionId = manager.RequsitionId;
            managerViewModel.VehicleId = manager.VehicleId;
            managerViewModel.EmployeeId = (int)manager.EmployeeId;
            managerViewModel.Requsition = requsitions.FirstOrDefault(c => c.Id == manager.RequsitionId);
            managerViewModel.Vehicle = vehicle.Where(c => c.Id == manager.VehicleId).FirstOrDefault();
            managerViewModel.Employee = emloyee.Where(c => c.Id == manager.EmployeeId).FirstOrDefault();

            managerViewModel.Id = manager.Id;
            managerViewModel.RequsitionId = manager.RequsitionId;
            ViewBag.EmployeeId = new SelectList(Driver, "Id", "Name", manager.EmployeeId);
            ViewBag.VehicleId = new SelectList(vehicle, "Id", "VehicleName", manager.VehicleId);

            return View(managerViewModel);
        }
        [HttpPost]
        public ActionResult CheckInConfirm(ManagerViewModel managerViewModel)
        {
            Manager manager = new Manager();
            manager.Id = managerViewModel.Id;
            manager.RequsitionId = managerViewModel.RequsitionId;
            manager.VehicleId = managerViewModel.VehicleId;
            manager.EmployeeId = managerViewModel.EmployeeId;
            manager.Status = "ComplereRequsition";
            bool isUpdate = managerManager.Update(manager);
            if (isUpdate)
            {
                return RedirectToAction("CheckIn");
            }
            return View();
        }



        [HttpGet]
        public ActionResult CompleteRequsition()
        {
            //var Manager = managerManager.Get(c => c.Status == "RequsitionComplete" && c.IsDeleted == false).OrderByDescending(c => c.Id);

            var searchingValue = _requisitionManager.Get(c => c.Status == "Complete" && c.IsDeleted == false);
            string todays = DateTime.Today.ToShortDateString();
            var todayRequsition = searchingValue.Where(c => c.JourneyStart.ToShortDateString() == todays);

            var employee = _employeeManager.Get(c => c.IsDriver == true && c.IsDeleted == false);
            var vehicle = vehicleManager.GetAll();
            var requsition = _requisitionManager.GetAll();

            FilteringSearchViewModel filteringSearchViewModel = new FilteringSearchViewModel();



            List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            foreach (var item in todayRequsition)
            {
                var requisitionVM = new RequsitionViewModel();
                requisitionVM.Id = item.Id;
                requisitionVM.Employee = item.Employee;
                requisitionVM.Form = item.Form;
                requisitionVM.To = item.To;
                requisitionVM.Description = item.Description;
                requisitionVM.JourneyStart = item.JourneyStart;
                requisitionVM.JouneyEnd = item.JouneyEnd;
                requsitionViewModels.Add(requisitionVM);
            }
            filteringSearchViewModel.RequsitionViewModels = requsitionViewModels;

            return View(filteringSearchViewModel);
        }

        [HttpPost]
        public ActionResult CompleteRequsition(FilteringSearchViewModel filteringSearchViewModels)
        {

            var startTime = filteringSearchViewModels.Startdate;
            var endTime = filteringSearchViewModels.EndDate;

            var searchingValue = _requisitionManager.Get(c=>c.Status == "Complete" && c.IsDeleted == false);
            var selectedValue = searchingValue.Where(c => c.JourneyStart > startTime && c.JouneyEnd < endTime);

            List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            foreach (var item in selectedValue)
            {
                var requisitionVM = new RequsitionViewModel();
                requisitionVM.Id = item.Id;
                requisitionVM.Employee = item.Employee;
                requisitionVM.Form = item.Form;
                requisitionVM.To = item.To;
                requisitionVM.Description = item.Description;
                requisitionVM.JourneyStart = item.JourneyStart;
                requisitionVM.JouneyEnd = item.JouneyEnd;
                requsitionViewModels.Add(requisitionVM);
            }

            filteringSearchViewModels.RequsitionViewModels = requsitionViewModels;
            return PartialView("_CompleteRequsitionPartial", requsitionViewModels);
        }

        public ActionResult AssignIndexDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requsition requsition = new Requsition();
            var manager = managerManager.GetById((int)id);

            var emloyee = _employeeManager.Get(c => c.IsDriver == true && c.IsDeleted == false);
            var Driver = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false);
            var requsitions = _requisitionManager.Get(c => c.Status == "Assign");
            var vehicle = vehicleManager.GetAll();


            ManagerViewModel managerViewModel = new ManagerViewModel();
            managerViewModel.Status = manager.Status;
            managerViewModel.Requsition = requsitions.FirstOrDefault(c => c.Id == manager.RequsitionId);
            managerViewModel.Vehicle = vehicle.Where(c => c.Id == manager.VehicleId).FirstOrDefault();
            managerViewModel.Employee = emloyee.Where(c => c.Id == manager.EmployeeId).FirstOrDefault();
            return View(managerViewModel);
        }


        public ActionResult AssignReport(RequsitionAssignViewModel requsitionAssignViewModel)
        {
            var reportData = managerManager.RequsitionAssignReportViewModels(requsitionAssignViewModel);
            return View();
        }

        public ActionResult Hold(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employee = _employeeManager.GetAll();
            var requsition = _requisitionManager.GetById((int)id);

            //RequsitionViewModel requisitionVm = new RequsitionViewModel()
            //{
            //    Id = requsition.Id,
            //    Form = requsition.Form,
            //    To = requsition.To,
            //    RequsitionNumber = requsition.RequsitionNumber,
            //    Description = requsition.Description,
            //    JourneyStart = requsition.JourneyStart,
            //    JouneyEnd = requsition.JouneyEnd,
            //    Employee = employee.Where(x => x.Id == requsition.EmployeeId).FirstOrDefault()
            //};
            requsition.Status = "Hold";
            bool isUpdate = _requisitionManager.Update(requsition);
            if (isUpdate)
            {
                return RedirectToAction("New");
            }
            return View();
        }

        public ActionResult HoldIndex()
        {
            Requsition requsitions = new Requsition();
            var employee = _employeeManager.GetAll();
            var requsition = _requisitionManager.GetAllByNull(requsitions.Status = "Hold");

            List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            foreach (var data in requsition)
            {
                var requsitionVM = new RequsitionViewModel()
                {
                    Id = data.Id,
                    Form = data.Form,
                    To = data.To,
                    RequsitionNumber = data.RequsitionNumber,
                    Description = data.Description,
                    JourneyStart = data.JourneyStart,
                    JouneyEnd = data.JouneyEnd,
                    Employee = employee.Where(x => x.Id == data.EmployeeId).FirstOrDefault()
                };
                requsitionViewModels.Add(requsitionVM);
            }
            return View(requsitionViewModels);
        }

        public ActionResult AssignDriver()
        {
            var employee = _employeeManager.Get(c => c.Status == "Assigned");
            List<DriverViewModel> driverViewList = new List<DriverViewModel>();
            foreach (var data in employee)
            {
                var driverVm = new DriverViewModel();
                driverVm.Id = data.Id;
                driverVm.Name = data.Name;
                driverVm.ContactNo = data.ContactNo;
                driverVm.Email = data.Email;
                driverVm.Address1 = data.Address1;
                driverVm.Address2 = data.Address2;
                driverVm.LicenceNo = data.LicenceNo;
                driverVm.Department = data.Department;
                driverVm.Designation = data.Designation;
                driverViewList.Add(driverVm);
            }
            return View(driverViewList);
        }

        public ActionResult AvailableDriver()
        {
            var driver = _employeeManager.Get(c => c.Status == "NULL" || c.Status == null && c.IsDriver);
            List<DriverViewModel> driverViewList = new List<DriverViewModel>();
            foreach (var data in driver)
            {
                var driverVm = new DriverViewModel();
                driverVm.Id = data.Id;
                driverVm.Name = data.Name;
                driverVm.ContactNo = data.ContactNo;
                driverVm.Email = data.Email;
                driverVm.Address1 = data.Address1;
                driverVm.Address2 = data.Address2;
                driverVm.LicenceNo = data.LicenceNo;
                driverVm.Department = data.Department;
                driverVm.Designation = data.Designation;
                driverViewList.Add(driverVm);
            }
            return View(driverViewList);
        }

        public ActionResult Cancel(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           // var employee = _employeeManager.GetAll();
            var requsition = _requisitionManager.GetById((int)id);

            //RequsitionViewModel requisitionVm = new RequsitionViewModel()
            //{
            //    Id = requsition.Id,
            //    Form = requsition.Form,
            //    To = requsition.To,
            //    RequsitionNumber = requsition.RequsitionNumber,
            //    Description = requsition.Description,
            //    JourneyStart = requsition.JourneyStart,
            //    JouneyEnd = requsition.JouneyEnd,
            //    Employee = employee.Where(x => x.Id == requsition.EmployeeId).FirstOrDefault()
            //};
            requsition.Status = "Cancel";
            bool isUpdate = _requisitionManager.Update(requsition);
            if (isUpdate)
            {
                return RedirectToAction("New");
            }
            return View();
        }

        public ActionResult CancelIndex()
        {
            Requsition requsitions = new Requsition();
            var employee = _employeeManager.GetAll();
            var requsition = _requisitionManager.GetAllByNull(requsitions.Status = "Cancel");

            List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            foreach (var data in requsition)
            {
                var requsitionVM = new RequsitionViewModel()
                {
                    Id = data.Id,
                    Form = data.Form,
                    To = data.To,
                    RequsitionNumber = data.RequsitionNumber,
                    Description = data.Description,
                    JourneyStart = data.JourneyStart,
                    JouneyEnd = data.JouneyEnd,
                    Employee = employee.Where(x => x.Id == data.EmployeeId).FirstOrDefault()
                };
                requsitionViewModels.Add(requsitionVM);
            }
            return View(requsitionViewModels);
        }

        public ActionResult Print(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vehicle = vehicleManager.GetAll();
            var requsition = _requisitionManager.GetAll();
            var printManager = managerManager.GetById((int)id);

            List<ManagerViewModel> managerViewModels = new List<ManagerViewModel>();


            var vehicl = vehicle.FirstOrDefault(c => c.Id == printManager.VehicleId);
            var reqs = requsition.FirstOrDefault(c => c.Id == printManager.RequsitionId);
            string EmployeName = reqs.Employee.Name;
            string employeeNo = reqs.Employee.ContactNo;
            string designation = reqs.Employee.Designation.Name;
            string to = reqs.To;
            DateTime JouneyEnd = reqs.JouneyEnd;
            string Description = reqs.Description;
            DateTime JourneyStart = reqs.JourneyStart;
            string DriverName = printManager.Employee.Name;
            string vehicleModel = vehicl.VModel;

            var managersVM = new ManagerViewModel();
            managersVM.Id = printManager.Id;
            managersVM.EmployeeName = EmployeName;
            managersVM.EmployeNumber = employeeNo;
            managersVM.JourneyEnd = JouneyEnd;
            managersVM.To = to;
            managersVM.Description = Description;
            managersVM.JourneyStart = JourneyStart;
            managersVM.DriverName = DriverName;
            managersVM.VehicleModel = vehicleModel;
            managersVM.Designation = designation;

            managerViewModels.Add(managersVM);
            string reportpath = Request.MapPath(Request.ApplicationPath) + @"Report\AssignRequsition\RequsitionAssignRDLC.rdlc";
            var reportViewer = new ReportViewer()
            {
                KeepSessionAlive = true,
                SizeToReportContent = true,
                Width = Unit.Percentage(100),
                ProcessingMode = ProcessingMode.Local
            };
            reportViewer.LocalReport.ReportPath = reportpath;
            ReportDataSource rds = new ReportDataSource("AssignRequsition", managerViewModels);
            reportViewer.LocalReport.DataSources.Add(rds);
            ViewBag.ReportViewer = reportViewer;
            return View(managerViewModels);
        }

        public ActionResult Print_V2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requsition requisition = _requisitionManager.GetById((int)id);
            var driverId = driverStatusManager.Get(c => c.RequsitionId == id).Select(c => c.EmployeeId).FirstOrDefault();
            var vehicleId = vehicleStatusManager.Get(c => c.RequsitionId == id).Select(c => c.VehicleId).FirstOrDefault();

            AssignedListViewModel assignVm = new AssignedListViewModel
            {
                Requisition = requisition,
                Employee = _employeeManager.GetById((int)requisition.EmployeeId),
                Driver = _employeeManager.GetById(driverId),
                Vehicle = vehicleManager.GetById(vehicleId)
            };
          
            List<ManagerViewModel> managerViewModels = new List<ManagerViewModel>();

            var managersVM = new ManagerViewModel
            {
                Id = assignVm.Id,
                EmployeeName = assignVm.Employee.Name,
                EmployeNumber = assignVm.Employee.ContactNo,
                JourneyEnd = assignVm.Requisition.JouneyEnd,
                To = assignVm.Requisition.To,
                Description = assignVm.Requisition.Description,
                JourneyStart = assignVm.Requisition.JourneyStart,
                DriverName = assignVm.Driver.Name,
                VehicleModel = assignVm.Vehicle.Name,
                Designation = assignVm.Employee.Designation.Name
            };

            managerViewModels.Add(managersVM);
            string reportpath = Request.MapPath(Request.ApplicationPath) + @"Report\AssignRequsition\RequsitionAssignRDLC.rdlc";
            var reportViewer = new ReportViewer()
            {
                KeepSessionAlive = true,
                SizeToReportContent = true,
                Width = Unit.Percentage(100),
                ProcessingMode = ProcessingMode.Local
            };
            reportViewer.LocalReport.ReportPath = reportpath;
            ReportDataSource rds = new ReportDataSource("AssignRequsition", managerViewModels);
            reportViewer.LocalReport.DataSources.Add(rds);
            ViewBag.ReportViewer = reportViewer;
            return View(managerViewModels);
        }
        public ActionResult DriverAssignedList()
        {

            var employees = _employeeManager.Get(c => c.IsDriver);
            ViewBag.Employees = employees;
            return View();

        }

        [HttpPost]
        public ActionResult DriverAssignedList(int? employeeId)
        {
            List<DriverDutyViewModel> assignedList = new List<DriverDutyViewModel>();
            if (employeeId != null)
            {
                var vehicle = vehicleManager.GetAll();
                var vehicleStatus = vehicleStatusManager.Get(c => c.Status == "Assign").OrderByDescending(c => c.Id);
                var driverStatus = driverStatusManager.Get(c => c.Status == "Assign").Where(e => e.EmployeeId == employeeId).OrderByDescending(c => c.Id);
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
            return PartialView("_DriverDutyListPartial",assignedList);
        }

        public ActionResult DriverDutyCompletedList()
        {

            var employees = _employeeManager.Get(c => c.IsDriver);
            ViewBag.Employees = employees;
            return View();

        }

        [HttpPost]
        public ActionResult DriverDutyCompletedList(int? employeeId)
        {
            List<DriverDutyViewModel> assignedList = new List<DriverDutyViewModel>();
            if (employeeId != null)
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
            return PartialView("_DriverDutyListPartial", assignedList);
        }
       
        public ActionResult CheckIn_V2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            var vehicleStatus = vehicleStatusManager.Get(c=>c.RequsitionId==id).FirstOrDefault();
            var driverStatus = driverStatusManager.Get(c => c.RequsitionId == id).FirstOrDefault();
            var requsitionStatus = _requisitionManager.GetById((int)id);
            
            if (vehicleStatus != null&& driverStatus != null && requsitionStatus != null)
            {
                vehicleStatus.Status = "Complete";
                bool isVehicleStatusUpdate = vehicleStatusManager.Update(vehicleStatus);

                driverStatus.Status = "Complete";
                bool isDriverStatusUpdate = driverStatusManager.Update(driverStatus);

                requsitionStatus.Status = "Complete";
                bool isRequsitionUpdate = _requisitionManager.Update(requsitionStatus);

                if (isVehicleStatusUpdate && isDriverStatusUpdate && isRequsitionUpdate)
                {
                    TempData["msg"] = "Check in operaion successfully done!";
                    return RedirectToAction("AssignedList");
                }

            }

            TempData["msg"] = "Check in operaion failed.";
            return RedirectToAction("AssignedList");
            
        }

        private bool ForReassign_AddDataToDriverStatusTable(int? employeeId, int? id)
        {
            if (employeeId == null || id == null)
            {
                return false;
            }
            var maxId = driverStatusManager.GetAll().Max(c => c.Id) + 1;
            var dv = driverStatusManager.Get(c=>c.RequsitionId==id && c.Status=="Assign").FirstOrDefault();

            if (dv == null) return false;
            dv.Status = "Reassigned - Ref :"+maxId;
            bool isUpdated = driverStatusManager.Update(dv);
            return isUpdated;
        }
        private bool ForReassign_AddDataToVehicleStatusTable(int? vehicleId, int? id)
        {
            if (vehicleId == null || id == null)
            {
                return false;
            }
            var maxId = vehicleStatusManager.GetAll().Max(c=>c.Id) + 1;
            var vs = vehicleStatusManager.Get(c => c.RequsitionId == id && c.Status == "Assign").FirstOrDefault();

            if (vs == null) return false;
            vs.Status = "Reassigned - Ref :" + maxId;
            bool isUpdated = vehicleStatusManager.Update(vs);
            return isUpdated;
        }

        [HttpGet]
        public ActionResult Reassign_V2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requsition requsition = _requisitionManager.GetById((int)id);
            var presentDriverId =
                driverStatusManager.Get(c => c.RequsitionId == id && c.Status=="Assign").Select(c=>c.EmployeeId).FirstOrDefault();
          
            var availableDriverList = driverStatusManager.Get(c => c.EndTime < requsition.JourneyStart).Where(c=>c.EmployeeId!= presentDriverId).GroupBy(x => x.EmployeeId).Select(x => x.First());

            List<Employee> availableDrivers = new List<Employee>();

            foreach (var driver in availableDriverList)
            {
                var driverItem = _employeeManager.GetById(driver.EmployeeId);
                availableDrivers.Add(driverItem);
            }

            List<Vehicle> availableVehicles = new List<Vehicle>();
            var presentVehicleId =
               vehicleStatusManager.Get(c => c.RequsitionId == id && c.Status == "Assign").Select(c=>c.VehicleId).FirstOrDefault();

         
            var availableVehicleList = vehicleStatusManager.Get(c => c.EndTime < requsition.JourneyStart).Where(c=>c.VehicleId!= presentVehicleId).GroupBy(x => x.VehicleId).Select(x => x.First());
            foreach (var vehicle in availableVehicleList)
            {
                var vehicleItem = vehicleManager.GetById(vehicle.VehicleId);
                availableVehicles.Add(vehicleItem);
            }

            RessignViewModel assignVm = new RessignViewModel
            {
                RequsitionId = requsition.Id,
                Requsition = requsition,
                PresentDriver = _employeeManager.GetById(presentDriverId),
                PresentDriverId = presentDriverId,
                PresentVehicle = vehicleManager.GetById(presentVehicleId),
                PresentVehicleId = presentVehicleId,
                Drivers = availableDrivers,
                Vehicles = availableVehicles
            };

            ViewBag.Vehicles = new SelectList(availableVehicles, "Id", "Name");
            return View(assignVm);
        }

        [HttpPost]
        public ActionResult Reassign_V2(RessignViewModel assignVm)
        {

            bool isRequisitionAssigned = RequisitionAssign(assignVm.Id);
           
            bool isPresentDriverStatusChanged = ForReassign_AddDataToDriverStatusTable(assignVm.PresentDriverId, assignVm.Id);
            bool isPresentVehicleStatusChanged = ForReassign_AddDataToVehicleStatusTable(assignVm.PresentVehicleId, assignVm.Id);

            bool isDriverAssigned = AddDataToDriverStatusTable(assignVm.NewDriverId, assignVm.Id);
            bool isVehicleAssigned = AddDataToVehicleStatusTable(assignVm.NewVehicleId, assignVm.Id);
            if (isRequisitionAssigned && isPresentDriverStatusChanged && isPresentVehicleStatusChanged && isDriverAssigned && isVehicleAssigned)
            {
                TempData["msg"] = "Requisition Ressigned Successfully!";

                //Email Sending Method start
                //SendingEmailDriver(assignVm.EmployeeId, assignVm.RequsitionId);
                //SendingEmailEmployee(assignVm.EmployeeId, assignVm.RequsitionId);
                //Email Sending Method end

                return RedirectToAction("AssignedList");
            }

            //if (!isRequisitionAssigned && !isDriverAssigned)
            //{
            //    TempData["msg"] = "Requisition Not Assigned";
            //    return View(assignVm);
            //}

            TempData["msg"] = "Requisition Not Reassigned";
            return View(assignVm);
        }

        public void ForReassignSendEmailToDriver(int? EmployeeId, int? requsitionId,string cause)
        {
            if (EmployeeId == null && requsitionId == null)
            {
                return;
            }
            var requsition = _requisitionManager.GetById((int)requsitionId);
            var employee = _employeeManager.GetById((int)EmployeeId);
            if (employee.Email == null)
            {
                return;
            }
            var dod = "<span><strong>Employee Name</strong> :" + " " + requsition.Employee.Name + "</span>" + "<br/>"
                    + "<span> <strong>Employee Number</strong> :" + " " + requsition.Employee.ContactNo + "</span>" + "<br/>"
                    + "<span> <strong>Department Name</strong> :" + " " + requsition.Employee.Department.Name + "</span>" + "<br/>"
                    + "<span> <strong>Designation Name</strong> :" + " " + requsition.Employee.Designation.Name + "</span>" + "<br/>";

            var body = dod;
            var message = new MailMessage();
            message.To.Add(new MailAddress(employee.Email));  // replace with valid value 
            message.From = new MailAddress("mohammadziaulm62@gmail.com");  // replace with valid value
            message.Subject = "For Your Have A New Car Assign";
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "mohammadziaulm62@gmail.com", // replace with valid value
                    Password = "01915982924" // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
                return;
            }
        }

        public void ForReassignSendEmailToEmployee(int? EmployeeId, int? RequsitionId, string cause)
        {
            if (EmployeeId == null && RequsitionId == null)
            {
                return;
            }
            var requsition = _requisitionManager.GetById((int)RequsitionId);
            var employee = _employeeManager.GetById((int)EmployeeId);
            if (employee.Email == null)
            {
                return;
            }
            var dod = "<span><strong>Driver Name</strong> :" + " " + employee.Name + "</span>" + "<br/>"
                      + "<span> <strong>Phone Number</strong> :" + " " + employee.ContactNo + "</span>" + "<br/>";


            var body = dod;
            var message = new MailMessage();
            message.To.Add(new MailAddress(requsition.Employee.Email));  // replace with valid value 
            message.From = new MailAddress("mohammadziaulm62@gmail.com");  // replace with valid value
            message.Subject = "Your Requsition Assign Successfully";
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "mohammadziaulm62@gmail.com", // replace with valid value
                    Password = "01915982924" // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
                return;
            }
        }
    }
}