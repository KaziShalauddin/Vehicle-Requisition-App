using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Repository.Contracts;
using VehicleManagementApp.Repository.Repository;

namespace VehicleManagementApp.Repository
{
    public class NewCommentRepository:DeletableRepository<Comment>,ICommentRepository
    {
        public IEnumerable<Comment> GetCommentsByRequisition(int requisitionId)
        {
            return base.Context.Comments.Where(x => x.RequsitionId == requisitionId).ToList();
        }
    }
}
