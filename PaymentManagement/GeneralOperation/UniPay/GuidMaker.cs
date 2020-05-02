using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PaymentManagement.GeneralOperation.UniPay
{
    public static class GuidMaker
    {
        public static string GetGuid() {
            int firstSeg = new Random().Next(111, 999999);
            int secSeg = new Random(1).Next(111, 999999);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}-{1}", firstSeg, secSeg);
            return sb.ToString();
        }
    }
}