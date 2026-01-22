using BusinessLogic.Dtos;
using BusinessLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class ExceptionService
    {
        public GenericResult<T> GeneraError<T>(Exception ex)
        {
            GenericResult<T> result = new GenericResult<T>();
            ApiException apiException = ex is ApiException ? (ApiException)ex : new ApiException((int)HttpStatusCode.InternalServerError, "Error en el servidor", ex.Message);
            result.Codigo = apiException.Code;
            result.Mensaje = apiException.Message;
            result.Detalles = apiException.Detalles;

            return result;
        }
    }
}
