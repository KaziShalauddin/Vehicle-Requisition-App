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
    public class DepartmentController : Controller
    {
        // GET: Department
        private IDepartmentManager _departmentManager;
        private IOrganaizationManager _organaizationManager;

        public DepartmentController(IDepartmentManager manager, IOrganaizationManager organaization)
        {
            this._departmentManager = manager;
            this._organaizationManager = organaization;
        }
        public ActionResult Index()
        {
            var organaization = _organaizationManager.GetAll();
            var Department = _departmentManager.GetAll();

            List<DepartmentViewModel> departmentVM = new List<DepartmentViewModel>();
            foreach (var department in Department)
            {
                var departmentViewModel = new DepartmentViewModel();

                departmentViewModel.Id = department.Id;
                departmentViewModel.Name = department.Name;
                departmentViewModel.Organaization = organaization.Where(x => x.Id == department.OrganaizationId).FirstOrDefault();

                departmentVM.Add(departmentViewModel);
            }
            return View(departmentVM);
        }

        // GET: Department/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var organaization = _organaizationManager.GetAll();

            Department department = _departmentManager.GetById((int)id);
            
            DepartmentViewModel departmentVM = new DepartmentViewModel()
            {
                Name = department.Name,
                Organaization = organaization.Where(x=>x.Id == department.OrganaizationId).FirstOrDefault()
            };
            return View(departmentVM);
        }

        // GET: Department/Create
        [HttpGet]
        public ActionResult Create()
        {
            DepartmentViewModel department = new DepartmentViewModel();
            var organaization = _organaizationManager.GetAll();
            department.Organaizations = organaization;
            return View(department);
        }

        // POST: Department/Create
        [HttpPost]
        public ActionResult Create(DepartmentViewModel departmentVM)
        {
            try
            {
                Department department = new Department();
                department.Name = departmentVM.Name;
                department.OrganaizationId = departmentVM.OrganaizationId;
                bool isSaved =  _departmentManager.Add(department);
                if (isSaved)
                {
                    TempData["msg"] = "Department Save Successfully";
                }
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        // GET: Department/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Department department = _departmentManager.GetById((int)id);
            DepartmentViewModel departmentVM = new DepartmentViewModel()
            {
                Id = department.Id,
                Name = department.Name,
                OrganaizationId = department.OrganaizationId
            };
            ViewBag.OrganaizationId = new SelectList(_organaizationManager.GetAll(),"Id","Name", department.OrganaizationId);
            return View(departmentVM);
        }

        // POST: Department/Edit/5
        [HttpPost]
        public ActionResult Edit(DepartmentViewModel departmentVM)
        {
            try
            {
                Department department = new Department();
                department.Id = departmentVM.Id;
                department.Name = departmentVM.Name;
                department.OrganaizationId = departmentVM.OrganaizationId;
                _departmentManager.Update(department);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Department/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Department department = _departmentManager.GetById((int)id);
            bool isDeleted = _departmentManager.Remove(department);
            if (isDeleted)
            {
                return RedirectToAction("Index");
            }
            return View(department);
        }

        // POST: Department/Delete/5
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
