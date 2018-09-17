using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Repository.Contracts;
using VehicleManagementApp.Repository.Migrations;
using VehicleManagementApp.ViewModels;
using Requsition = VehicleManagementApp.Models.Models.Requsition;

namespace VehicleManagementApp.Controllers
{
    public class ManagerController : Controller
    {
        private IManagerManager managerManager;
        private IRequsitionManager _requsitionManager;
        private IEmployeeManager _employeeManager;
        private IVehicleManager vehicleManager;
        private IVehicleTypeManager vehicleTypeManager;

        public ManagerController(IRequsitionManager _requsition, IEmployeeManager employee, IManagerManager manager,
            IVehicleManager vehicle, IVehicleTypeManager vehicleType)
        {
            this._employeeManager = employee;
            this._requsitionManager = _requsition;
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
            var requsition = _requsitionManager.GetAllByNull(requsitions.Status = null);

            List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            foreach (var data in requsition)
            {
                var requsitionVM = new RequsitionViewModel()
                {
                    Id = data.Id,
                    Form = data.Form,
                    To = data.To,
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
            var requsition = _requsitionManager.GetById((int) id);

            RequsitionViewModel requsitionVM = new RequsitionViewModel()
            {
                Id = requsition.Id,
                Form = requsition.Form,
                To = requsition.To,
                Description = requsition.Description,
                JourneyStart = requsition.JourneyStart,
                JouneyEnd = requsition.JouneyEnd,
                Employee = employee.Where(x => x.Id == requsition.EmployeeId).FirstOrDefault()
            };
            requsition.Status = "Seen";
            _requsitionManager.Update(requsition);
            return View(requsitionVM);
        }
        public ActionResult RequsitionAssign(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var employee = _employeeManager.GetAll();
            var requsition = _requsitionManager.GetById((int)id);

            RequsitionViewModel requsitionVM = new RequsitionViewModel()
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
            bool assign = _requsitionManager.Update(requsition);
            if (assign)
            {
                return RedirectToAction("Assign");
            }
            return View(requsitionVM);
        }
        [HttpGet]
        public ActionResult Assign(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Requsition requsition = _requsitionManager.GetById((int) id);
            Manager manager = new Manager();
            var employees = _employeeManager.Get(c => c.IsDriver == true && c.IsDeleted == false);
            var assignVehicle = vehicleManager.Get(c => c.Status == null);

            ManagerViewModel managerVM = new ManagerViewModel();
            managerVM.Id = manager.Id;
            managerVM.RequsitionId = requsition.Id;

            managerVM.Employees = employees;
            managerVM.Vehicles = assignVehicle;

            //ViewBag.EmployeeId = new SelectList(employees, "Id", "Name", manager.EmployeeId);
            //ViewBag.VehicleId = new SelectList(assignVehicle, "Id", "VehicleName", manager.EmployeeId);
            
            return View(managerVM);
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

            bool isSaved = managerManager.Add(manager);
            RequsitionAssign(managerViewModel.Id);
            VehicleStatusChange(managerViewModel.VehicleId);
            DriverAssigned(managerViewModel.EmployeeId);


            if (isSaved)
            {
                TempData["msg"] = "Requsition Assign Successfully";
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
            
        }
        private void VehicleStatusChange(int? vehicleId)
        {
            if (vehicleId == null)
            {
                return;
            }
            var vehicles = vehicleManager.GetById((int)vehicleId);
            Vehicle vehicle = new Vehicle();
            vehicles.Status = "Assigned";
            vehicleManager.Update(vehicles);
            return;
        }
        public ActionResult AssignIndex()
        {
            Manager manager = new Manager();
            var employee = _employeeManager.GetAll();
            var vehicle = vehicleManager.GetAll();
            var managers = managerManager.GetAll();
            var requsition = _requsitionManager.GetAll();

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
        public ActionResult OnProgress()
        {
            Requsition requsition = new Requsition();
            var data = _requsitionManager.GetAllBySeen(requsition.Status = "Seen");
            var employee = _employeeManager.GetAll();
            List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            foreach (var allRequsition in data)
            {
                var requsitionVM = new RequsitionViewModel();
                requsitionVM.Id = allRequsition.Id;
                requsitionVM.Form = allRequsition.Form;
                requsitionVM.To = allRequsition.To;
                requsitionVM.Description = allRequsition.Description;
                requsitionVM.JourneyStart = allRequsition.JourneyStart;
                requsitionVM.JouneyEnd = allRequsition.JouneyEnd;
                requsitionVM.Employee = employee.Where(x => x.Id == allRequsition.EmployeeId).FirstOrDefault();

                requsitionViewModels.Add(requsitionVM);
            }
            return View(requsitionViewModels);
        }
        public ActionResult DriverAndCar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var manager = managerManager.GetAll();
            Requsition requsition = _requsitionManager.GetById((int)id);
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
                Employee = employee.Where(c=>c.Id == manager.EmployeeId).FirstOrDefault()
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
            var employee = _employeeManager.GetAll();
            var requsition = _requsitionManager.GetById((int)id);

            RequsitionViewModel requsitionVM = new RequsitionViewModel()
            {
                Id = requsition.Id,
                Form = requsition.Form,
                To = requsition.To,
                Description = requsition.Description,
                JourneyStart = requsition.JourneyStart,
                JouneyEnd = requsition.JouneyEnd,
                Employee = employee.FirstOrDefault(x => x.Id == requsition.EmployeeId)
            };
            //requsition.Status = "Seen";
           // _requsitionManager.Update(requsition);
            return View(requsitionVM);
        }
        public ActionResult RequsitionEmployeeName(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int ids = (int)id;
            var manager = managerManager.GetById((int) id);
            Manager managers = new Manager();
            manager.Id = ids;
            var requsition = _requsitionManager.GetAll();
            var data = requsition.Where(c => c.Id == manager.RequsitionId).FirstOrDefault();

            return View();

        }
        public ActionResult Complete()
        {
            var requsition = _requsitionManager.Get(c => c.Status == "Complete" && c.IsDeleted == false);
            var employee = _employeeManager.Get(c => c.IsDriver == false);


            List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            foreach (var Data in requsition)
            {
                var requsitionVM = new RequsitionViewModel();
                requsitionVM.Id = Data.Id;
                requsitionVM.Employee = employee.Where(c => c.Id == Data.EmployeeId).FirstOrDefault();
                requsitionVM.Description = Data.Description;
                requsitionVM.JourneyStart = Data.JourneyStart;
                requsitionVM.JouneyEnd = Data.JouneyEnd;
                requsitionVM.Form = Data.Form;
                requsitionVM.To = Data.To;
                requsitionViewModels.Add(requsitionVM);
            }
            
            return View(requsitionViewModels);
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
            var assignVehicle = vehicleManager.Get(c => c.Status == null);

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
            var manager = managerManager.GetById((int) id);
            if (manager == null)
            {
                return HttpNotFound();
            }
            bool isDeleted = managerManager.Remove(manager);
            if (isDeleted)
            {
                return RedirectToAction("AssignIndex");
            }
            return View();
        }

        public ActionResult CheckOut()
        {
            Manager manager = new Manager();
            var employee = _employeeManager.GetAll();
            var vehicle = vehicleManager.GetAll();
            var managers = managerManager.Get(c => c.Status != "Execute");
            var requsition = _requsitionManager.GetAll();

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

        public ActionResult CheckIn()
        {
            Manager manager = new Manager();
            var employee = _employeeManager.GetAll();
            var vehicle = vehicleManager.GetAll();
            var managers = managerManager.Get(c => c.Status == "Execute");
            var requsition = _requsitionManager.GetAll();

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
    }
}