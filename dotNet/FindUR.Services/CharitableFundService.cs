using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Models.Requests.CharitableFunds;
using Sabio.Models.Domain.CharitableFunds;
using System.Data;
using Sabio.Data;

namespace Sabio.Services
{
    public class CharitableFundService : ICharitableFundService
    {
        IDataProvider _data = null;

        public CharitableFundService(IDataProvider data)
        {
            _data = data;
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[CharitableFunds_Delete_ById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    col.AddWithValue("@Id", id);

                },
                returnParameters: null);
        }

        public void Update(CharitableFundUpdateRequest model, int userId)
        {
            string procName = "[dbo].[CharitableFunds_Update]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@Id", model.Id);
                    col.AddWithValue("@CreatedBy", userId);
                },
                returnParameters: null);
        }

        public int Add(CharitableFundAddRequest model, int userId)
        {
            int id = 0;

            string procName = "[dbo].[CharitableFunds_Insert]";
            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {
                    AddCommonParams(model, col);
                    col.AddWithValue("@CreatedBy", userId);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);

                    idOut.Direction = ParameterDirection.Output;

                    col.Add(idOut);
                },
                returnParameters: delegate (SqlParameterCollection returnColllection)
                {
                    Object oId = returnColllection["@Id"].Value;

                    int.TryParse(oId.ToString(), out id);

                    Console.WriteLine("");
                });

            return id;

        }

        public List<CharitableFund> GetAll()
        {
            List<CharitableFund> charitableFundList = null;

            string procName = "[dbo].[CharitableFunds_Select_All]";

            _data.ExecuteCmd(procName, inputParamMapper: null
            , singleRecordMapper: delegate (IDataReader reader, short set)
            {
                CharitableFund aCharitableFund = MapSignleCharitableFund(reader);

                if (charitableFundList == null)
                {
                    charitableFundList = new List<CharitableFund>();
                }

                charitableFundList.Add(aCharitableFund);
            });
            return charitableFundList;
        }

        public CharitableFund Get(int id) 
        { 
            string procName = "[dbo].[CharitableFunds_Select_ById]"; 
             
            CharitableFund charitableFund = null; 
             
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection) 
            { 
                 
                paramCollection.AddWithValue("@Id", id); 
                 
            }, delegate (IDataReader reader, short set)  
            { 
                charitableFund = MapSignleCharitableFund(reader); 
            } 
            ); 
            return charitableFund; 
        } 

        private static void AddCommonParams(CharitableFundAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Name", model.Name);  
            col.AddWithValue("@Description", model.Description);  
            col.AddWithValue("@Url", model.Url); 
             
        } 

        private static CharitableFund MapSignleCharitableFund(IDataReader reader)
        {
            CharitableFund aCharitableFund = new CharitableFund();

            int startingIndex = 0;

            aCharitableFund.Id = reader.GetSafeInt32(startingIndex++);
            aCharitableFund.Name = reader.GetSafeString(startingIndex++);
            aCharitableFund.Description = reader.GetSafeString(startingIndex++);
            aCharitableFund.Url = reader.GetSafeString(startingIndex++);
            aCharitableFund.DateAdded = reader.GetSafeDateTime(startingIndex++);
            aCharitableFund.DateModified = reader.GetSafeDateTime(startingIndex++);
            aCharitableFund.CreatedBy = reader.GetSafeInt32(startingIndex++);

            return aCharitableFund;
        }

    }
}
