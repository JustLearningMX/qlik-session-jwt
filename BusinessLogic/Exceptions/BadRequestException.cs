using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Exceptions
{
    public class BadRequestException : ApiException
    {

        public BadRequestException(string message) : base((int)HttpStatusCode.BadRequest, message)
        {
        }

        public BadRequestException(string message, params string[] detalles) : base((int)HttpStatusCode.BadRequest, message, detalles)
        {
        }
    }
}
