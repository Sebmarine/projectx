using Sabio.Models;
using Sabio.Models.Requests.Schedules;
using Sabio.Models.Domain.Schedules;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IScheduleService
    {
        int Add(ScheduleAddRequest model, int userId);
        Schedule Get(int id);
        Paged<Schedule> Pagination(int pageIndex, int pageSize, int userId);
        void Update(ScheduleUpdateRequest model, int userId);
        void Delete(int id);

        #region -- Availability

        ScheduleAvailability GetAvailabilityById(int id);
        ScheduleAvailabilityV2 GetAvailabilityByIdV2(int id);
        List<ScheduleAvailabilityV2> GetAllAvailability();
        List<ScheduleAvailabilityV2> GetAllAvailabilityByVet(int vetProfileId);
        List<ScheduleAvailability> GetAvailabilityByScheduleId(int scheduleId);
        List<ScheduleAvailability> GetAvailabilityByCreatedBy(int createdBy);
        public int AddAvailability(ScheduleAvailabilityAddRequest model, int createdBy);
        public int AddAvailabilityV2(ScheduleAvailabilityAddRequestV2 model, int createdBy);
        public void UpdateAvailability(ScheduleAvailabilityUpdateRequest model, int modifiedBy);
        public void UpdateAvailabilityV2(ScheduleAvailabilityUpdateRequestV2 model, int modifiedBy);
        public void DeleteAvailabilityById(int id);
        public void DeleteAvailabilityByIdV2(int id);

        #endregion
    }
}