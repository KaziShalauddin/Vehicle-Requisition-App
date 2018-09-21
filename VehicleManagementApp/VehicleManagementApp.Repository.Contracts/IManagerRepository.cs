using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Models.ReportViewModel;

namespace VehicleManagementApp.Repository.Contracts
{
    public interface IManagerRepository:IRepository<Manager>
    {
        ICollection<RequsitionAssignReportViewModel> RequsitionAssignReportViewModels(
            RequsitionAssignViewModel requsitionAssignViewModel);
    }
}
