using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dtos
{
    public class JWTDtoResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string WebIntegrationID { get; set; } = string.Empty;
    }
}
