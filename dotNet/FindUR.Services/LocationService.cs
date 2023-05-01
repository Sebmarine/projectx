using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Locations;
using Sabio.Models.Requests.Location;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class LocationService : ILocationService
    {
        IDataProvider _data = null;
        IAuthenticationService<int> _authService = null;

        public LocationService(IDataProvider data, IAuthenticationService<int> authService)
        {
            _data = data;
            _authService = authService;
        }

        public void Update(LocationUpdateRequest model, int currentUser)
        {
            string procName = "[dbo].[Locations_Update]";

            _data.ExecuteNonQuery(procName
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    AddCommonParams(model, paramCollection);
                    paramCollection.AddWithValue("@Id", model.Id);
                    paramCollection.AddWithValue("@ModifiedBy", currentUser);
                });
        }

        public void Delete(int id)
        {
            string procName = "[dbo].[Locations_Delete_ById]";

            _data.ExecuteNonQuery(procName
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);
                });
        }

        public List<Location> GetByGeo(int radius, double lat, double lng)
        {
            string procName = "[dbo].[Locations_Select_ByGeo]";
            List<Location> list = null;

            _data.ExecuteCmd(procName
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Radius", radius);
                    paramCollection.AddWithValue("@Lat", lat);
                    paramCollection.AddWithValue("@Lng", lng);
                }
                , singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;
                    Location aLocation = MapSingleLocation(reader, ref startingIndex);

                    if (list == null)
                    {
                        list = new List<Location>();
                    }

                    list.Add(aLocation);
                });

            return list;

        }
        public Paged<Location> GetByCreatedBy(int id, int pageIndex, int pageSize)
        {
            Paged<Location> pagedLocation = null;
            List<Location> locations = null;
            Location aLocation = null;
            int totalCount = 0;

            string procName = "[dbo].[Locations_Select_ByCreatedBy]";

            _data.ExecuteCmd(procName
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    paramCollection.AddWithValue("@Id", id);
                    paramCollection.AddWithValue("@PageSize", pageSize);
                    paramCollection.AddWithValue("@PageIndex", pageIndex);
                }
                , singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    aLocation = MapSingleLocation(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (locations == null)
                    {
                        locations = new List<Location>();
                    }

                    locations.Add(aLocation);
                });

            if (locations != null)
            {
                pagedLocation = new Paged<Location>(locations, pageIndex, pageSize, totalCount);
            }

            return pagedLocation;
        }


        public int Create(LocationAddRequest model, int currentUser)
        {
            int id = 0;
            string procName = "[dbo].[Locations_Insert]";

            _data.ExecuteNonQuery(procName
                , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                {
                    AddCommonParams(model, paramCollection);
                    paramCollection.AddWithValue("@CreatedBy", currentUser);

                    SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);

                    idOut.Direction = ParameterDirection.Output;
                    paramCollection.Add(idOut);
                }
                , returnParameters: delegate (SqlParameterCollection returnCollection)
                {
                    object oId = returnCollection["@Id"].Value;
                    int.TryParse(oId.ToString(), out id);
                });

            return id;
        }

        private void AddCommonParams(LocationAddRequest model, SqlParameterCollection paramCollection)
        {
            paramCollection.AddWithValue("@LocationTypeId", model.LocationTypeId);
            paramCollection.AddWithValue("@LineOne", model.LineOne);
            paramCollection.AddWithValue("@LineTwo", model.LineTwo);
            paramCollection.AddWithValue("@City", model.City);
            paramCollection.AddWithValue("@Zip", model.Zip);
            paramCollection.AddWithValue("@StateId", model.StateId);
            paramCollection.AddWithValue("@Latitude", model.Latitude);
            paramCollection.AddWithValue("@Longitude", model.Longitude);
        }

        public Location MapSingleLocation(IDataReader reader, ref int startingIndex)
        {
            Location aLocation = new Location();
            LookUp locationType = new LookUp();
            StateLookUp state = new StateLookUp();

            aLocation.Id = reader.GetSafeInt32(startingIndex++);

            locationType.Id = reader.GetSafeInt32(startingIndex++);
            locationType.Name = reader.GetSafeString(startingIndex++);
            aLocation.LocationType = locationType;

            aLocation.LineOne = reader.GetSafeString(startingIndex++);
            aLocation.LineTwo = reader.GetSafeString(startingIndex++);
            aLocation.City = reader.GetSafeString(startingIndex++);
            aLocation.Zip = reader.GetSafeString(startingIndex++);

            state.Id = reader.GetSafeInt32(startingIndex++);
            state.Name = reader.GetSafeString(startingIndex++);
            aLocation.State = state;

            aLocation.Latitude = reader.GetSafeDouble(startingIndex++);
            aLocation.Longitude = reader.GetSafeDouble(startingIndex++);
            aLocation.DateCreated = reader.GetSafeDateTime(startingIndex++);
            aLocation.DateModified = reader.GetSafeDateTime(startingIndex++);
            aLocation.CreatedBy = reader.GetSafeInt32(startingIndex++);
            aLocation.ModifiedBy = reader.GetSafeInt32(startingIndex++);

            return aLocation;
        }
    }
}
