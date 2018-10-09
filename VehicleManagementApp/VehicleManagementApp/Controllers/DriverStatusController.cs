using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VehicleManagementApp.Controllers
{
    public class DriverStatusController : Controller
    {
        // GET: DriverStatus
        public ActionResult Index()
        {
            return View();
        }

        // GET: DriverStatus/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DriverStatus/Create
        public ActionResult Add()
        {
            return View();
        }

        // POST: DriverStatus/Create
        [HttpPost]
        public ActionResult Add(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: DriverStatus/Edit/5
        public ActionResult Update(int id)
        {
            return View();
        }

        // POST: DriverStatus/Edit/5
        [HttpPost]
        public ActionResult Update(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

    }
}
