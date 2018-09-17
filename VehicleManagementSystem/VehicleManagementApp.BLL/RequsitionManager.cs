using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    public class RequsitionManager:Manager<Requsition>,IRequsitionManager
    {
        private IRequsitionRepository _requsition;
        public RequsitionManager() : base(new RequsistionRepository())
        {
            _requsition = (IRequsitionRepository)base.BaseRepository;
        }

        public RequsitionManager(IRequsitionRepository requsition):base(requsition)
        {
            _requsition = requsition;
        }

        public ICollection<Requsition> GetAllBySeen(string Status)
        {
            var requsition = _requsition.Get(c=>c.Status == Status);
            return requsition;
        }

        public ICollection<Requsition> GetAllByNull(string Status)
        {
            var requsition = _requsition.Get(c => c.Status == Status);
            return requsition;
        }
    }
}
