using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public class HomeController : Controller
    {
    
        private IRequsitionManager _requisitionManager;
        private IEmployeeManager _employeeManager;
        private ICommentManager commentManager;
        public HomeController(IRequsitionManager requisition, IEmployeeManager employee, ICommentManager comment)
        {
            this._employeeManager = employee;
            this._requisitionManager = requisition;
          
            this.commentManager = comment;

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public FileContentResult UserPhotos()
        {
            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    string fileName = HttpContext.Server.MapPath(@"~/img/noImg.png");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");

                }
                // to get the user details to load user Image
                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (userImage.UserPhoto == null)
                {
                    return GetNotRegisteredUserPhotos();
                }
                return new FileContentResult(userImage.UserPhoto, "image/jpeg");
            }

            return GetNotRegisteredUserPhotos();

        }

        private FileContentResult GetNotRegisteredUserPhotos()
        {
            string fileName = HttpContext.Server.MapPath(@"~/img/noImg.png");

            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(fileName);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            return File(imageData, "image/png");
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
        [Authorize]
        public ActionResult Dashboard()
        {
            var userEmployeeId = GetEmployeeId();
            ViewBag.UserEmployeeId = userEmployeeId;


            var requsition = _requisitionManager.GetAll().OrderByDescending(c => c.Id);



            var comments = commentManager.Get(c => c.IsReceiverSeen == false).Where(c => c.ReceiverEmployeeId == userEmployeeId);

            var commentsWithRequisition = from r in requsition
                                          join c in comments on r.Id equals c.RequsitionId
                                          select new
                                          {
                                              r.Id,
                                              r.Status,
                                              c.SenderEmployee,
                                              c.SenderEmployeeId,
                                              c.ReceiverEmployeeId,
                                              c.ReceiverSeenTime,
                                              c.IsReceiverSeen
                                          };
            List<CountCommentViewModel> commentsList = new List<CountCommentViewModel>();
            foreach (var item in commentsWithRequisition)
            {
                var comment = new CountCommentViewModel();
                comment.Id = item.Id;
                comment.ReceiverEmployeeId = item.ReceiverEmployeeId;
                comment.Status = item.Status;
                commentsList.Add(comment);
            }
            if (commentsList.Count(c => c.Status == null) == 0)
            {
                ViewBag.NewRequisitionComments = 0;
            }
            if (commentsList.Count(c => c.Status == null) > 0)
            {
                ViewBag.NewRequisitionComments = commentsList.Count(c => c.Status == null);
            }
            if (commentsList.Count(c => c.Status == "Assign") == 0)
            {
                ViewBag.AssignRequisitionComments = 0;
            }
            if (commentsList.Count(c => c.Status == "Assign") > 0)
            {
                ViewBag.AssignRequisitionComments = commentsList.Count(c => c.Status == "Assign");
            }
            if (commentsList.Count(c => c.Status == "Hold") == 0)
            {
                ViewBag.HoldRequisitionComments = 0;
            }
            if (commentsList.Count(c => c.Status == "Hold") > 0)
            {
                ViewBag.HoldRequisitionComments = commentsList.Count(c => c.Status == "Hold");
            }

            return View();
        }

        public List<SelectListItem> GetRequisitionTypes()
        {
            return new List<SelectListItem>(){
                new SelectListItem{Value = "1",Text = "Official"},
                new SelectListItem{Value = "2",Text = "Personal"}
            };
        }
        private string GetEmployeeName(int? employeeId)
        {

            var employee = _employeeManager.Get(c => c.Id == employeeId).Select(c => c.Name).FirstOrDefault();
            return employee;
        }
        private List<MyRequsitionListViewModel> MyRequisitionListView()
        {
            var employeeId = GetEmployeeId();

            var allRequisitions = _requisitionManager.Get(r => r.RequestedBy == employeeId || r.EmployeeId == employeeId && r.Status == null).OrderByDescending(c => c.Id);
            

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
        public ActionResult NewList()
        {
            var userEmployeeId = GetEmployeeId();
            ViewBag.UserEmployeeId = userEmployeeId;
            //var requsition = _requisitionManager.Get(c => c.Status == null).Where(c => c.EmployeeId == userEmployeeId);


            MyRequsitionCreateViewModel allRequsitions = new MyRequsitionCreateViewModel();
            allRequsitions.RequestTypes = GetRequisitionTypes();
            var employees = _employeeManager.Get(c => c.IsDriver == false && c.IsDeleted == false && c.Id != userEmployeeId);
            ViewBag.Employees = employees.ToList();

            var requsitionViewList = MyRequisitionListView();
            allRequsitions.RequsitionViewModels = requsitionViewList;
            return View(allRequsitions);

            //List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            //foreach (var data in requsition)
            //{
            //    var requsitionVM = new RequsitionViewModel()
            //    {
            //        Id = data.Id,
            //        Form = data.Form,
            //        To = data.To,
            //        RequsitionNumber = data.RequsitionNumber,
            //        Description = data.Description,
            //        JourneyStart = data.JourneyStart,
            //        JouneyEnd = data.JouneyEnd,
            //       Employee = data.Employee
            //    };
            //    requsitionViewModels.Add(requsitionVM);
            //}
            //return View(requsitionViewModels);
        }

        public ActionResult MyRequestList()
        {
            var requsitionViewList = MyRequisitionListView();
            return PartialView("_MyRequisitionListPartial", requsitionViewList);
        }
        public string AutoNumber()
        {
            string year = DateTime.Now.Year.ToString();
            string month;
            if (DateTime.Now.Month < 10)
            {
                month = "0" + DateTime.Now.Month;
            }
            else
            {
                month = DateTime.Now.Month.ToString();
            }

            int day = DateTime.Now.Day;
            string time = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string second = DateTime.Now.Second.ToString();

            string yearMonth = "RQ-" + second + minute + time + day + month + year;
            return yearMonth;
        }

        public ActionResult MyJsonCreate(MyRequsitionCreateViewModel requisitionVm)
        {
            //newDateTime = date.Date + time.TimeOfDay;

            List<MyRequsitionListViewModel> requsitionViewList;
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

                requsitionViewList = MyRequisitionListView();
                return PartialView("_MyRequisitionListPartial", requsitionViewList);
            }
            TempData["msg"] = "Requisition not Send !";
            requsitionViewList = MyRequisitionListView();
            return PartialView("_MyRequisitionListPartial", requsitionViewList);

        }
        public ActionResult HoldList()
        {
            var userEmployeeId = GetEmployeeId();
            ViewBag.UserEmployeeId = userEmployeeId;
            var requsition = _requisitionManager.Get(c => c.Status == "Hold").Where(c => c.EmployeeId == userEmployeeId); ;
           
            List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            foreach (var data in requsition)
            {
                var requsitionVM = new RequsitionViewModel()
                {
                    Id = data.Id,
                    Form = data.Form,
                    To = data.To,
                    RequsitionNumber = data.RequsitionNumber,
                    Description = data.Description,
                    JourneyStart = data.JourneyStart,
                    JouneyEnd = data.JouneyEnd,
                    Employee = data.Employee
                };
                requsitionViewModels.Add(requsitionVM);
            }
            return View(requsitionViewModels);
        }

        public ActionResult AssignedList()
        {
            var userEmployeeId = GetEmployeeId();
            ViewBag.UserEmployeeId = userEmployeeId;
            var searchingValue =
                _requisitionManager.Get(c => c.Status == "Assign" && c.IsDeleted == false).Where(c => c.EmployeeId == userEmployeeId).OrderByDescending(c => c.Id);

            List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            foreach (var item in searchingValue)
            {
                var requisitionVM = new RequsitionViewModel();
                requisitionVM.Id = item.Id;
                requisitionVM.Employee = item.Employee;
                requisitionVM.Form = item.Form;
                requisitionVM.To = item.To;
                requisitionVM.StartTime = item.JourneyStart.ToString("f");
                requisitionVM.EndTime = item.JouneyEnd.ToString("f");
                requisitionVM.RequsitionNumber = item.RequsitionNumber;
                requisitionVM.Description = item.Description;
                requisitionVM.JourneyStart = item.JourneyStart;
                requisitionVM.JouneyEnd = item.JouneyEnd;
                requsitionViewModels.Add(requisitionVM);
            }
            return View(requsitionViewModels);
        }

        public ActionResult CompleteRequsition()
        {
           
            var userEmployeeId = GetEmployeeId();
            ViewBag.UserEmployeeId = userEmployeeId;

            var searchingValue = _requisitionManager.Get(c => c.Status == "Complete" && c.IsDeleted == false).Where(c => c.EmployeeId == userEmployeeId).OrderByDescending(c => c.JourneyStart); ;
            string todays = DateTime.Today.ToShortDateString();
            var todayRequsition = searchingValue.Where(c => c.JouneyEnd.ToShortDateString() == todays);

            FilteringSearchViewModel filteringSearchViewModel = new FilteringSearchViewModel();

            List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            foreach (var item in todayRequsition)
            {
                var requisitionVM = new RequsitionViewModel();
                requisitionVM.Id = item.Id;
                requisitionVM.Employee = item.Employee;
                requisitionVM.Form = item.Form;
                requisitionVM.To = item.To;
                requisitionVM.Description = item.Description;
                requisitionVM.JourneyStart = item.JourneyStart;
                requisitionVM.JouneyEnd = item.JouneyEnd;
                requsitionViewModels.Add(requisitionVM);
            }
            filteringSearchViewModel.RequsitionViewModels = requsitionViewModels;

            //var designation = designationManager.GetAll();
            //filteringSearchViewModel.Designations = designation;

            return View(filteringSearchViewModel);
        }

        [HttpPost]
        public ActionResult SearchCompleteRequsition(FilteringSearchViewModel filteringSearchViewModels)
        {
            var userEmployeeId = GetEmployeeId();
            ViewBag.UserEmployeeId = userEmployeeId;
            var startTime = filteringSearchViewModels.Startdate;
            var endTime = filteringSearchViewModels.EndDate;

            var searchingValue = _requisitionManager.Get(c => c.Status == "Complete" && c.IsDeleted == false).Where(r => r.RequestedBy == userEmployeeId || r.EmployeeId == userEmployeeId);
            var completeRequsitionList = searchingValue.Where(c => c.JourneyStart >= startTime && c.JouneyEnd <= endTime);

            List<RequsitionViewModel> requsitionViewModels = new List<RequsitionViewModel>();
            foreach (var item in completeRequsitionList)
            {
                var requisitionVM = new RequsitionViewModel();
                requisitionVM.Id = item.Id;
                requisitionVM.Employee = item.Employee;
                requisitionVM.Form = item.Form;
                requisitionVM.To = item.To;
                requisitionVM.Description = item.Description;
                requisitionVM.JourneyStart = item.JourneyStart;
                requisitionVM.JouneyEnd = item.JouneyEnd;
                requsitionViewModels.Add(requisitionVM);
            }

            filteringSearchViewModels.RequsitionViewModels = requsitionViewModels;
            return PartialView("_CompleteRequsitionPartial", requsitionViewModels);
        }

    }
}