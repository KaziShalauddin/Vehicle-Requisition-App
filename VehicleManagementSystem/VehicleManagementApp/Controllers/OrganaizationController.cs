using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.BLL;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.ViewModels;

namespace VehicleManagementApp.Controllers
{
    public class OrganaizationController : Controller
    {
        // GET: Organaization
        //OrganaizationManager _organaizationManager = new OrganaizationManager();
        IOrganaizationManager _manager;

        public OrganaizationController(IOrganaizationManager manager)
        {
            this._manager = manager;
        }
        public ActionResult Index()
        {
            var data = _manager.GetAll();
            return View(data);
        }

        // GET: Organaization/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Organaization organaization = _manager.GetById((int)id);
            if (organaization == null)
            {
                return HttpNotFound();
            }
            return View(organaization);
        }

        // GET: Organaization/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Organaization/Create
        [HttpPost]
        public ActionResult Create(OrganaizationViewModels organaizationVM)
        {
            try
            {
                Organaization organaization = new Organaization();
                organaization.Name = organaizationVM.Name;
                organaization.Description = organaizationVM.Description;
                bool isSaved = _manager.Add(organaization);
                if (isSaved)
                {
                    TempData["msg"] = "Organaization Saved Successfully";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Organaization/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Organaization organaization = _manager.GetById((int)id);
            OrganaizationViewModels organaizationVM = new OrganaizationViewModels();
            organaizationVM.Id = organaization.Id;
            organaizationVM.Name = organaization.Name;
            organaizationVM.Description = organaization.Description;
            return View(organaizationVM);
        }

        // POST: Organaization/Edit/5
        [HttpPost]
        public ActionResult Edit(OrganaizationViewModels organaizationVM)
        {
            try
            {
                Organaization organaization = new Organaization();
                organaization.Id = organaizationVM.Id;
                organaization.Name = organaizationVM.Name;
                organaization.Description = organaizationVM.Description;
                bool isUpdate = _manager.Update(organaization);
                if (isUpdate)
                {
                    return RedirectToAction("Index");
                }
                
            }
            catch
            {
                //return View();
            }
            return View();
        }

        // GET: Organaization/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Organaization data = _manager.GetById((int)id);
            bool isDeleted = _manager.Remove(data);
            if (isDeleted)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Organaization/Delete/5
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

        public JsonResult IsNameExist(string Name)
        {
            var names = _manager.IsExistsByName(Name);
            return Json(names, JsonRequestBehavior.AllowGet);
        }
    }
}
