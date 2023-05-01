using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain;

namespace Sabio.Models.Domain.Schedules
{
    public class ScheduleAvailabilityV2
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public LookUp DayOfWeek { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Boolean IsBooked { get; set; }
    }
}

