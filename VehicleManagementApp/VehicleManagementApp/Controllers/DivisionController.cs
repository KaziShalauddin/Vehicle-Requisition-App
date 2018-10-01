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
    public class DivisionController : Controller
    {
        // GET: Division
        private IDivisionManager _divisionManager;

        public DivisionController(IDivisionManager manager)
        {
            this._divisionManager = manager;
        }
        public ActionResult Index()
        {
            var Division = _divisionManager.GetAll();
            return View(Division);
        }

        // GET: Division/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = _divisionManager.GetById((int)id);
            return View(division);
        }

        // GET: Division/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Division/Create
        [HttpPost]
        public ActionResult Create(DivisionViewModel divisionVM)
        {
            try
            {
                Division division = new Division();
                division.Name = divisionVM.Name;
                bool isSave = _divisionManager.Add(division);
                if (isSave)
                {
                    TempData["msg"] = "Division Saved Successfully";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Division/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = _divisionManager.GetById((int)id);
            if (division == null)
            {
                return HttpNotFound();
            }

            EditDivisionViewModel editDivisionViewModel = new EditDivisionViewModel();
            editDivisionViewModel.Id = division.Id;
            editDivisionViewModel.Name = division.Name;
            return View(editDivisionViewModel);
        }

        // POST: Division/Edit/5
        [HttpPost]
        public ActionResult Edit(EditDivisionViewModel divisionVM)
        {
            try
            {
                Division division = new Division();
                division.Id = divisionVM.Id;
                division.Name = divisionVM.Name;
                _divisionManager.Update(division);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Division/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = _divisionManager.GetById((int)id);
            bool isRemove = _divisionManager.Remove(division);
            if (isRemove)
            {
                return RedirectToAction("Index");
            }
            return View(division);
        }

        // POST: Division/Delete/5
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
            var name = _divisionManager.IsNameAlreadyExist(Name);
            return Json(name, JsonRequestBehavior.AllowGet);
        }
    }
}
