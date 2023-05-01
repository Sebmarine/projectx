﻿using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain.StripeSubscriptions;
using Sabio.Models.Domain.Subscriptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services
{
    public class SubscriptionTierService : ISubscriptionService
    {
        IDataProvider _data = null;

        public SubscriptionTierService(IDataProvider data)
        {
            _data = data;
        }

        public List<Subscription> GetAllByUserId(int userId)
        {
            string procName = "[dbo].[Subscriptions_SelectByUserId]";
            List<Subscription> list = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@UserId", userId);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                Subscription subscription = MapSingleSubscription(reader);

                if (list == null)
                {
                    list = new List<Subscription>();
                }
                list.Add(subscription);
            });
            return list;
        }

        public List<StripeSubscriptionPayment> GetAllSubscriptionPayments()
        {
            string procName = "[dbo].[StripeSubscriptionPayments_SelectAll]";
            List<StripeSubscriptionPayment> list = null;

            _data.ExecuteCmd(procName,null, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                StripeSubscriptionPayment payment = MapSinglePayment(reader);

                if (list == null)
                {
                    list = new List<StripeSubscriptionPayment>();
                }
                list.Add(payment);
            });
            return list;
        }

        private static Subscription MapSingleSubscription(IDataReader reader)
        {
            Subscription subscription = new Subscription();

            int startingIndex = 0;
            subscription.Id = reader.GetSafeInt32(startingIndex++);
            subscription.SubscriptionId = reader.GetSafeString(startingIndex++);
            subscription.UserId = reader.GetSafeInt32(startingIndex++);
            subscription.CustomerId = reader.GetSafeString(startingIndex++);
            subscription.DateEnded = reader.GetSafeUtcDateTime(startingIndex++);
            subscription.isActive = reader.GetSafeString(startingIndex++);
            subscription.ProductId = reader.DeserializeObject<List<SubscriptionProduct>>(startingIndex++);
            return subscription;
        }

        private static StripeSubscriptionPayment MapSinglePayment(IDataReader reader)
        {
            StripeSubscriptionPayment payment = new StripeSubscriptionPayment();

            int startingIndex = 0;
        
            payment.SubscriptionId = reader.GetSafeString(startingIndex++);
            payment.DateEnded = reader.GetSafeUtcDateTime(startingIndex++);
            payment.Total = reader.GetSafeInt32(startingIndex++);
            return payment;
        }
    }
}
