using System;
namespace Sabio.Models.Domain.HorseProfiles
{
    public class BaseHorseProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Age { get; set; }
        public bool IsFemale { get; set; }
        public string Color { get; set; }
        public decimal Weight { get; set; }
        public string PrimaryImageUrl { get; set; }
        public bool HasConsent { get; set; }
        public double Distance { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
