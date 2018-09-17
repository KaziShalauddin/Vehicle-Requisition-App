using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.ViewModels;

namespace VehicleManagementApp.Controllers
{
    public class RequsitionStatusController : Controller
    {
        // GET: RequsitionStatus
        private IRequsitionStatusManager requsitionStatusManager;
        private IRequsitionManager _requsitionManager;

        public RequsitionStatusController(IRequsitionStatusManager manager, IRequsitionManager _requsition)
        {
            this._requsitionManager = _requsition;
            this.requsitionStatusManager = manager;
        }
        public ActionResult Index()
        {
            var requsitionStatus = requsitionStatusManager.GetAll();
            return View(requsitionStatus);
        }

        // GET: RequsitionStatus/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RequsitionStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RequsitionStatus/Create
        [HttpPost]
        public ActionResult Create(RequsitionStatusViewModel requsitionStatusVM)
        {
            try
            {
                RequsitionStatus requsitionStatus = new RequsitionStatus();
                requsitionStatus.StatusName = requsitionStatusVM.StatusName;
                bool isSaved = requsitionStatusManager.Add(requsitionStatus);
                if (isSaved)
                {
                    TempData["msg"] = "Requsiton Status Save Successfuly";
                }
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        // GET: RequsitionStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            RequsitionStatus requsitionStatus = requsitionStatusManager.GetById((int)id);
            RequsitionStatusViewModel requsitionStatusVM = new RequsitionStatusViewModel();
            requsitionStatusVM.Id = requsitionStatus.Id;
            requsitionStatusVM.StatusName = requsitionStatus.StatusName;
            return View(requsitionStatusVM);
        }

        // POST: RequsitionStatus/Edit/5
        [HttpPost]
        public ActionResult Edit(RequsitionStatusViewModel requsitionStatusVM)
        {
            try
            {
                RequsitionStatus requsitionStatus = new RequsitionStatus();
                requsitionStatus.Id = requsitionStatusVM.Id;
                requsitionStatus.StatusName = requsitionStatusVM.StatusName;
                requsitionStatusManager.Update(requsitionStatus);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: RequsitionStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            RequsitionStatus requsitionStatus =  requsitionStatusManager.GetById((int) id);
            bool isRemove = requsitionStatusManager.Remove(requsitionStatus);
            if (isRemove)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: RequsitionStatus/Delete/5
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
        ////////new 
        



    }
}
