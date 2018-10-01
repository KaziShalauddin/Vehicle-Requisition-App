using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Repository.Contracts;
using VehicleManagementApp.ViewModels;

namespace VehicleManagementApp.Controllers
{
    public class ThanaController : Controller
    {
        // GET: Thana
        private IThanaManager _thanaManager;
        private IDistrictManager _districtManager;

        public ThanaController(IThanaManager thana, IDistrictManager district)
        {
            this._thanaManager = thana;
            this._districtManager = district;
        }
        public ActionResult Index()
        {
            var thana = _thanaManager.GetAll();
            var district = _districtManager.GetAll();

            List<ThanaViewModel> thanaViewModels = new List<ThanaViewModel>();
            foreach (var allthana in thana)
            {
                var thanaVM = new ThanaViewModel();
                thanaVM.Id = allthana.Id;
                thanaVM.Name = allthana.Name;
                thanaVM.District = district.Where(x => x.Id == allthana.DistrictId).FirstOrDefault();
                thanaViewModels.Add(thanaVM);
            }
            return View(thanaViewModels);
        }

        // GET: Thana/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var district = _districtManager.GetAll();
            Thana thana = _thanaManager.GetById((int)id);
            ThanaViewModel thanaVM = new ThanaViewModel()
            {
                Id = thana.Id,
                Name = thana.Name,
                District = district.Where(x => x.Id == thana.DistrictId).FirstOrDefault()
            };
            return View(thanaVM);
        }

        // GET: Thana/Create
        [HttpGet]
        public ActionResult Create()
        {
            var District = _districtManager.GetAll();
            ThanaViewModel thanaViewModel = new ThanaViewModel();
            thanaViewModel.Districts = District;
            return View(thanaViewModel);
        }

        // POST: Thana/Create
        [HttpPost]
        public ActionResult Create(ThanaViewModel thanaViewModel)
        {
            try
            {
                Thana thana = new Thana();
                thana.Name = thanaViewModel.Name;
                thana.DistrictId = thanaViewModel.DistrictId;

                bool isSaved = _thanaManager.Add(thana);
                if (isSaved)
                {
                    TempData["msg"] = "Upzilla Saved Successfully";
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Thana/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thana thana = _thanaManager.GetById((int) id);
            EditThanaViewModel thanaVM = new EditThanaViewModel()
            {
                Id = thana.Id,
                Name = thana.Name,
                DistrictId = thana.DistrictId
            };
            ViewBag.DistrictId = new SelectList(_districtManager.GetAll(),"Id","Name", thana.DistrictId);
            return View(thanaVM);
        }

        // POST: Thana/Edit/5
        [HttpPost]
        public ActionResult Edit(EditThanaViewModel thanaViewModel)
        {
            try
            {
                Thana thana = new Thana();
                thana.Id = thanaViewModel.Id;
                thana.Name = thanaViewModel.Name;
                thana.DistrictId = thanaViewModel.DistrictId;

                _thanaManager.Update(thana);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Thana/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Thana thana = _thanaManager.GetById((int)id);
            if (thana == null)
            {
                return HttpNotFound();
            }
            bool isRemove = _thanaManager.Remove(thana);
            if (isRemove)
            {
                return RedirectToAction("Index");
            }
            return View(thana);
        }

        // POST: Thana/Delete/5
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
        public JsonResult GetByDistrict(int? districtId)
        {
            if (districtId == null)
            {
                return null;
            }

            var thana = _thanaManager.GetAll();

            var divisionCat = thana.Where(x => x.DistrictId == districtId).ToList();

            return Json(divisionCat, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IsNameExist(string Name)
        {
            var name = _thanaManager.IsThanaAlreadyExist(Name);
            return Json(name, JsonRequestBehavior.AllowGet);
        }
    }
}
