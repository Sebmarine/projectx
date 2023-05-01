using Sabio.Data.Providers;
using Sabio.Models.Requests.Appointments;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Sabio.Services.Interfaces;
using Sabio.Models.Domain.Appointments;
using Sabio.Data;
using Newtonsoft.Json;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Domain.VetProfiles;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.ExternalLinks;
using Sabio.Models.Domain.ExternalLinks;

namespace Sabio.Services
{
    public class ExternalLinksService : IExternalLinksService
    {
        private IDataProvider _data = null;

        public ExternalLinksService(IDataProvider data)
        {
            _data = data;
        }

        public int Add(ExternalLinksAddRequest model, int userId)
        {
            int id = 0;
            DataTable urlBatchDataTable = MapUrlTypeToTable(model.Urls, userId);
            string procName = "[dbo].[ExternalLinks_InsertBatch]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@BatchExternalLinks", urlBatchDataTable);
                
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                collection.Add(idOut);
            },
            returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out id);
            });

            return id;
        }

        public void Update(ExternalLinksUpdateRequest request, int userId)
        {
            string procName = "[dbo].[ExternalLinks_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                AddCommonParams(request, collection, userId);
                
                collection.AddWithValue("@Id", request.Id);

            }, returnParameters: null);
        }
        
        public List<ExternalLink> Get(int userId)
        {
            List<ExternalLink> list = null;
            string procName = "[dbo].[ExternalLinks_Select_ByCreatedBy]";

            

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@UserId", userId);

            }, delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                ExternalLink anExternalLink = MapSingleExternalLink(reader, ref startingIndex);
                if (list == null)
                {
                   list = new List<ExternalLink>();
                }
                list.Add(anExternalLink);
            });
            
            return list;
        }
        
        public void Delete(int id, int userId)
        {
            string procName = "[dbo].[ExternalLinks_Delete_ById]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Id", id);
                collection.AddWithValue("@UserId", userId);
            }, returnParameters: null);
        }

        
        
        private ExternalLink MapSingleExternalLink(IDataReader reader, ref int startingIndex)
        {
            ExternalLink anExternalLink = new ExternalLink();
            anExternalLink.UrlType = new LookUp();
            anExternalLink.EntityType = new LookUp();

            anExternalLink.Id = reader.GetSafeInt32(startingIndex++);
            //anExternalLink.UserId = reader.GetSafeInt32(startingIndex++);
            anExternalLink.UrlType.Id = reader.GetSafeInt32(startingIndex++);
            anExternalLink.UrlType.Name = reader.GetSafeString(startingIndex++);
            anExternalLink.Url = reader.GetSafeString(startingIndex++);
            anExternalLink.EntityId = reader.GetSafeInt32(startingIndex++);
            anExternalLink.EntityType.Id = reader.GetSafeInt32(startingIndex++);
            anExternalLink.EntityType.Name = reader.GetSafeString(startingIndex++);
            anExternalLink.DateCreated = reader.GetSafeDateTime(startingIndex++);
            anExternalLink.DateModified = reader.GetSafeDateTime(startingIndex++);

            return anExternalLink;
        }
        

        private void AddCommonParams(ExternalLinksAddRequest request, SqlParameterCollection collection, int userId)
        {   
            collection.AddWithValue("@UserId", userId);
        }

        private DataTable MapUrlTypeToTable(List<ExternalLinkUrlAddRequest> externalLinkUrl, int userId)
        {
            DataTable table = new DataTable();
            table.Columns.Add("UserId", typeof(Int32));
            table.Columns.Add("UrlTypeId", typeof(Int32));
            table.Columns.Add("Url", typeof(string));
            table.Columns.Add("EntityId", typeof(Int32));
            table.Columns.Add("EntityTypeId", typeof(Int32));
            foreach (ExternalLinkUrlAddRequest singleUrl in externalLinkUrl)
            {
                DataRow row = table.NewRow();
                int startingIndex = 0;
                row.SetField(startingIndex++, userId);
                row.SetField(startingIndex++, singleUrl.UrlTypeId);
                row.SetField(startingIndex++, singleUrl.Url);
                row.SetField(startingIndex++, singleUrl.EntityId);
                row.SetField(startingIndex++, singleUrl.EntityTypeId);
                table.Rows.Add(row);
            }

            return table;
        }
    }
}
