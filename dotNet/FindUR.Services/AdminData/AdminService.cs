using Sabio.Data.Providers;
using Sabio.Services.Interfaces;
using Sabio.Models.Domain.AdminData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Components.Forms;
using System.Data;
using Sabio.Data;

namespace Sabio.Services
{

    public class AdminService : IAdminService
    {
        private IDataProvider _data = null;
       

        public AdminService(IDataProvider data)
        {
            _data = data;
           
        }


        public AdminData GetAllByDateRange(int dateRange)
        {
            AdminData singleItem = null;
            int startingIndex = 0;
            string procName = "[dbo].[AdminData_GetTotalCountByRange]";
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@DateRange", dateRange);
                },
                delegate (IDataReader reader, short set)
                {
                    singleItem = MapAdminData(reader, ref startingIndex);
                }
                );
            return singleItem;

        }

        public AdminData GetAllByDates(DateTime startDate, DateTime endDate )
        {
            AdminData singleItem = null;
            int startingIndex = 0;
            string procName = "[dbo].[AdminData_GetTotalCount]";
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@StartDate", startDate);
                paramCollection.AddWithValue("@EndDate", endDate);
            },
                delegate (IDataReader reader, short set)
                {
                    singleItem = MapAdminData(reader, ref startingIndex);
                }
                );
            return singleItem;

        }

        private AdminData MapAdminData(IDataReader reader, ref int startingIndex)
        {
            AdminData aData = new AdminData();
                       
            aData.Appointments = reader.GetSafeInt32(startingIndex++);
            aData.Users = reader.GetSafeInt32(startingIndex++);
            aData.Vets = reader.GetSafeInt32(startingIndex++);
            aData.Patients = reader.GetSafeInt32(startingIndex++);

            return aData;
        }
    }

}