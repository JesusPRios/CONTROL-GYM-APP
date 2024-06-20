using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Entidad;
using Capa_Datos;

namespace Capa_Negocio
{
    public class CN_Multa
    {
        private CD_Multa objcapa_dato = new CD_Multa();  // Instancia de la capa de datos para interactuar con la base de datos

        // Método para registrar una multa
        public int Registrar_multa(Multa obj, out string mensaje)
        {
            mensaje = string.Empty;  // Inicializa el mensaje como cadena vacía

            // Validacion de campos
            if (string.IsNullOrEmpty(obj.estado_multa))
            {
                mensaje = "Debe ingresar el estado de la multa";
                return 0;  // Retorna 0 si hay errores de validación
            }

            return objcapa_dato.Registrar_multa(obj, out mensaje);
        }
    }
}
