using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class QlikCloudSettings
    {
        public string WebIntegrationID { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public string TenantUrl { get; set; } = string.Empty;
        public string OAuthTokenURL { get; set; } = string.Empty;
        public string SessionURL { get; set; } = string.Empty;
        public string GroupListURL { get; set; } = string.Empty;
        public string GroupListFilterURL { get; set; } = string.Empty;
        public string ApiKeyToken { get; set; } = string.Empty;
        public string ClientID { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}
