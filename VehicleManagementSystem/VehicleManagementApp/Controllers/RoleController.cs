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
    public class RoleController : Controller
    {
        // GET: Role
        private IRoleManager _roleManager;

        public RoleController(IRoleManager manager)
        {
            this._roleManager = manager;
        }
        public ActionResult Index()
        {
            return View(_roleManager.GetAll());
        }

        // GET: Role/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Role/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Role/Create
        [HttpPost]
        public ActionResult Create(RoleViewModel roleVM)
        {
            try
            {
                Role role = new Role();
                role.Name = roleVM.Name;
                bool isSaved = _roleManager.Add(role);
                if (isSaved)
                {
                    TempData["msg"] = "Role Save Successfully";
                }
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        // GET: Role/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Role role = _roleManager.GetById((int)id);
            RoleViewModel roleVM = new RoleViewModel();
            roleVM.Id = role.Id;
            roleVM.Name = role.Name;
            return View(roleVM);
        }

        // POST: Role/Edit/5
        [HttpPost]
        public ActionResult Edit(RoleViewModel roleVM)
        {
            if (ModelState.IsValid)
            {
                Role role = new Role();
                role.Id = roleVM.Id;
                role.Name = roleVM.Name;
                bool isUpdate = _roleManager.Update(role);

                if (isUpdate)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();

        }

        // GET: Role/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Role role = _roleManager.GetById((int)id);
            bool isDeleted = _roleManager.Remove(role);
            if (isDeleted)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Role/Delete/5
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
