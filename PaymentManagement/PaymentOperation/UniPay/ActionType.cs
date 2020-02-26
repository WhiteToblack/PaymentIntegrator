﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentManagement.PaymentOperation.UniPay
{
    public enum ActionType
    {
        SALE,
        DENY,
        PREAUTH,
        POSTAUTH,
        DETACHEDREFUND,
        EXTERNALREFUND,
        RECURRINGPLANDELETE,
        QUERYCAMPAIGNONLINE,
        QUERYPOINTS
    }
}