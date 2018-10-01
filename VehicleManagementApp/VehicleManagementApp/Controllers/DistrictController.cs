using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.ViewModels;

namespace VehicleManagementApp.Controllers
{
    public class DistrictController : Controller
    {
        // GET: District
        private IDistrictManager _districtManager;
        private IDivisionManager _divisionManager;

        public DistrictController(IDistrictManager district, IDivisionManager division)
        {
            this._districtManager = district;
            this._divisionManager = division;
        }
        public ActionResult Index()
        {
            var district = _districtManager.GetAll();
            var division = _divisionManager.GetAll();

            List<DistrictViewModel> districtVM = new List<DistrictViewModel>();
            foreach (var dist in district)
            {
                var districtList = new DistrictViewModel();
                districtList.Id = dist.Id;
                districtList.Name = dist.Name;
                districtList.Division = division.Where(x => x.Id == dist.DivisionId).FirstOrDefault();
                districtVM.Add(districtList);
            }
            return View(districtVM);
        }

        // GET: District/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var division = _divisionManager.GetAll();
            District district = _districtManager.GetById((int)id);
            DistrictViewModel districtVM = new DistrictViewModel()
            {
                Id = district.Id,
                Name = district.Name,
                Division = division.Where(x=>x.Id == district.DivisionId).FirstOrDefault()
            };
            return View(districtVM);
        }

        // GET: District/Create
        [HttpGet]
        public ActionResult Create()
        {
            var division = _divisionManager.GetAll();
            DistrictViewModel districtVM = new DistrictViewModel();
            districtVM.Divisions = division;
            return View(districtVM);
        }

        // POST: District/Create
        [HttpPost]
        public ActionResult Create(DistrictViewModel districtVM)
        {
            try
            {
                District district = new District();
                district.Name = districtVM.Name;
                district.DivisionId = districtVM.DivisionId;
                bool isSaved = _districtManager.Add(district);
                if (isSaved)
                {
                    TempData["msg"] = "District Save Successfully";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: District/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = _districtManager.GetById((int)id);
            EditDistrictViewModel districtVM = new EditDistrictViewModel();
            districtVM.Id = district.Id;
            districtVM.Name = district.Name;
            districtVM.DivisionId = district.DivisionId;

            ViewBag.DivisionId = new SelectList(_divisionManager.GetAll(),"Id","Name", district.DivisionId);

            return View(districtVM);
        }

        // POST: District/Edit/5
        [HttpPost]
        public ActionResult Edit(EditDistrictViewModel districtVM)
        {
            try
            {
                District district = new District();
                district.Id = districtVM.Id;
                district.Name = districtVM.Name;
                district.DivisionId = districtVM.DivisionId;
                _districtManager.Add(district);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: District/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            District district = _districtManager.GetById((int)id);
            if (district == null)
            {
                return HttpNotFound();
            }
            bool isRemove = _districtManager.Remove(district);
            if (isRemove)
            {
                return RedirectToAction("Index");
            }
            return View(district);
        }

        // POST: District/Delete/5
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

        public JsonResult GetByDivision(int? divisionId)
        {
            if (divisionId == null)
            {
                return null;
            }

            var district = _districtManager.GetAll();
            var divisionCat = district.Where(x => x.DivisionId == divisionId).ToList();

            return Json(divisionCat, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsNameExist(string Name)
        {
            var name = _districtManager.IsNameAlreadyExist(Name);
            return Json(name, JsonRequestBehavior.AllowGet);
        }
    }
}
