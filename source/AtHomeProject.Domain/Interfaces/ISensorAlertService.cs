using System.Collections.Generic;
using System.Threading.Tasks;
using AtHomeProject.Domain.Models;

namespace AtHomeProject.Domain.Interfaces
{
    public interface ISensorAlertService
    {
        Task ProcessSensorsInputAsync(List<SensorInputModel> models);
    }
}
