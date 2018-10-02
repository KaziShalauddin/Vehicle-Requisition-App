using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Models.ReportViewModel;
using VehicleManagementApp.Repository.Contracts;
using VehicleManagementApp.ViewModels;
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
        private IVehicleTypeManager vehicleTypeManager;

        public ManagerController(IRequsitionManager requisition, IEmployeeManager employee, IManagerManager manager,
            IVehicleManager vehicle, IVehicleTypeManager vehicleType)
        {
            this._employeeManager = employee;
            this._requisitionManager = requisition;
            this.managerManager = manager;
            this.vehicleManager = vehicle;
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
            var requsition = _requisitionManager.GetAllByNull(requsitions.Status = null).OrderByDescending(c=>c.Id);

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
            return View(requisitionVm);
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
            var employees = _employeeManager.Get(c => c.IsDriver == true && c.Status == "Available" && c.IsDeleted == false);
            var assignVehicle = vehicleManager.Get(c => c.Status == "Available" && c.IsDeleted == false);

            ManagerViewModel managerVM = new ManagerViewModel();
            managerVM.Id = manager.Id;
            managerVM.RequsitionId = requsition.Id;

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


        public void SendingEmailDriver(int? EmployeeId, int? requsitionId)
        {
            if (EmployeeId == null && requsitionId == null)
            {
                return;
            }
            var requsition = _requisitionManager.GetById((int)requsitionId);
            var employee = _employeeManager.GetById((int)EmployeeId);

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

        [HttpPost]
        public ActionResult Assign(ManagerViewModel managerViewModel)
        {
            Requsition requsition = new Requsition();
            Manager manager = new Manager();
            manager.Id = managerViewModel.Id;
            manager.DriverNo = managerViewModel.DriverNo;
            manager.RequsitionId = managerViewModel.RequsitionId;
            manager.EmployeeId = managerViewModel.EmployeeId;
            manager.VehicleId = managerViewModel.VehicleId;

            //Email Sending Methon start
            SendingEmailDriver(managerViewModel.EmployeeId, managerViewModel.RequsitionId);
            SendingEmailEmployee(managerViewModel.EmployeeId, managerViewModel.RequsitionId);
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
            if (driver.Status == "Available")
            {
                driver.Status = "Assigned";
            }
            else
            {
                driver.Status = "Available";
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
            if (vehicles.Status == "NULL")
            {
                vehicles.Status = "Assigned";
            }
            else
            {
                vehicles.Status = "NULL";
            }

            vehicleManager.Update(vehicles);

        }
        public ActionResult AssignIndex()
        {
            Manager manager = new Manager();
            var employee = _employeeManager.GetAll();
            var vehicle = vehicleManager.GetAll();
            var managers = managerManager.Get(c => c.Status == null).OrderByDescending(c=>c.Id);
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
            var assignVehicle = vehicleManager.Get(c => c.Status == "Null");

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

        public ActionResult CheckOut()
        {
            Manager manager = new Manager();
            var employee = _employeeManager.GetAll();
            var vehicle = vehicleManager.GetAll();
            var managers = managerManager.Get(c => c.Status != "Execute");
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

        public ActionResult CheckOutEdit(int? id)
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
            AssignManager.Status = "Execute";

            bool isUpdate = managerManager.Update(AssignManager);

            if (isUpdate)
            {
                return RedirectToAction("CheckOut");
            }


            return View();
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
        public ActionResult CompleteRequsition()
        {
            var Manager = managerManager.Get(c => c.Status == "RequsitionComplete" && c.IsDeleted == false).OrderByDescending(c=>c.Id);

            var employee = _employeeManager.Get(c => c.IsDriver == true && c.IsDeleted == false);
            var vehicle = vehicleManager.GetAll();
            var requsition = _requisitionManager.GetAll();

            List<ManagerViewModel> managerViewModels = new List<ManagerViewModel>();
            foreach (var manager in Manager)
            {
                var managerVM = new ManagerViewModel();
                managerVM.Id = manager.Id;
                managerVM.Status = manager.Status;
                managerVM.Employee = employee.Where(c => c.Id == manager.EmployeeId).FirstOrDefault();
                managerVM.Vehicle = vehicle.Where(c => c.Id == manager.VehicleId).FirstOrDefault();
                managerVM.Employee = employee.Where(c => c.Id == manager.EmployeeId).FirstOrDefault();
                managerVM.Requsition = requsition.Where(c => c.Id == manager.RequsitionId).FirstOrDefault();

                managerViewModels.Add(managerVM);
            }

            return View(managerViewModels);
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

        public ActionResult Hole(int? id)
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
            var driver = _employeeManager.Get(c => c.Status == "Available");
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

        public ActionResult Cancle(int? id)
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
            requsition.Status = "Cancle";
            bool isUpdate = _requisitionManager.Update(requsition);
            if (isUpdate)
            {
                return RedirectToAction("New");
            }
            return View();
        }

        public ActionResult CancleIndex()
        {
            Requsition requsitions = new Requsition();
            var employee = _employeeManager.GetAll();
            var requsition = _requisitionManager.GetAllByNull(requsitions.Status = "Cancle");

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
    }
}