using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain;

namespace Sabio.Models.Domain.ExternalLinks
{
    public class ExternalLink
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public LookUp UrlType { get; set; }
        public string Url { get; set; }
        public int EntityId { get; set; }
        public LookUp EntityType { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

    }
}
