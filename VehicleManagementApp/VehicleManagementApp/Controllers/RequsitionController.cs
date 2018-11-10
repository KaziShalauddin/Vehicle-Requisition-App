using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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

        private IDriverStatusManager driverStatusManager;
        private IVehicleStatusManager vehicleStatusManager;

        public RequsitionController(IRequsitionManager requisition, IEmployeeManager employee,  IManagerManager manager, IVehicleManager vehicle, ICommentManager comment, IDriverStatusManager driverStatus, IVehicleStatusManager vehicleStatus)
        {
            this._requisitionManager = requisition;
            this._employeeManager = employee;
            this.commentManager = comment;
            this._managerManager = manager;
            this.vehicleManager = vehicle;

            this.driverStatusManager = driverStatus;
            this.vehicleStatusManager = vehicleStatus;

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
            var userEmployeeId = GetEmployeeId();
            ViewBag.UserEmployeeId = userEmployeeId;
            EditRequisitionViewModel EditRequsition = new EditRequisitionViewModel();
            EditRequsition.Id = requisition.Id;
            EditRequsition.Form = requisition.Form;
            EditRequsition.To = requisition.To;
            EditRequsition.Description = requisition.Description;
            EditRequsition.JourneyStart = requisition.JourneyStart;
            EditRequsition.JourneyEnd = requisition.JouneyEnd;
            EditRequsition.EmployeeId = (int)requisition.EmployeeId;

            EditRequsition.RequestTypes = GetRequisitionTypes();
            EditRequsition.RequestType = requisition.RequestType;
            var employees = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false && c.Id != userEmployeeId);
            ViewBag.Employees = employees.ToList();

            return View(EditRequsition);
        }

        // POST: Requsition/Edit/5
        [HttpPost]
        public ActionResult Edit(EditRequisitionViewModel requisitionVm)
        {
            try
            {
               
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

                    DateTime journyStartDate = (DateTime)requisitionVm.JourneyStartDate;

                    var journeyStart = journyStartDate.Date + requisitionVm.JourneyStartTime.TimeOfDay;
                    DateTime jouneyEndDate = (DateTime)requisitionVm.JouneyEndDate;
                    var jouneyEnd = jouneyEndDate.Date + requisitionVm.JouneyEndTime.TimeOfDay;


                    Requsition requisition = _requisitionManager.GetById((int)requisitionVm.Id);
                    requisition.Form = requisitionVm.Form;
                    requisition.To = requisitionVm.To;
                    
                    requisition.Description = requisitionVm.Description;
                    requisition.JourneyStart = journeyStart;
                    requisition.JouneyEnd = jouneyEnd;
                    requisition.EmployeeId = requestForEmployeeId;

                    requisition.RequestedBy = GetEmployeeId();
                    requisition.RequestType = requisitionVm.RequestType;

                    bool isUpdated = _requisitionManager.Update(requisition);
                    if (isUpdated)
                    {
                        TempData["msg"] = "Requisition Updated Successfully";

                    }
                    else
                    {
                        TempData["msg"] = "Requisition Not Updated !";
                    }
                }
                return RedirectToAction("Dashboard","Home");
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
        [HttpGet]
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
        [HttpPost]
        public ActionResult MyRequisitionList(MyRequsitionCreateViewModel requisitionVm)
        {

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
                DateTime journyStartDate = (DateTime)requisitionVm.JourneyStartDate;

                var journeyStart = journyStartDate.Date + requisitionVm.JourneyStartTime.TimeOfDay;
                DateTime jouneyEndDate = (DateTime)requisitionVm.JouneyEndDate;
                var jouneyEnd = jouneyEndDate.Date + requisitionVm.JouneyEndTime.TimeOfDay;



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
                return RedirectToAction("MyRequisitionList");
            }
            requisitionVm.RequestTypes = GetRequisitionTypes();
            TempData["msg"] = "Requisition not Send !";
            return View(requisitionVm);


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
                DateTime journyStartDate = (DateTime)requisitionVm.JourneyStartDate;
                var journeyStart = journyStartDate.Date + requisitionVm.JourneyStartTime.TimeOfDay;

                DateTime jouneyEndDate = (DateTime)requisitionVm.JouneyEndDate;
                var jouneyEnd = jouneyEndDate.Date + requisitionVm.JouneyEndTime.TimeOfDay;
               
                

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

            var employee = _employeeManager.Get(c => c.IsDeleted == false && c.UserId == user.Id);
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

        [Authorize(Roles = "Controller,Employee,Operator,Driver")]
        public ActionResult Details_V2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requsition requisition = _requisitionManager.GetById((int)id);
            var userEmployeeId = GetEmployeeId();
            ViewBag.UserEmployeeId = userEmployeeId;
            var driverId=0;
            var vehicleId=0;
            switch (requisition.Status)
            {
                case "Assign":
                    driverId = driverStatusManager.Get(c => c.RequsitionId == id && c.Status == "Assign").Select(c => c.EmployeeId).FirstOrDefault();
                    vehicleId = vehicleStatusManager.Get(c => c.RequsitionId == id && c.Status == "Assign").Select(c => c.VehicleId).FirstOrDefault();
                    break;
                case "Complete":
                    driverId = driverStatusManager.Get(c => c.RequsitionId == id && c.Status == "Complete").Select(c => c.EmployeeId).FirstOrDefault();
                    vehicleId = vehicleStatusManager.Get(c => c.RequsitionId == id && c.Status == "Complete").Select(c => c.VehicleId).FirstOrDefault();
                    break;
                    
            }
            //if (requisition.Status == "Assign")
            //{
            //    driverId = driverStatusManager.Get(c => c.RequsitionId == id && c.Status== "Assign").Select(c => c.EmployeeId).FirstOrDefault();
            //    vehicleId = vehicleStatusManager.Get(c => c.RequsitionId == id && c.Status == "Assign").Select(c => c.VehicleId).FirstOrDefault();
            //}

            //if (requisition.Status == "Complete")
            //{
            //    driverId = driverStatusManager.Get(c => c.RequsitionId == id && c.Status == "Complete").Select(c => c.EmployeeId).FirstOrDefault();
            //    vehicleId = vehicleStatusManager.Get(c => c.RequsitionId == id && c.Status == "Complete").Select(c => c.VehicleId).FirstOrDefault();
            //}
           
            if (!User.IsInRole("Controller"))
            {
                if (requisition.EmployeeId != userEmployeeId && requisition.RequestedBy != userEmployeeId && driverId!= userEmployeeId)
                {
                    TempData["msg"] = "Sorry, you have no permission to access this type of data!";
                    return RedirectToAction("Dashboard","Home");

                }
            }
            AssignedListViewModel assignVm = new AssignedListViewModel
            {
                Requisition = requisition,
                Employee = _employeeManager.GetById((int)requisition.EmployeeId),
                Driver = _employeeManager.GetById(driverId),
                Vehicle = vehicleManager.GetById(vehicleId)
            };
            GetCommentViewModelForInsertComment(requisition, userEmployeeId, assignVm);

            //Collect the list of comment to display the list under comment
            GetCommentList(requisition, assignVm);
            return View(assignVm);
        }

        private void GetCommentViewModelForInsertComment(Requsition requisition, int userEmployeeId,AssignedListViewModel assignVm)
        {
            int? emplId = requisition.EmployeeId;
            string employeeNam = requisition.Employee.Name;
            var comment = new CommentViewModel
            {
                EmployeeId = (int)emplId,
                EmployeName = employeeNam,
                RequsitionId = requisition.Id,
                SenderEmployeeId = userEmployeeId,
                ReceiverEmployees = _employeeManager.Get(c => c.UserRole == "Controller")
            };
            assignVm.CommentViewModel = comment;
        }

        private void GetCommentList(Requsition requisition, AssignedListViewModel assignVm)
        {
            List<CommentViewModel> commentListViewModel = new List<CommentViewModel>();
            var commentListView = commentManager.GetCommentsByRequisition(requisition.Id).Where(c=>c.ReceiverEmployeeId==GetEmployeeId()||c.SenderEmployeeId==GetEmployeeId());
            foreach (var item in commentListView.ToList())
            {
                var cmnt = new CommentViewModel
                {
                    Id = item.Id,
                    RequsitionId = item.RequsitionId,
                    EmployeeId = item.EmployeeId,
                    Comments = item.Comments,
                    UserName = item.UserName,
                    CommentTime = item.CommentTime,
                    IsReceiverSeen = item.IsReceiverSeen,
                    ReceiverSeenTime = item.ReceiverSeenTime,
                    SenderEmployee = item.SenderEmployee,
                    SenderEmployeeId = (int) item.SenderEmployeeId,
                    ReceiverEmployee = item.ReceiverEmployee,
                    ReceiverEmployeeId = (int) item.ReceiverEmployeeId
                };
                commentListViewModel.Add(cmnt);
            }
            assignVm.CommentViewModels = commentListViewModel;
        }


        [Authorize(Roles = "Controller,Employee,Operator,Driver")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComment_V2(CommentViewModel commentViewModel)
        {

            //var userId = User.Identity.GetUserId();
            var userName = User.Identity.Name;
            if (commentViewModel.ReceiverEmployeeId == null)
            {
                var requisition = _requisitionManager.GetById(commentViewModel.RequsitionId);
                

                if (requisition.Status == "Assign")
                {
                    var driverId = driverStatusManager.Get(c => c.RequsitionId == commentViewModel.RequsitionId).Select(c => c.EmployeeId).FirstOrDefault();
                    if (User.IsInRole("Controller"))
                    {
                        if (commentViewModel.ReceiverForControllerComment == "Employee")
                        {
                            commentViewModel.ReceiverEmployeeId = requisition.EmployeeId;
                        }
                        if (commentViewModel.ReceiverForControllerComment == "Driver")
                        {
                            commentViewModel.ReceiverEmployeeId = driverId;
                        }
                        else
                        {
                            commentViewModel.ReceiverEmployeeId = requisition.EmployeeId;
                        }

                    }
                    if (!User.IsInRole("Controller"))
                    {
                        if (driverId==GetEmployeeId())
                        {
                            commentViewModel.ReceiverEmployeeId = requisition.EmployeeId;
                        }
                        else
                        {
                            commentViewModel.ReceiverEmployeeId = driverId;
                        }

                    }
                    //if (User.IsInRole("Employee"))
                    //{
                    //    commentViewModel.ReceiverEmployeeId = commentViewModel.ReceiverForControllerComment == "Driver" ? driverId : GetEmployeeId();
                    //}
                    //if (User.IsInRole("Driver"))
                    //{
                    //    commentViewModel.ReceiverEmployeeId = commentViewModel.ReceiverForControllerComment == "Employee" ? requisition.EmployeeId : driverId;
                    //}

                }
                if (requisition.Status == null)
                {
                   
                    if (User.IsInRole("Controller"))
                    {
                        if (commentViewModel.ReceiverForControllerComment == "Employee")
                        {
                            commentViewModel.ReceiverEmployeeId = requisition.EmployeeId;
                        }
                        else
                        {
                            commentViewModel.IsReceiverSeen = true;
                            commentViewModel.ReceiverEmployeeId = GetEmployeeId();
                        }

                    }
                    if (!User.IsInRole("Controller"))
                    {
                            commentViewModel.IsReceiverSeen = true;
                            commentViewModel.ReceiverEmployeeId = GetEmployeeId();
                        
                    }

                }

            }
          
            Comment comment = new Comment
            {
                IsReceiverSeen = commentViewModel.IsReceiverSeen,
                RequsitionId = commentViewModel.RequsitionId,
                Comments = commentViewModel.Comments,
                EmployeeId = commentViewModel.EmployeeId,
                SenderEmployeeId = commentViewModel.SenderEmployeeId,
                // SenderEmployee = _employeeManager.GetById(commentViewModel.SenderEmployeeId),
                ReceiverEmployeeId = commentViewModel.ReceiverEmployeeId,
                ReceiverSeenTime = DateTime.Now,
                //ReceiverEmployee = _employeeManager.GetById(commentViewModel.SenderEmployeeId),
                UserName = userName,
                CommentTime = DateTime.Now
            };
            bool isSaved = commentManager.Add(comment);

            List<CommentViewModel> commentListViewModel = new List<CommentViewModel>();

            if (isSaved)
            {
                var userEmployeeId = GetEmployeeId();
                ViewBag.UserEmployeeId = userEmployeeId;
                //Collect the list of comment to display the list under comment
                var commentListView = commentManager.GetCommentsByRequisition(commentViewModel.RequsitionId);

                foreach (var item in commentListView.ToList())
                {
                    var cmnt = new CommentViewModel();
                    //{

                    cmnt.Id = item.Id;
                    cmnt.RequsitionId = item.RequsitionId;
                    cmnt.EmployeeId = item.EmployeeId;
                    cmnt.Comments = item.Comments;
                    cmnt.UserName = item.UserName;
                    cmnt.CommentTime = item.CommentTime;
                    cmnt.IsReceiverSeen = item.IsReceiverSeen;
                    cmnt.ReceiverSeenTime = item.ReceiverSeenTime;
                    cmnt.SenderEmployee = item.SenderEmployee;
                    cmnt.SenderEmployeeId = (int) item.SenderEmployeeId;
                    cmnt.ReceiverEmployee = item.ReceiverEmployee;
                    cmnt.ReceiverEmployeeId = (int) item.ReceiverEmployeeId;
                    //};
                    commentListViewModel.Add(cmnt);

                }
                return PartialView("_CommentList", commentListViewModel);
            }
            return PartialView("_CommentList", commentListViewModel);
        }

        public RedirectToRouteResult CommentSeen(int? id)
        {
            var commentSeen = commentManager.GetById((int)id);
            var requisition = _requisitionManager.GetById(commentSeen.RequsitionId);
            commentSeen.ReceiverSeenTime = DateTime.Now;
            commentSeen.IsReceiverSeen = true;
            commentManager.Update(commentSeen);
            return RedirectToAction("Details_V2", new { id = requisition.Id });
        }
        public ActionResult AssignDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requsition requisition = _requisitionManager.GetById((int)id);
            var driverId = driverStatusManager.Get(c => c.RequsitionId == id).Select(c => c.EmployeeId).FirstOrDefault();
            var vehicleId = vehicleStatusManager.Get(c => c.RequsitionId == id).Select(c => c.VehicleId).FirstOrDefault();

            AssignedListViewModel assignVm = new AssignedListViewModel
            {
                Requisition = requisition,
                Employee = _employeeManager.GetById((int)requisition.EmployeeId),
                Driver = _employeeManager.GetById(driverId),
                Vehicle = vehicleManager.GetById(vehicleId)
            };
            return View(assignVm);
        }
        public ActionResult Print_V2(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Requsition requisition = _requisitionManager.GetById((int)id);
            var driverId = driverStatusManager.Get(c => c.RequsitionId == id).Select(c => c.EmployeeId).FirstOrDefault();
            var vehicleId = vehicleStatusManager.Get(c => c.RequsitionId == id).Select(c => c.VehicleId).FirstOrDefault();

            AssignedListViewModel assignVm = new AssignedListViewModel
            {
                Requisition = requisition,
                Employee = _employeeManager.GetById((int)requisition.EmployeeId),
                Driver = _employeeManager.GetById(driverId),
                Vehicle = vehicleManager.GetById(vehicleId)
            };

            List<ManagerViewModel> managerViewModels = new List<ManagerViewModel>();

            var managersVM = new ManagerViewModel
            {
                Id = assignVm.Id,
                EmployeeName = assignVm.Employee.Name,
                EmployeNumber = assignVm.Employee.ContactNo,
                JourneyEnd = assignVm.Requisition.JouneyEnd,
                To = assignVm.Requisition.To,
                Description = assignVm.Requisition.Description,
                JourneyStart = assignVm.Requisition.JourneyStart,
                DriverName = assignVm.Driver.Name,
                VehicleModel = assignVm.Vehicle.Name,
                Designation = assignVm.Employee.Designation.Name
            };

            managerViewModels.Add(managersVM);
            string reportpath = Request.MapPath(Request.ApplicationPath) + @"Report\AssignRequsition\RequsitionAssignRDLC.rdlc";
            var reportViewer = new ReportViewer()
            {
                KeepSessionAlive = true,
                SizeToReportContent = true,
                Width = Unit.Percentage(100),
                ProcessingMode = ProcessingMode.Local
            };
            reportViewer.LocalReport.ReportPath = reportpath;
            ReportDataSource rds = new ReportDataSource("AssignRequsition", managerViewModels);
            reportViewer.LocalReport.DataSources.Add(rds);
            ViewBag.ReportViewer = reportViewer;
            return View(managerViewModels);
        }

        public ActionResult AllUnseenComments()
        {
           
            List<CommentViewModel> commentListViewModel = new List<CommentViewModel>();
            var commentListView = commentManager.Get(c=>c.IsReceiverSeen==false).Where(c => c.ReceiverEmployeeId == GetEmployeeId());
            foreach (var item in commentListView.ToList())
            {
                var cmnt = new CommentViewModel
                {
                    Id = item.Id,
                    RequsitionId = item.RequsitionId,
                    Requsition = _requisitionManager.GetById(item.RequsitionId) ,
                    EmployeeId = item.EmployeeId,
                    Comments = item.Comments,
                    //UserName = item.UserName,
                    CommentTime = item.CommentTime,
                    //IsReceiverSeen = item.IsReceiverSeen,
                    //ReceiverSeenTime = item.ReceiverSeenTime,
                    SenderEmployee = item.SenderEmployee,
                    //SenderEmployeeId = (int)item.SenderEmployeeId,
                    //ReceiverEmployee = item.ReceiverEmployee,
                    //ReceiverEmployeeId = (int)item.ReceiverEmployeeId
                };
                commentListViewModel.Add(cmnt);
            }
            //assignVm.CommentViewModels = commentListViewModel;
            return View(commentListViewModel);
        }
    }
}
