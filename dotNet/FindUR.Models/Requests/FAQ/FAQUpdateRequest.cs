using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.FAQ
{
    public class FAQUpdateRequest: FAQAddRequest, IModelIdentifier
    {
        public int Id { get; set; }
    }
}
