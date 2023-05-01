using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain;

namespace Sabio.Models.Domain.Schedules
{
    public class ScheduleAvailability
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public LookUp DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}

