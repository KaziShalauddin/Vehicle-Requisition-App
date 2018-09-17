using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.ViewModels;


namespace VehicleManagementApp.Controllers
{
    public class CommentController : Controller
    {
        private ICommentManager commentManager;

        public CommentController(ICommentManager comment)
        {
            this.commentManager = comment;
        }
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        // GET: Comment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Comment/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Comment/Create
        [HttpPost]
        public ActionResult Create(Comment comment)
        {
            try
            {
                //var userId = User.Identity.GetUserId();
                //Comment comment = new Comment();
                //comment.RequsitionId = RequsitionViewModel.Id;
                //comment.Comments = RequsitionViewModel.CommentViewModel.Comments;

                commentManager.Add(comment);
                return RedirectToAction("Details", "Requsition");

                
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Comment/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Comment/Delete/5
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
