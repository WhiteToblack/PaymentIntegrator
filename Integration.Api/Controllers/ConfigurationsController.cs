using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Integrator.Api.DbOperation;

namespace Integrator.Api.Controllers
{
    [Route("api/Configurations/{action}")]
    public class ConfigurationsController : ApiController
    {       
        [HttpGet]
        public Dictionary<string, string> GetAllConfigurations() {
            return PaymentConfiguration.Instance.ConfigurationInstance;
        }
    }
}
