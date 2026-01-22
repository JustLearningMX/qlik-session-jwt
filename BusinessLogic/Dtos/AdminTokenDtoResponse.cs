using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dtos
{
    public class AdminTokenDtoResponse
    {
        public string access_token { get; set; } = string.Empty;
        public string scope { get; set; } = string.Empty;
        public string token_type { get; set; } = string.Empty;
        public DateTime expires_at { get; set; } = DateTime.MinValue;
        public int expires_in { get; set; }
    }
}
