using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.BLL.Base;
using VehicleManagementApp.BLL.Contracts;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Repository;
using VehicleManagementApp.Repository.Contracts;
using VehicleManagementApp.Repository.Repository;

namespace VehicleManagementApp.BLL
{
    public class CommentManager:Manager<Comment>,ICommentManager
    {
        private ICommentRepository commentRepository;

        public CommentManager():base(new CommentRepository())
        {
            this.commentRepository = (CommentRepository)base.BaseRepository;
        }

        public CommentManager(ICommentRepository commentRepository) : base(commentRepository)
        {
            this.commentRepository = commentRepository;
        }
        public IEnumerable<Comment> GetCommentsByRequisition(int requisitionId)
        {
            return commentRepository.GetCommentsByRequisition(requisitionId);
        }
    }
}
