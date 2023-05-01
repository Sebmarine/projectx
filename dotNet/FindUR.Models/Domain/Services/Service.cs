using System;

namespace Sabio.Models.Domain.Services
{
    public class Service
    {
        public int Id { get; set; }
        public ServiceType ServiceType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public string ServiceCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public ServiceUser CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}