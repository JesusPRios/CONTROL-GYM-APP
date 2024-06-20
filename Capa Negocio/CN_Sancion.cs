using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Entidad;
using Capa_Datos;

namespace Capa_Negocio
{
    public class CN_Sancion
    {
        private CD_Sancion objcapa_dato = new CD_Sancion(); // Instancia de la capa de datos para interactuar con la base de datos

        // Método para listar todas las sanciones
        public List<Sancion> Listar()
        {
            return objcapa_dato.Listar();  // Llama al método de la capa de datos para listar sanciones
        }

        // Método para registrar una nueva sanción
        public int Registrar_sancion(Sancion obj, out string Mensaje)
        {
            Mensaje = string.Empty;  // Inicializa el mensaje como cadena vacía

            // Validaciones
            if (string.IsNullOrEmpty(obj.tipo_sancion))
            {
                Mensaje = "Debe ingresar el tipo de sanción.";  // Establece un mensaje de error si el tipo de sanción está vacío
                return 0;
            }

            // Intentar registrar la sanción llamando al método de la capa de datos
            return objcapa_dato.Registrar_sancion(obj, out Mensaje);
        }
    }
}