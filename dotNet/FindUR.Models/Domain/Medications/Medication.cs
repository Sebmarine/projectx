using System;
namespace Sabio.Models.Domain.Medications
{
    public class Medication
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public int Dosage { get; set; }
        public string DosageUnit { get; set; }
        public int NumberDoses { get; set; }
        public string Frequency { get; set; }
        public DateTime StartDate { get; set; }
    }

}
