using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessLogic.Dtos
{
    public class GenericResult<T>
    {

        // Codigo del error. 404, 500, 201 etc
        [JsonPropertyName("codigo")]
        public int Codigo { get; set; }

        // El mensaje que queremos mostrar
        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; }

        // Si hay mensajes de errores o excepciones
        [JsonPropertyName("detalles")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<string> Detalles { get; set; } //<T> en lugar de <string>

        // Cuando el resultado es un objeto
        [JsonPropertyName("resultado")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Resultado { get; set; }

        // Cuando el resultado es un arreglo
        [JsonPropertyName("resultados")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<T>? Resultados { get; set; }

        // Metodos para settear los 2 primeros valores, el Codigo y un mensaje
        public void OperacionExitosa()
        {
            this.Codigo = (int)HttpStatusCode.OK;
            this.Mensaje = "Operación exitosa";
        }

        public void CreacionExitosa()
        {
            this.Codigo = (int)HttpStatusCode.Created;
            this.Mensaje = "Creacion exitosa";
        }

        public void ActualizacionExitosa()
        {
            this.Codigo = (int)HttpStatusCode.OK;
            this.Mensaje = "Actualizacion exitosa";
        }

        public void ConsultaExitosa()
        {
            this.Codigo = (int)HttpStatusCode.OK;
            this.Mensaje = "Consulta exitosa";
        }

        public void EliminacionExitosa()
        {
            this.Codigo = (int)HttpStatusCode.NoContent;
            this.Mensaje = "Eliminacion exitosa";
        }


    }
}
