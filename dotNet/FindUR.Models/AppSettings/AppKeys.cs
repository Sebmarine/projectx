using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.AppSettings
{
    public class AppKeys
    {
        public string StripeSecretKey { get; set; }
        public string SendGridAppKey { get; set; }
        public string GoogleAnalyticsViewId { get; set; }
        public string DailyWebRTCAppKey { get; set; }
    }
}
