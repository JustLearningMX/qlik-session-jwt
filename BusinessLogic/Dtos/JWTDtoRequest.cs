using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Dtos
{
    public class JWTDtoRequest
    {
        public string email { get; set; } = string.Empty;
        public string name{ get; set; } = string.Empty;
        public string[] groups { get; set; } = Array.Empty<string>();
    }
}
