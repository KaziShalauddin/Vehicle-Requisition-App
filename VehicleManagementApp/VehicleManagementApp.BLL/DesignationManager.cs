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

namespace VehicleManagementApp.BLL
{
    public class DesignationManager:Manager<Designation>, IDesignationManager
    {
        private IDesignationRepository designationRepository;

        public DesignationManager():base(new DesignationRepository())
        {
            this.designationRepository = (DesignationRepository)base.BaseRepository;
        }

        public DesignationManager(IDesignationRepository Repository) : base(Repository)
        {
            this.designationRepository = Repository;
        }

        public bool IsNameAlreadyExist(string name)
        {
            var designation = designationRepository.Get(c => c.Name.ToLower().Equals(name.ToLower()));
            if (designation.Count > 0)
            {
                return false;
            }else
            {
                return true;
            }
        }

        public bool IsNameUnique(string name, int departmentId)
        {
            var searchDesignation = designationRepository.Get(c => c.Name == name && c.DepartmentId == departmentId);
            if (searchDesignation.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }


        }

        //public bool IsNameAlreadyExist(string name, int departmentId)
        //{
        //    var designation = designationRepository.Get(c => c.Name.ToLower().Equals(name.ToLower()) && c.DepartmentId == departmentId);
        //    if (designation.Count > 0)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }

        //    //if (context.Entities.Any(e => e.FirstColumn == value1
        //    //               && e.SecondColumn == value2))
        //    //{
        //    //    // deal with duplicate values here.
        //    //}

        //}
    }
}
