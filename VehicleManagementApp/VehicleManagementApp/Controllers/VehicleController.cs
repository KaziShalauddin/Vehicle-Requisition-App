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
    public class VehicleController : Controller
    {
        // GET: Vehicle
        private IVehicleManager _vehicleManager;
        private IVehicleTypeManager _typeManager;

        public VehicleController(IVehicleManager manager, IVehicleTypeManager typeManager)
        {
            this._vehicleManager = manager;
            this._typeManager = typeManager;
        }
        public ActionResult Index()
        {
            var vehicle = _vehicleManager.GetAll();
            var vehicleType = _typeManager.GetAll();

            List<VehicleViewModel> vehicleVM = new List<VehicleViewModel>();
            foreach (var vehicledata in vehicle)
            {
                var vechileViewModel = new VehicleViewModel();
                vechileViewModel.Id = vehicledata.Id;
                vechileViewModel.VehicleName = vehicledata.VehicleName;
                vechileViewModel.VModel = vehicledata.VModel;
                vechileViewModel.VRegistrationNo = vehicledata.VRegistrationNo;
                vechileViewModel.VChesisNo = vehicledata.VChesisNo;
                vechileViewModel.VCapacity = vehicledata.VCapacity;
                vechileViewModel.Description = vehicledata.Description;
                vechileViewModel.VehicleType = vehicleType.Where(x => x.Id == vehicledata.VehicleTypeId).FirstOrDefault();
                vehicleVM.Add(vechileViewModel);
            }
            return View(vehicleVM);
        }
        public ActionResult VehicleList()
        {
            var vehicle = _vehicleManager.GetAll();
            var vehicleType = _typeManager.GetAll();

            List<VehicleViewModel> vehicleVM = new List<VehicleViewModel>();
            foreach (var vehicledata in vehicle)
            {
                var vechileViewModel = new VehicleViewModel();
                vechileViewModel.Id = vehicledata.Id;
                vechileViewModel.VehicleName = vehicledata.VehicleName;
                vechileViewModel.VModel = vehicledata.VModel;
                vechileViewModel.VRegistrationNo = vehicledata.VRegistrationNo;
                vechileViewModel.VChesisNo = vehicledata.VChesisNo;
                vechileViewModel.VCapacity = vehicledata.VCapacity;
                vechileViewModel.Description = vehicledata.Description;
                vechileViewModel.VehicleType = vehicleType.Where(x => x.Id == vehicledata.VehicleTypeId).FirstOrDefault();
                vehicleVM.Add(vechileViewModel);
            }
            ViewBag.TotalVehicle = vehicle.Count;
            return View(vehicleVM);
        }

        // GET: Vehicle/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vehicleType = _typeManager.GetAll();
            Vehicle vehicle = _vehicleManager.GetById((int)id);

            VehicleViewModel vehicleVM = new VehicleViewModel()
            {
              Id  = vehicle.Id,
              VehicleName = vehicle.VehicleName,
              VModel = vehicle.VModel,
              VRegistrationNo = vehicle.VRegistrationNo,
              VChesisNo = vehicle.VChesisNo,
              VCapacity = vehicle.VCapacity,
              Description = vehicle.Description,
              VehicleType = vehicleType.Where(x=>x.Id == vehicle.VehicleTypeId).FirstOrDefault()
            };
            return View(vehicleVM);
        }

        // GET: Vehicle/Create
        [HttpGet]
        public ActionResult Create()
        {
            VehicleViewModel vehicleVM = new VehicleViewModel();
            var data = _typeManager.GetAll();
            vehicleVM.VehicleTypes = data;
            return View(vehicleVM);
        }

        // POST: Vehicle/Create
        [HttpPost]
        public ActionResult Create(VehicleViewModel vehicleViewModel)
        {
            try
            {
                Vehicle vehicle = new Vehicle();

                vehicle.VehicleName = vehicleViewModel.VehicleName;
                vehicle.VModel = vehicleViewModel.VModel;
                vehicle.VRegistrationNo = vehicleViewModel.VRegistrationNo;
                vehicle.VChesisNo = vehicleViewModel.VChesisNo;
                vehicle.VCapacity = vehicleViewModel.VCapacity;
                vehicle.Description = vehicleViewModel.Description;
                vehicle.VehicleTypeId = vehicleViewModel.VehicleTypeId;
                
                bool isSaved = _vehicleManager.Add(vehicle);
                if (isSaved)
                {
                    TempData["msg"] = "Vehicle Saved Successfully";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Vehicle/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Vehicle vehicle = _vehicleManager.GetById((int)id);
            EditVehicleViewModel vehicleVM = new EditVehicleViewModel()
            {
                Id = vehicle.Id,
                VehicleName = vehicle.VehicleName,
                VModel = vehicle.VModel,
                VRegistrationNo = vehicle.VRegistrationNo,
                VChesisNo = vehicle.VChesisNo,
                VCapacity = vehicle.VCapacity,
                Description = vehicle.Description,
                VehicleTypeId = vehicle.VehicleTypeId
            };
            ViewBag.VehicleTypeId = new SelectList(_typeManager.GetAll(),"Id", "TypeName",vehicle.VehicleTypeId);
            return View(vehicleVM);
        }

        // POST: Vehicle/Edit/5
        [HttpPost]
        public ActionResult Edit(EditVehicleViewModel vehicleVM)
        {
            try
            {
                Vehicle vehicle = new Vehicle()
                {
                    Id = vehicleVM.Id,
                    VehicleName = vehicleVM.VehicleName,
                    VModel = vehicleVM.VModel,
                    VRegistrationNo = vehicleVM.VRegistrationNo,
                    VChesisNo = vehicleVM.VChesisNo,
                    VCapacity = vehicleVM.VCapacity,
                    Description = vehicleVM.Description,
                    VehicleTypeId = vehicleVM.VehicleTypeId
                };
                _vehicleManager.Update(vehicle);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Vehicle/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Vehicle vehicle = _vehicleManager.GetById((int) id);
            bool isRemove = _vehicleManager.Remove(vehicle);
            if (isRemove)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Vehicle/Delete/5
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

        public JsonResult IsNameExist(string VModel)
        {
            var name = _vehicleManager.IsNameAlreadyExist(VModel);
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsRegistrationExist(string VRegistrationNo)
        {
            var registration = _vehicleManager.IsRegistrationAlreadyExist(VRegistrationNo);
            return Json(registration, JsonRequestBehavior.AllowGet);
        }
    }
}
