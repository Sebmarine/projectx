using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.AdminData;

namespace Sabio.Services.Interfaces
{
    public interface IAdminService
    {
        AdminData GetAllByDateRange(int dateRange);
        AdminData GetAllByDates(DateTime startDate, DateTime endDate);
    }
}
