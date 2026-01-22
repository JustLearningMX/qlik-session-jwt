using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Exceptions
{
    public class ApiException : Exception
    {

        public int Code { get; set; }
        public IEnumerable<string> Detalles { get; set; }

        public ApiException(string message, params string[] detalles) : base(message)
        {
            this.Code = (int)HttpStatusCode.InternalServerError;
            this.Detalles = detalles;
        }

        public ApiException(int code, string message, params string[] detalles) : base(message)
        {
            this.Code = code;
            this.Detalles = detalles;
        }

        public ApiException(int code, string message, Exception inner, params string[] detalles) : base(message, inner)
        {
            this.Code = code;
            this.Detalles = detalles;
        }

        public ApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Code = (int)HttpStatusCode.InternalServerError; ;
            this.Detalles = new List<string>();
        }

    }
}
