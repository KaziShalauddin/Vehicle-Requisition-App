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
    public class VehicleTypeController : Controller
    {
        // GET: VehicleType
        private IVehicleTypeManager _typeManager;

        public VehicleTypeController(IVehicleTypeManager manager)
        {
            this._typeManager = manager;
        }
        public ActionResult Index()
        {
            var data = _typeManager.GetAll();
            return View(data);
        }

        // GET: VehicleType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleType vehicleType = _typeManager.GetById((int)id);
            if (vehicleType == null)
            {
                return HttpNotFound();
            }
            return View(vehicleType);
        }

        // GET: VehicleType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehicleType/Create
        [HttpPost]
        public ActionResult Create(VehicleTypeViewModel vehicleTypeVM)
        {
            try
            {
                VehicleType vehicleType = new VehicleType();
                vehicleType.TypeName = vehicleTypeVM.TypeName;
                bool isSaved = _typeManager.Add(vehicleType);
                if (isSaved)
                {
                    TempData["msg"] = "Vehicle Type Saved Successfully";
                }
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        // GET: VehicleType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            VehicleType vehicleType = _typeManager.GetById((int)id);

            VehicleTypeViewModel vehicleTypeVM = new VehicleTypeViewModel()
            {
                Id = vehicleType.Id,
                TypeName = vehicleType.TypeName
            };
            return View(vehicleTypeVM);
        }

        // POST: VehicleType/Edit/5
        [HttpPost]
        public ActionResult Edit(VehicleTypeViewModel vehicleTypeVM)
        {
            try
            {
                VehicleType vehicleType = new VehicleType()
                {
                    Id = vehicleTypeVM.Id,
                    TypeName = vehicleTypeVM.TypeName
                };
                _typeManager.Update(vehicleType);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: VehicleType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var data = _typeManager.GetById((int)id);
            bool isDeleted = _typeManager.Remove(data);
            if (isDeleted)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: VehicleType/Delete/5
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
    }
}
