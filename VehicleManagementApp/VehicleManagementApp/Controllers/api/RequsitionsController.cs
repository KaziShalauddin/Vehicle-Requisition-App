using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Repository.DatabaseContext;

namespace VehicleManagementApp.Controllers.api
{
    public class RequsitionsController : ApiController
    {
        private VehicleDatabaseContext db = new VehicleDatabaseContext();

        // GET: api/Requsitions
        public IQueryable<Requsition> GetRequsitions()
        {
            return db.Requsitions;
        }

        // GET: api/Requsitions/5
        [ResponseType(typeof(Requsition))]
        public IHttpActionResult GetRequsition(int id)
        {
            Requsition requsition = db.Requsitions.Find(id);
            if (requsition == null)
            {
                return NotFound();
            }

            return Ok(requsition);
        }

        // PUT: api/Requsitions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutRequsition(int id, Requsition requsition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != requsition.Id)
            {
                return BadRequest();
            }

            db.Entry(requsition).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequsitionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Requsitions
        [ResponseType(typeof(Requsition))]
        public IHttpActionResult PostRequsition(Requsition requsition)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Requsitions.Add(requsition);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = requsition.Id }, requsition);
        }

        // DELETE: api/Requsitions/5
        [ResponseType(typeof(Requsition))]
        public IHttpActionResult DeleteRequsition(int id)
        {
            Requsition requsition = db.Requsitions.Find(id);
            if (requsition == null)
            {
                return NotFound();
            }

            db.Requsitions.Remove(requsition);
            db.SaveChanges();

            return Ok(requsition);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RequsitionExists(int id)
        {
            return db.Requsitions.Count(e => e.Id == id) > 0;
        }
    }
}