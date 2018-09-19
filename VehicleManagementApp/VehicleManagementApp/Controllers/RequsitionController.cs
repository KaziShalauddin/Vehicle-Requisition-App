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
        private IRequsitionManager _requisitionManager;
        private IEmployeeManager _employeeManager;
        //private IRequsitionStatusManager _requsitionStatusManager;
        private IManagerManager _managerManager;
        private IVehicleManager vehicleManager;
        //IRequsitionStatusManager requsitionStatus,
        public RequsitionController(IRequsitionManager requisition, IEmployeeManager employee,  IManagerManager manager, IVehicleManager vehicle)
        {
            this._requisitionManager = requisition;
            this._employeeManager = employee;
            //this._requsitionStatusManager = requsitionStatus;
            this._managerManager = manager;
            this.vehicleManager = vehicle;
        }
        public ActionResult Index()
        {

            GetRequisitionComplete();

            var allRequisitions = _requisitionManager.GetAll();
            var employee = _employeeManager.GetAll();
            //var requstionStatus = _requsitionStatusManager.GetAll();

            List<RequisitionViewModel> requisitionViewList = new List<RequisitionViewModel>();
            foreach (var requisition in allRequisitions)
            {
                var requisitionVM = new RequisitionViewModel();
                requisitionVM.Id = requisition.Id;
                requisitionVM.Form = requisition.Form;
                requisitionVM.To = requisition.To;
                requisitionVM.Description = requisition.Description;
                requisitionVM.JourneyStart = requisition.JourneyStart;
                requisitionVM.JouneyEnd = requisition.JouneyEnd;
                requisitionVM.Employee = employee.Where(x => x.Id == requisition.EmployeeId).FirstOrDefault();
                requisitionVM.Status = requisition.Status;
                requisitionViewList.Add(requisitionVM);
            }
            return View(requisitionViewList);
        }

        private TempDataDictionary data;
        public ActionResult RequisitionIndex()
        {
            RequsitionCreateViewModel allRequsitions = new RequsitionCreateViewModel();
            var employees = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false);

            ViewBag.Employees = employees.ToList();
            ViewBag.TempData = data;
            var requsitionViewList = RequisitionListView();
            allRequsitions.RequsitionViewModels = requsitionViewList;
            return View(allRequsitions);
        }

        public JsonResult JsonCreate(RequisitionViewModel requisitionVm)
        {

            Requsition requisition = new Requsition();
            requisition.Form = requisitionVm.Form;
            requisition.To = requisitionVm.To;
            requisition.Description = requisitionVm.Description;
            requisition.JourneyStart = requisitionVm.JourneyStart;
            requisition.JouneyEnd = requisitionVm.JouneyEnd;
            requisition.EmployeeId = requisitionVm.EmployeeId;

            bool isSaved = _requisitionManager.Add(requisition);
            if (isSaved)
            {
                TempData["msg"] = "Requisition Send Successfully";
            }
            else
            {
                TempData["msg"] = "Requisition Send Successfully";
            }

            return Json(JsonRequestBehavior.AllowGet);

        }

        private List<RequisitionViewModel> RequisitionListView()
        {
            GetRequisitionComplete();

            var allRequisitions = _requisitionManager.GetAll();
            var employee = _employeeManager.GetAll();
            //var requstionStatus = _requsitionStatusManager.GetAll();

            List<RequisitionViewModel> requisitionViewList = new List<RequisitionViewModel>();
            foreach (var requisition in allRequisitions)
            {
                var requisitionVM = new RequisitionViewModel();
                requisitionVM.Id = requisition.Id;
                requisitionVM.Form = requisition.Form;
                requisitionVM.To = requisition.To;
                requisitionVM.Description = requisition.Description;
                requisitionVM.JourneyStart = requisition.JourneyStart;
                requisitionVM.JouneyEnd = requisition.JouneyEnd;
                requisitionVM.Employee = employee.Where(x => x.Id == requisition.EmployeeId).FirstOrDefault();
                requisitionVM.Status = requisition.Status;
                requisitionViewList.Add(requisitionVM);
            }
            return requisitionViewList;
        }

        private void GetRequisitionComplete()
        {
            var allRequisitions = _requisitionManager.GetAll();
            foreach (var request in allRequisitions)
            {
                var today = DateTime.Now;
                if (request.JouneyEnd < today)
                {
                    request.Status = "Complete";
                    _requisitionManager.Update(request);
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

            Requsition requisition = _requisitionManager.GetById((int)id);
            var employee = _employeeManager.Get(c => c.IsDriver == true && c.IsDeleted == false);


            CommentViewModel commentViewModel = new CommentViewModel();

            commentViewModel.RequsitionViewModelId = requisition.Id;
            commentViewModel.RequisitionViewModel.Form = requisition.Form;
            commentViewModel.RequisitionViewModel.To = requisition.To;
            commentViewModel.RequisitionViewModel.Description = requisition.Description;
            commentViewModel.RequisitionViewModel.JourneyStart = requisition.JourneyStart;
            commentViewModel.RequisitionViewModel.JouneyEnd = requisition.JouneyEnd;

            //commentViewModel.RequsitionViewModel.EmployeeId = requsition.EmployeeId;

            //RequsitionViewModel requsitionViewModel = new RequsitionViewModel();
            //requsitionViewModel.Id = requsition.Id;
            //requsitionViewModel.Employee = employee.Where(c => c.Id == requsition.EmployeeId).FirstOrDefault();


            return View(commentViewModel);
        }


        // GET: Requsition/Create
        public ActionResult Create()
        {
            var employee = _employeeManager.GetAll();
            var empl = employee.Where(c => c.IsDriver == false);
            var employees = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false);

            RequisitionViewModel requisitionVM = new RequisitionViewModel();

            requisitionVM.Employees = empl;
            return View(requisitionVM);
        }

        // POST: Requsition/Create
        [HttpPost]
        public ActionResult Create(RequisitionViewModel requisitionVm)
        {
            try
            {
                Requsition requisition = new Requsition();
                requisition.Form = requisitionVm.Form;
                requisition.To = requisitionVm.To;
                requisition.Description = requisitionVm.Description;
                requisition.JourneyStart = requisitionVm.JourneyStart;
                requisition.JouneyEnd = requisitionVm.JouneyEnd;
                requisition.EmployeeId = requisitionVm.EmployeeId;

                bool isSaved = _requisitionManager.Add(requisition);
                if (isSaved)
                {
                    TempData["msg"] = "Requisition Send Successfully";
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
            Requsition requisition = _requisitionManager.GetById((int)id);
            RequisitionViewModel requisitionView = new RequisitionViewModel();

            requisitionView.Id = requisition.Id;
            requisitionView.Form = requisition.Form;
            requisitionView.To = requisition.To;
            requisitionView.Description = requisition.Description;
            requisitionView.JourneyStart = requisition.JourneyStart;
            requisitionView.JouneyEnd = requisition.JouneyEnd;
            requisitionView.EmployeeId = (int) requisition.EmployeeId;

            ViewBag.EmployeeId = new SelectList(_employeeManager.GetAll(), "Id", "Name", requisition.EmployeeId);

            return View(requisitionView);
        }

        // POST: Requsition/Edit/5
        [HttpPost]
        public ActionResult Edit(RequisitionViewModel requisitionVm)
        {
            try
            {
                Requsition reqiusition = new Requsition();
                reqiusition.Id = requisitionVm.Id;
                reqiusition.Form = requisitionVm.Form;
                reqiusition.To = requisitionVm.To;
                reqiusition.Description = requisitionVm.Description;
                reqiusition.JourneyStart = requisitionVm.JourneyStart;
                reqiusition.JouneyEnd = requisitionVm.JouneyEnd;
                reqiusition.EmployeeId = requisitionVm.EmployeeId;

                _requisitionManager.Update(reqiusition);

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
            Requsition requisition = _requisitionManager.GetById((int)id);
            bool isRemove = _requisitionManager.Remove(requisition);
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
