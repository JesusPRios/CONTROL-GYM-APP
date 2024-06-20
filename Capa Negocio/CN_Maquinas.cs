using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos;
using Capa_Entidad;

namespace Capa_Negocio
{
    public class CN_Maquinas
    {
        private CD_Maquinas objcapa_dato = new CD_Maquinas();  // Instancia de la capa de datos para interactuar con la base de datos

        // Método para listar todas las máquinas
        public List<Maquinas> Listar()
        {
            return objcapa_dato.Listar(); 
        }

        // Método para registrar una nueva máquina
        public int Registrar(Maquinas obj, out string Mensaje)
        {
            Mensaje = string.Empty;  // Inicializa el mensaje como cadena vacía

            // Validacion de campos
            if (string.IsNullOrEmpty(obj.nombre_maquina) || string.IsNullOrWhiteSpace(obj.nombre_maquina))
            {
                Mensaje = "El nombre de la maquina no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.tipo_maquina) || string.IsNullOrWhiteSpace(obj.tipo_maquina))
            {
                Mensaje = "El tipo de maquina no puede estar vacío";
            }
            else if (string.IsNullOrEmpty(obj.estado_maquina) || string.IsNullOrWhiteSpace(obj.estado_maquina))
            {
                Mensaje = "El estado de maquina no puede estar vacío";
            }
            else if (obj.cantidad_maquinas <= 0)
            {
                Mensaje = "La cantidad de maquinas no puede estar vacía o ser cero";
            }

            // Si no hay mensajes de error, llama al método de registro en la capa de datos
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objcapa_dato.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;  // Retorna 0 si hay mensajes de error
            }
        }
    }
}