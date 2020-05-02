using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integrator.Models
{
    public enum ActionType
    {
        SALE,
        DENY,
        PREAUTH,
        POSTAUTH,
        QUERYPOINTS,
        SESSIONTOKEN,
        QUERYSESSION,
        QUERYPAYMENT,
        DETACHEDREFUND,
        EXTERNALREFUND,
        RECURRINGPLANDELETE,
        QUERYCAMPAIGNONLINE,       
        QUERYMAXINSTALLMENTCOUNTS
    }
}
