using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Schedules
{
    public class ScheduleAvailabilityAddRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ScheduleId { get; set; }
        [Required]
        [Range(1, 7)]
        public int DayOfWeek { get; set; }
        [Required]
        public string StartTime { get; set; }
        [Required]
        public string EndTime { get; set; }
    }
}
