using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class JwtSettings
    {
        public string PrivateCertificateFile { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string KeyID { get; set; } = string.Empty;
        public string TenantUrl { get; set; } = string.Empty;
        public string WebIntegrationID { get; set; } = string.Empty;
    }
}
