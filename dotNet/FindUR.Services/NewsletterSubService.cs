using Amazon.Runtime.Internal;
using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain.NewsletterSubscriptions;
using Sabio.Models.Requests.NewsletterSubscriptions;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace Sabio.Services
{
    public class NewsletterSubService : INewsletterSubService
    {
        private static IDataProvider _data = null;

        public NewsletterSubService(IDataProvider data)
        {
            _data = data;
        }

        public Paged<NewsSub> GetAllPagination(int pageIndex, int pageSize)
        {
            string procName = "[dbo].[NewsletterSubscriptions_SelectAll]";
            Paged<NewsSub> pagedList = null;
            List<NewsSub> list = null;

            _data.ExecuteCmd(procName, inputParamMapper: (param) =>
            {
                param.AddWithValue("@pageIndex", pageIndex);
                param.AddWithValue("@pageSize", pageSize);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                NewsSub subs = MapSubscriber(reader, ref startingIndex);
                int totalCount = reader.GetSafeInt32(startingIndex++);
                

                if (list == null)
                {
                    list = new List<NewsSub>();
                }
                list.Add(subs);
                if (list != null)
                {
                    pagedList = new Paged<NewsSub>(list, pageIndex, pageSize, totalCount);
                }
            }, returnParameters: null);

            return pagedList;
        }

        public List<NewsSub> GetAllSubscribed()
        {
            string procName = "[dbo].[NewsletterSubscriptions_SelectAll_Subscribed]";
            List<NewsSub> list = null;

            _data.ExecuteCmd(procName, inputParamMapper: null, 
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;
                NewsSub subs = MapSubscriber(reader, ref startingIndex);

                if (list == null)
                {
                    list = new List<NewsSub>();
                }
                list.Add(subs);
             
            }, returnParameters: null);

            return list;
        } 

        public Paged<NewsSub> SelectByDate(int pageIndex, int pageSize, string date)
        {
            string procName = "[dbo].[NewsletterSubscriptions_Select_ByCreatedBy]";
            Paged<NewsSub> pagedList = null;
            List<NewsSub> list = null;

            _data.ExecuteCmd(procName, inputParamMapper: (param) =>
            {
                param.AddWithValue("@pageIndex", pageIndex);
                param.AddWithValue("@pageSize", pageSize);
                param.AddWithValue("@Date", date);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int startingIndex = 0;

                NewsSub subs = MapSubscriber(reader, ref startingIndex);
                int totalCount = reader.GetSafeInt32(startingIndex++);

                if (list == null)
                {
                    list = new List<NewsSub>();
                }
                list.Add(subs);
                if (list != null)
                {
                    pagedList = new Paged<NewsSub>(list, pageIndex, pageSize, totalCount);
                }
            }, returnParameters: null);

            return pagedList;
        }

        public string Add(NewsSubAddRequest model)
        {
            string procName = "[dbo].[NewsletterSubscriptions_Insert]";
            string email = "";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Email", model.Email);

            }, returnParameters: delegate (SqlParameterCollection param)
            {
                email = param["@Email"].Value.ToString();
            });

            return email;
        }

        public void Update(NewsSubUpdateRequest model)
        {
            string procName = "[dbo].[NewsletterSubscriptions_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Email", model.Email);
                col.AddWithValue("@isSubscribed", model.isSubscribed);
            }, returnParameters: null);
        }

        private static NewsSub MapSubscriber(IDataReader reader, ref int startingIndex)
        {
            NewsSub newsSub = new NewsSub();

            newsSub.Email = reader.GetSafeString(startingIndex++);
            newsSub.IsSubscribed = reader.GetSafeBool(startingIndex++);
            newsSub.DateCreated = reader.GetSafeDateTime(startingIndex++);
            newsSub.DateModified = reader.GetSafeDateTime(startingIndex++);

            return newsSub;
        }

    }
}
