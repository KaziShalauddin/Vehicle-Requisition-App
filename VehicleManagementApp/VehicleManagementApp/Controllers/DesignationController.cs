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
    public class DesignationController : Controller
    {
        // GET: Designation
        //DesignationManager _designationManager = new DesignationManager();
        //OrganaizationManager _organaizationManager = new OrganaizationManager();

        private IDesignationManager _designationManager;
        private IDepartmentManager _departmenManager;
        public DesignationController(IDesignationManager manager, IDepartmentManager departmenManager)
        {
            this._designationManager = manager;
            this._departmenManager = departmenManager;
        }
        public ActionResult Index()
        {
            var departments = _departmenManager.GetAll();
            var designation = _designationManager.GetAll();
            
            List<DesignationViewModel> designationList = new List<DesignationViewModel>();
            foreach (var designation1 in designation)
            {
                var designationVM = new DesignationViewModel();
                designationVM.Id = designation1.Id;
                designationVM.Name = designation1.Name;
                designationVM.Department = departments.Where(x => x.Id == designation1.DepartmentId).FirstOrDefault();
                designationList.Add(designationVM);
            } 
            return View(designationList);
        }

        // GET: Designation/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var departments = _departmenManager.GetAll();
            Designation designation = _designationManager.GetById((int) id);

            DesignationViewModel designationVM = new DesignationViewModel()
            {
                Id = designation.Id,
                Name = designation.Name,
                Department = departments.Where(x=>x.Id == designation.DepartmentId).FirstOrDefault()

            };
            return View(designationVM);
        }

        // GET: Designation/Create
        [HttpGet]
        public ActionResult Create()
        {
            DesignationViewModel designationVM = new DesignationViewModel();
            var departments = _departmenManager.GetAll();
            designationVM.Departments = departments;

            return View(designationVM);
        }

        // POST: Designation/Create
        [HttpPost]
        public ActionResult Create(DesignationViewModel designationVM)
        {
            try
            {
                Designation designation = new Designation();
                designation.Name = designationVM.Name;
                designation.DepartmentId = designationVM.DepartmentId;
                _designationManager.Add(designation);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Designation/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Designation designation = _designationManager.GetById((int) id);
            DesignationViewModel designationVM = new DesignationViewModel();
            designationVM.Id = designation.Id;
            designationVM.Name = designation.Name;
            designationVM.Departments = _departmenManager.GetAll();
            //designationVM.DepartmentId = designation.DepartmentId;
            //ViewBag.DesignationId = new SelectList(_departmenManager.GetAll(),"Id","Name", designation.DepartmentId);
            //return View(designationVM);
            return View(designationVM);
        }

        // POST: Designation/Edit/5
        [HttpPost]
        public ActionResult Edit(DesignationViewModel designationVM)
        {
            try
            {
                Designation designation = new Designation();
                designation.Id = designationVM.Id;
                designation.Name = designationVM.Name;
                designation.DepartmentId = designationVM.DepartmentId;
                _designationManager.Update(designation);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Designation/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var data = _designationManager.GetById((int)id);
            bool isDeleted = _designationManager.Remove(data);
            if (isDeleted)
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }

        // POST: Designation/Delete/5
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
            var name = _designationManager.IsNameAlreadyExist(Name);
            return Json(name, JsonRequestBehavior.AllowGet);

           
        }

        public JsonResult IsNameUnique(string name,int departmentId)
        {
            var searchDesignation = _designationManager.IsNameUnique(name, departmentId); 
            return Json(searchDesignation, JsonRequestBehavior.AllowGet);
            
        }
    }
}
