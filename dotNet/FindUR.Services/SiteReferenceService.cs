using Sabio.Data.Providers;
using Sabio.Models.Requests.SiteReference;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class SiteReferenceService : ISiteReferenceService
    {
        IDataProvider _data = null;

        public SiteReferenceService(IDataProvider data)
        {
            _data = data;
        }

        public void Add(SiteReferenceAddRequest model)
        {
            string procName = "[dbo].[SiteReferences_Insert]";

            _data.ExecuteNonQuery(procName
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@UserId", model.UserId);
                    paramCollection.AddWithValue("@ReferenceTypeId", model.ReferenceTypeId);
                });

        }
    }
}
