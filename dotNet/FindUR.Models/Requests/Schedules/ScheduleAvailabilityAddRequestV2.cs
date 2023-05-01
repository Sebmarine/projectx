using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Schedules
{
    public class ScheduleAvailabilityAddRequestV2
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ScheduleId { get; set; }
        [Required]
        [Range(1, 7)]
        public int DayOfWeek { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public Boolean IsBooked { get; set; }
    }
}
