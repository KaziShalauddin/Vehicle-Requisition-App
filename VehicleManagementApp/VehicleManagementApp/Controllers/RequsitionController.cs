using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.ViewModels;

namespace VehicleManagementApp.Controllers
{
    [Authorize]
    public class RequsitionController : Controller
    {
        // GET: Requsition
        private IRequsitionManager _requisitionManager;
        private IEmployeeManager _employeeManager;
        private ICommentManager commentManager;
        //private IRequsitionStatusManager _requsitionStatusManager;
        private IManagerManager _managerManager;
        private IVehicleManager vehicleManager;
        //IRequsitionStatusManager requsitionStatus,
        public RequsitionController(IRequsitionManager requisition, IEmployeeManager employee,  IManagerManager manager, IVehicleManager vehicle, ICommentManager comment)
        {
            this._requisitionManager = requisition;
            this._employeeManager = employee;
            this.commentManager = comment;
            this._managerManager = manager;
            this.vehicleManager = vehicle;
        }
        public ActionResult Index()
        {

            GetRequisitionComplete();

            var allRequisitions = _requisitionManager.GetAll();
            var employee = _employeeManager.GetAll();
            //var requstionStatus = _requsitionStatusManager.GetAll();

            List<RequsitionViewModel> requisitionViewList = new List<RequsitionViewModel>();
            foreach (var requisition in allRequisitions)
            {
                var requisitionVM = new RequsitionViewModel();
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


        // GET: Requsition/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int requsitionId = (int)id;
            Requsition requsition = _requisitionManager.GetById(requsitionId);
            if (requsition == null)
            {
                return HttpNotFound();
            }
            var employee = _employeeManager.Get(c => c.IsDriver == true && c.IsDeleted == false);
            var manager = _managerManager.GetAll();
            RequsitionViewModel requsitionViewModel = new RequsitionViewModel();
            requsitionViewModel.Id = requsition.Id;
            requsitionViewModel.Form = requsition.Form;
            requsitionViewModel.To = requsition.To;
            requsitionViewModel.Description = requsition.Description;
            requsitionViewModel.Employee = requsition.Employee;
            requsitionViewModel.Manager = manager.FirstOrDefault(c => c.RequsitionId == requsition.Id);

            int? emplId = requsition.EmployeeId;
            string employeeNam = requsition.Employee.Name;
            requsitionViewModel.CommentViewModel = new CommentViewModel
            {
                EmployeeId = (int)emplId,
                EmployeName = employeeNam,
                RequsitionId = requsitionId
            };

            //Collect the list of comment to display the list under comment
            List<CommentViewModel> commentListViewModel = new List<CommentViewModel>();
            var commentListView = commentManager.GetCommentsByRequisition(requsitionId);
            foreach (var item in commentListView.ToList())
            {
                commentListViewModel.Add
                (
                    new CommentViewModel { RequsitionId = requsitionId, Comments = item.Comments, EmployeeId = item.EmployeeId,  EmployeName = item.Employee.Name}
                );
            }
            requsitionViewModel.CommentViewModels = commentListViewModel;

            return View(requsitionViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(CommentViewModel commentViewModel)
        {
            Comment comment = new Comment();
            comment.RequsitionId = commentViewModel.RequsitionId;
            comment.Comments = commentViewModel.Comments;
            comment.EmployeeId = commentViewModel.EmployeeId;
            bool isSaved = commentManager.Add(comment);

            List<CommentViewModel> commentListViewModel = new List<CommentViewModel>();

            if (isSaved)
            {
                //Collect the list of comment to display the list under comment
                var commentListView = commentManager.GetCommentsByRequisition(commentViewModel.RequsitionId);

                foreach (var item in commentListView.ToList())
                {
                    var cmnt = new CommentViewModel();
                    cmnt.RequsitionId = item.RequsitionId;
                    cmnt.EmployeeId = item.EmployeeId;
                    cmnt.Comments = item.Comments;
                    cmnt.Employee = item.Employee;
                    cmnt.EmployeName = commentViewModel.EmployeName;
                    commentListViewModel.Add(cmnt);

                }
                return PartialView("_CommentList", commentListViewModel);
            }
            return PartialView("_CommentList", commentListViewModel);
        }


        public ActionResult RequisitionIndex()
        {
            RequsitionCreateViewModel allRequsitions = new RequsitionCreateViewModel();
            var employees = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false);

            ViewBag.Employees = employees.ToList();
            //if (data["msg"] != null)
            //{
            //    ViewBag.Result = data["msg"];
            //}
            
            var requsitionViewList = RequisitionListView();
            allRequsitions.RequsitionViewModels = requsitionViewList;
            return View(allRequsitions);
        }
        public ActionResult MyRequisitionList()
        {
            ApplicationUser user =
                System.Web.HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            RequsitionCreateViewModel allRequsitions = new RequsitionCreateViewModel();
            var employees = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false&&c.UserId==user.Id);

            ViewBag.Employees = employees.ToList();
            //if (data["msg"] != null)
            //{
            //    ViewBag.Result = data["msg"];
            //}

            var requsitionViewList = RequisitionListView();
            allRequsitions.RequsitionViewModels = requsitionViewList;
            return View(allRequsitions);
        }

        public string AutoNumber()
        {
            string year = DateTime.Now.Year.ToString();
            string month;
            if (DateTime.Now.Month < 10)
            {
                month ="0"+ DateTime.Now.Month;
            }
            else
            {
                month =  DateTime.Now.Month.ToString();
            }
           
            int day = DateTime.Now.Day;
            string time = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string second = DateTime.Now.Second.ToString();

            string yearMonth ="RQ-"+ second + minute + time + day + month + year;
            return yearMonth;
        }


        public JsonResult JsonCreate(RequsitionCreateViewModel requisitionVm)
        {
            //newDateTime = date.Date + time.TimeOfDay;

            if (ModelState.IsValid)
            {
                var journeyStart = requisitionVm.JourneyStartDate.Date + requisitionVm.JourneyStartTime.TimeOfDay;
                var jouneyEnd = requisitionVm.JouneyEndDate.Date + requisitionVm.JouneyEndTime.TimeOfDay;

                Requsition requisition = new Requsition();
                requisition.Form = requisitionVm.Form;
                requisition.To = requisitionVm.To;
                requisition.RequsitionNumber = AutoNumber();
                requisition.Description = requisitionVm.Description;
                requisition.JourneyStart = journeyStart;
                requisition.JouneyEnd = jouneyEnd;
                requisition.EmployeeId = requisitionVm.EmployeeId;

                bool isSaved = _requisitionManager.Add(requisition);
                if (isSaved)
                {
                    TempData["msg"] = "Requisition Send Successfully";
                   
                }
                else
                {
                    TempData["msg"] = "Requisition not Send !";
                }
            }
            else
            {
                TempData["msg"] = "Requisition not Send !";
            }
            return Json(TempData["msg"], JsonRequestBehavior.AllowGet);

        }

        private List<RequsitionViewModel> RequisitionListView()
        {
            GetRequisitionComplete();

            var allRequisitions = _requisitionManager.GetAll();
            
            var employee = _employeeManager.GetAll();
            //var requstionStatus = _requsitionStatusManager.GetAll();

            List<RequsitionViewModel> requisitionViewList = new List<RequsitionViewModel>();
            foreach (var requisition in allRequisitions)
            {
                var requisitionVM = new RequsitionViewModel();
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
        
        // GET: Requsition/Create
        public ActionResult Create()
        {
            var employee = _employeeManager.GetAll();
            var empl = employee.Where(c => c.IsDriver == false);
            var employees = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false);

            RequsitionViewModel requisitionVM = new RequsitionViewModel();

            requisitionVM.Employees = empl;
            return View(requisitionVM);
        }

        // POST: Requsition/Create
        [HttpPost]
        public ActionResult Create(RequsitionViewModel requisitionVm)
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
            RequsitionViewModel requisitionView = new RequsitionViewModel();

            requisitionView.Id = requisition.Id;
            requisitionView.Form = requisition.Form;
            requisitionView.To = requisition.To;
            requisitionView.Description = requisition.Description;
            requisitionView.JourneyStart = requisition.JourneyStart;
            requisitionView.JouneyEnd = requisition.JouneyEnd;
            requisitionView.EmployeeId = (int)requisition.EmployeeId;

            ViewBag.EmployeeId = new SelectList(_employeeManager.GetAll(), "Id", "Name", requisition.EmployeeId);

            return View(requisitionView);
        }

        // POST: Requsition/Edit/5
        [HttpPost]
        public ActionResult Edit(RequsitionViewModel requisitionVm)
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
