using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Models.Models;
using VehicleManagementApp.Models.ReportViewModel;

namespace VehicleManagementApp.BLL.Contracts
{
    public interface IManagerManager:IManager<Manager>
    {
        ICollection<RequsitionAssignReportViewModel> RequsitionAssignReportViewModels(
            RequsitionAssignViewModel requsitionAssignViewModel);
    }
}
