using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Domain.File;

namespace Sabio.Models.Domain.Vendors
{
    public class FormVendor : SimpleVendor
    {
        public int PrimaryImageId { get; set; }

        public FileModel File { get; set; } 
    }
}
