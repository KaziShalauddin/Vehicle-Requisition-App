using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Models.ReportViewModel;
using VehicleManagementApp.Repository.Contracts;
using VehicleManagementApp.Repository.DatabaseContext;
using VehicleManagementApp.Repository.Repository;

namespace VehicleManagementApp.Repository
{
    public class ManagerRepository:DeletableRepository<Manager>,IManagerRepository
    {
        VehicleDatabaseContext sdb = new VehicleDatabaseContext();

        

        public ICollection<RequsitionAssignReportViewModel> RequsitionAssignReportViewModels(RequsitionAssignViewModel requsitionAssignViewModel)
        {
            var result = sdb.GetRequsitionAssignSummary();
            if (requsitionAssignViewModel.JourneyStart != null && requsitionAssignViewModel.JourneyStart != DateTime.MinValue)
            {
                result = result.Where(c => DbFunctions.TruncateTime(c.JourneyStart) >= requsitionAssignViewModel.JourneyStart);
            }
            if (requsitionAssignViewModel.JouneyEnd != null && requsitionAssignViewModel.JouneyEnd != DateTime.MinValue)
            {
                result = result.Where(c => DbFunctions.TruncateTime(c.JouneyEnd) <= requsitionAssignViewModel.JouneyEnd);
            }
            if (!string.IsNullOrEmpty(requsitionAssignViewModel.Form))
            {
                result = result.Where(c => c.Form.ToLower().Contains(requsitionAssignViewModel.Form.ToLower()));
            }
            if (!string.IsNullOrEmpty(requsitionAssignViewModel.Name))
            {
                result = result.Where(c => c.Name.ToLower().Contains(requsitionAssignViewModel.Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(requsitionAssignViewModel.DriverName))
            {
                result = result.Where(c => c.DriverName.ToLower().Contains(requsitionAssignViewModel.DriverName.ToLower()));
            }
            if (!string.IsNullOrEmpty(requsitionAssignViewModel.VehicleName))
            {
                result = result.Where(c => c.VehicleName.ToLower().Contains(requsitionAssignViewModel.VehicleName.ToLower()));
            }
            return result.ToList();
        }

    }
}
