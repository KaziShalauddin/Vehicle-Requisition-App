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
    public class RequsitionController : Controller
    {
        // GET: Requsition
        private IRequsitionManager _requsitionManager;
        private IEmployeeManager _employeeManager;
        private IRequsitionStatusManager _requsitionStatusManager;
        private IManagerManager _managerManager;
        private IVehicleManager vehicleManager;

        public RequsitionController(IRequsitionManager requsition, IEmployeeManager employee, IRequsitionStatusManager requsitionStatus, IManagerManager manager, IVehicleManager vehicle)
        {
            this._requsitionManager = requsition;
            this._employeeManager = employee;
            this._requsitionStatusManager = requsitionStatus;
            this._managerManager = manager;
            this.vehicleManager = vehicle;
        }
        public ActionResult Index()
        {

            GetRequsitionComplete();

            var requsition = _requsitionManager.GetAll();
            var employee = _employeeManager.GetAll();
            var requstionStatus = _requsitionStatusManager.GetAll();

            List<RequsitionViewModel> requsitionViewList = new List<RequsitionViewModel>();
            foreach (var allRequsition in requsition)
            {
                var requsitionVM = new RequsitionViewModel();
                requsitionVM.Id = allRequsition.Id;
                requsitionVM.Form = allRequsition.Form;
                requsitionVM.To = allRequsition.To;
                requsitionVM.Description = allRequsition.Description;
                requsitionVM.JourneyStart = allRequsition.JourneyStart;
                requsitionVM.JouneyEnd = allRequsition.JouneyEnd;
                requsitionVM.Employee = employee.Where(x => x.Id == allRequsition.EmployeeId).FirstOrDefault();
                requsitionVM.Status = allRequsition.Status;
                requsitionViewList.Add(requsitionVM);
            }
            return View(requsitionViewList);
        }

        private TempDataDictionary data;
        public ActionResult RequisitionIndex()
        {
            RequsitionCreateViewModel requsitionVM = new RequsitionCreateViewModel();
            var employees = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false);

            ViewBag.Employees = employees.ToList();
            ViewBag.TempData = data;
            var requsitionViewList = RequsitionListView();
            requsitionVM.RequsitionViewModels = requsitionViewList;
            return View(requsitionVM);
        }

        public JsonResult JsonCreate(RequsitionViewModel requsitionVM)
        {

            Requsition requsition = new Requsition();
            requsition.Form = requsitionVM.Form;
            requsition.To = requsitionVM.To;
            requsition.Description = requsitionVM.Description;
            requsition.JourneyStart = requsitionVM.JourneyStart;
            requsition.JouneyEnd = requsitionVM.JouneyEnd;
            requsition.EmployeeId = requsitionVM.EmployeeId;

            bool isSaved = _requsitionManager.Add(requsition);
            if (isSaved)
            {
                TempData["msg"] = "Requsition Send Successfully";
            }
            else
            {
                TempData["msg"] = "Requsition Send Successfully";
            }

            return Json(JsonRequestBehavior.AllowGet);

        }

        private List<RequsitionViewModel> RequsitionListView()
        {
            GetRequsitionComplete();

            var requsition = _requsitionManager.GetAll();
            var employee = _employeeManager.GetAll();
            var requstionStatus = _requsitionStatusManager.GetAll();

            List<RequsitionViewModel> requsitionViewList = new List<RequsitionViewModel>();
            foreach (var allRequsition in requsition)
            {
                var requsitionVM = new RequsitionViewModel();
                requsitionVM.Id = allRequsition.Id;
                requsitionVM.Form = allRequsition.Form;
                requsitionVM.To = allRequsition.To;
                requsitionVM.Description = allRequsition.Description;
                requsitionVM.JourneyStart = allRequsition.JourneyStart;
                requsitionVM.JouneyEnd = allRequsition.JouneyEnd;
                requsitionVM.Employee = employee.Where(x => x.Id == allRequsition.EmployeeId).FirstOrDefault();
                requsitionVM.Status = allRequsition.Status;
                requsitionViewList.Add(requsitionVM);
            }
            return requsitionViewList;
        }

        private void GetRequsitionComplete()
        {
            var requsition = _requsitionManager.GetAll();
            foreach (var allRequest in requsition)
            {
                var today = DateTime.Now;
                if (allRequest.JouneyEnd < today)
                {
                    allRequest.Status = "Complete";
                    _requsitionManager.Update(allRequest);
                }
            }
        }

        // GET: Requsition/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Requsition requsition = _requsitionManager.GetById((int)id);
            var employee = _employeeManager.Get(c => c.IsDriver == true && c.IsDeleted == false);


            CommentViewModel commentViewModel = new CommentViewModel();

            commentViewModel.RequsitionViewModelId = requsition.Id;
            commentViewModel.RequsitionViewModel.Form = requsition.Form;
            commentViewModel.RequsitionViewModel.To = requsition.To;
            commentViewModel.RequsitionViewModel.Description = requsition.Description;
            commentViewModel.RequsitionViewModel.JourneyStart = requsition.JourneyStart;
            commentViewModel.RequsitionViewModel.JouneyEnd = requsition.JouneyEnd;

            //commentViewModel.RequsitionViewModel.EmployeeId = requsition.EmployeeId;

            //RequsitionViewModel requsitionViewModel = new RequsitionViewModel();
            //requsitionViewModel.Id = requsition.Id;
            //requsitionViewModel.Employee = employee.Where(c => c.Id == requsition.EmployeeId).FirstOrDefault();


            return View(commentViewModel);
        }


        // GET: Requsition/Create
        public ActionResult Create()
        {
            //var employee = _employeeManager.GetAll();
            var employee = _employeeManager.GetAll();
            var empl = employee.Where(c => c.IsDriver == false);
            var employees = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false);

            RequsitionViewModel requsitionVM = new RequsitionViewModel();

            requsitionVM.Employees = empl;
            return View(requsitionVM);
        }

        // POST: Requsition/Create
        [HttpPost]
        public ActionResult Create(RequsitionViewModel requsitionVM)
        {
            try
            {
                Requsition requsition = new Requsition();
                requsition.Form = requsitionVM.Form;
                requsition.To = requsitionVM.To;
                requsition.Description = requsitionVM.Description;
                requsition.JourneyStart = requsitionVM.JourneyStart;
                requsition.JouneyEnd = requsitionVM.JouneyEnd;
                requsition.EmployeeId = requsitionVM.EmployeeId;

                bool isSaved = _requsitionManager.Add(requsition);
                if (isSaved)
                {
                    TempData["msg"] = "Requsition Send Successfully";
                    data = TempData;
                    return RedirectToAction("RequisitionIndex", TempData["msg"]);
                }
                return RedirectToAction("Create");
            }
            catch
            {
                return View();
            }
        }

        // GET: Requsition/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Requsition requsition = _requsitionManager.GetById((int)id);
            RequsitionViewModel requsitionView = new RequsitionViewModel();

            requsitionView.Id = requsition.Id;
            requsitionView.Form = requsition.Form;
            requsitionView.To = requsition.To;
            requsitionView.Description = requsition.Description;
            requsitionView.JourneyStart = requsition.JourneyStart;
            requsitionView.JouneyEnd = requsition.JouneyEnd;
            requsitionView.EmployeeId = requsition.EmployeeId;

            ViewBag.EmployeeId = new SelectList(_employeeManager.GetAll(), "Id", "Name", requsition.EmployeeId);

            return View(requsitionView);
        }

        // POST: Requsition/Edit/5
        [HttpPost]
        public ActionResult Edit(RequsitionViewModel requsitionVM)
        {
            try
            {
                Requsition requsition = new Requsition();
                requsition.Id = requsitionVM.Id;
                requsition.Form = requsitionVM.Form;
                requsition.To = requsitionVM.To;
                requsition.Description = requsitionVM.Description;
                requsition.JourneyStart = requsitionVM.JourneyStart;
                requsition.JouneyEnd = requsitionVM.JouneyEnd;
                requsition.EmployeeId = requsitionVM.EmployeeId;

                _requsitionManager.Update(requsition);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Requsition/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Requsition requsition = _requsitionManager.GetById((int)id);
            bool isRemove = _requsitionManager.Remove(requsition);
            if (isRemove)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // POST: Requsition/Delete/5
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
