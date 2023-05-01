using Sabio.Models.Domain.File;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Domain.Medications;
using Sabio.Models.Domain.Users;
using System.Collections.Generic;

namespace Sabio.Models.Domain.HorseProfiles
{
    public class HorseProfile : BaseHorseProfile
    {
        public LookUp BreedTypeId { get; set; }
        public User OwnerInfo { get; set; }
        public Locations.Location HorseLocation { get; set; }
        public List<User> UserHorses { get; set; }
        public List<Medication> HorseMedications { get; set; }
        public List<FileModel> HorseFiles { get; set; }
    }
}
