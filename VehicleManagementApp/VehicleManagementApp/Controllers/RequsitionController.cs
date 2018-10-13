using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.UI.WebControls.Expressions;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
using Microsoft.Reporting.WebForms;
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
            //GetRequisitionComplete();

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
            requsitionViewModel.JourneyStart = requsition.JourneyStart;
            requsitionViewModel.JouneyEnd = requsition.JouneyEnd;
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
                    new CommentViewModel
                    {
                        RequsitionId = requsitionId,
                        Comments = item.Comments,
                        EmployeeId = item.EmployeeId,
                        EmployeName = item.Employee.Name,
                        UserName = item.UserName,
                        UserId = item.UserId,
                        CommentTime = item.CommentTime
                    }
                );
            }
            requsitionViewModel.CommentViewModels = commentListViewModel;

            return View(requsitionViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment(CommentViewModel commentViewModel)
        {
            
            var userId = User.Identity.GetUserId();
            var userName = User.Identity.Name;


            Comment comment = new Comment();
            comment.RequsitionId = commentViewModel.RequsitionId;
            comment.Comments = commentViewModel.Comments;
            comment.EmployeeId = commentViewModel.EmployeeId;
            comment.UserId = userId;
            comment.UserName = userName;
            comment.CommentTime = DateTime.Now;
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
                    cmnt.Employee = item.Employee;
                    cmnt.UserId = item.UserId;
                    cmnt.UserName = item.UserName;
                    cmnt.CommentTime = item.CommentTime;
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
            var employees = _employeeManager.Get( c => c.IsDriver == false && c.IsDeleted == false);

            ViewBag.Employees = employees.ToList();

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

            var allRequisitions = _requisitionManager.GetAll().OrderByDescending(c=>c.Id);
            
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
        public bool IsEmployeeIdProvided(int? employeeId,bool requestForOther)
        {
            if (requestForOther)
            {
                if (employeeId == 0)
                    return false;
            }
            return true;

        }
        public ActionResult MyRequisitionList()
        {


            MyRequsitionCreateViewModel allRequsitions = new MyRequsitionCreateViewModel();
            allRequsitions.RequestTypes = GetRequisitionTypes();

           int myEmployeeId = GetEmployeeId();


            var employees = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false && c.Id!=myEmployeeId);
            ViewBag.Employees = employees.ToList();
           
            var requsitionViewList = MyRequisitionListView();
            allRequsitions.RequsitionViewModels = requsitionViewList;
            return View(allRequsitions);
        }
        public ActionResult MyRequestList()
        {
            var requsitionViewList = MyRequisitionListView();
            return PartialView("_MyRequisitionListPartial",requsitionViewList);
        }
        public JsonResult MyJsonCreate(MyRequsitionCreateViewModel requisitionVm)
        {
            //newDateTime = date.Date + time.TimeOfDay;
            

            if (ModelState.IsValid)
            {
                int requestForEmployeeId;
                if (requisitionVm.RequestForOther == false)
                {
                    requestForEmployeeId = GetEmployeeId();

                }
                else
                {
                    requestForEmployeeId = (int)requisitionVm.EmployeeId;
                }


                var journeyStart = requisitionVm.JourneyStartDate.Date + requisitionVm.JourneyStartTime.TimeOfDay;
                var jouneyEnd = requisitionVm.JouneyEndDate.Date + requisitionVm.JouneyEndTime.TimeOfDay;
               
                

                Requsition requisition = new Requsition();
                requisition.Form = requisitionVm.Form;
                requisition.To = requisitionVm.To;
                requisition.RequsitionNumber = AutoNumber();
                requisition.Description = requisitionVm.Description;
                requisition.JourneyStart = journeyStart;
                requisition.JouneyEnd = jouneyEnd;
                requisition.EmployeeId = requestForEmployeeId;
                
                requisition.RequestedBy = GetEmployeeId();
                requisition.RequestType = requisitionVm.RequestType;

                bool isSaved = _requisitionManager.Add(requisition);
                if (isSaved)
                {
                    TempData["msg"] = "Requisition Send Successfully";
                   
                }
                else
                {
                    TempData["msg"] = "Requisition not Send !";
                }
               
                return Json(TempData["msg"], JsonRequestBehavior.AllowGet);
            }
            TempData["msg"] = "Requisition not Send !";
            return Json(TempData["msg"], JsonRequestBehavior.AllowGet);

        }
        private List<MyRequsitionListViewModel> MyRequisitionListView()
        {
            GetRequisitionComplete();
            var employeeId = GetEmployeeId();

            var allRequisitions = _requisitionManager.Get(r => r.RequestedBy == employeeId || r.EmployeeId==employeeId).OrderByDescending(c => c.Id);


            //var requstionStatus = _requsitionStatusManager.GetAll();

            List<MyRequsitionListViewModel> requisitionViewList = new List<MyRequsitionListViewModel>();
            foreach (var requisition in allRequisitions)
            {
                var requisitionVM = new MyRequsitionListViewModel();
                requisitionVM.Id = requisition.Id;
                requisitionVM.RequestType = requisition.RequestType;

                requisitionVM.RequestedBy = requisition.RequestedBy == employeeId ? "Me" : GetEmployeeName(requisition.RequestedBy);
                requisitionVM.EmployeeName = requisition.EmployeeId == employeeId ? "Me" : GetEmployeeName(requisition.EmployeeId);
                
                
                requisitionVM.Form = requisition.Form;
                requisitionVM.To = requisition.To;
                requisitionVM.Description = requisition.Description;
                requisitionVM.JourneyStart = requisition.JourneyStart;
                requisitionVM.JouneyEnd = requisition.JouneyEnd;
                
                requisitionVM.Status = requisition.Status;
                requisitionViewList.Add(requisitionVM);
            }
            return requisitionViewList;
        }

        private string GetEmployeeName(int? employeeId)
        {

            var employee = _employeeManager.Get(c => c.Id== employeeId).Select(c=>c.Name).FirstOrDefault();
            return employee;
        }

        private int GetEmployeeId()
        {
            ApplicationUser user =
                System.Web.HttpContext.Current.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>()
                    .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());

            var employee = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false && c.UserId == user.Id);
            var employeeId = employee.Select(e => e.Id).FirstOrDefault();
            return employeeId;
        }
        public List<SelectListItem> GetRequisitionTypes()
        {
            return new List<SelectListItem>(){
                    new SelectListItem{Value = "1",Text = "Official"},
                    new SelectListItem{Value = "2",Text = "Personal"}
                    };
        }

        public ActionResult Print(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var requsition = _requisitionManager.GetById((int) id);
            var emplt = requsition.Employee.Name;
            var empltNo = requsition.Employee.ContactNo;
            var dept = requsition.Employee.Department.Name;
            var des = requsition.Employee.Designation.Name;
            var startdate = requsition.JourneyStart.ToString("f");
            var enddate = requsition.JouneyEnd.ToString("f");

            List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            var requsitionVM = new RequsitionViewModel();
            requsitionVM.DepartmentName = dept;
            requsitionVM.EmployeeName = emplt;
            requsitionVM.DesignationName = des;
            requsitionVM.EmployeeNo = empltNo;
            requsitionVM.Form = requsition.Form;
            requsitionVM.To = requsition.To;
            requsitionVM.Description = requsition.Description;
            requsitionVM.StartTime = startdate;
            requsitionVM.EndTime = enddate;
            requsitionVM.JouneyEnd = requsition.JouneyEnd;
            requsitionVM.RequsitionNumber = requsition.RequsitionNumber;
            requsitionViewModels.Add(requsitionVM);

            string reportpath = Request.MapPath(Request.ApplicationPath) + @"Report\RequsitionDetails\RequsitionDetailsRDLC.rdlc";
            var reportViewer = new ReportViewer()
            {
                KeepSessionAlive = true,
                SizeToReportContent = true,
                Width = Unit.Percentage(100),
                ProcessingMode = ProcessingMode.Local
            };
            reportViewer.LocalReport.ReportPath = reportpath;
            ReportDataSource rds = new ReportDataSource("DataSet1", requsitionViewModels);
            reportViewer.LocalReport.DataSources.Add(rds);
            ViewBag.ReportViewer = reportViewer;
            return View(requsitionViewModels);
        }

    }
}
