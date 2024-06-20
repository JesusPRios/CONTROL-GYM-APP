using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos;
using Capa_Entidad;

namespace Capa_Negocio
{
    public class CN_Suspension
    {
        private CD_Suspension objcapa_dato = new CD_Suspension(); // Instancia de la capa de datos para interactuar con la base de datos

        // Método para listar todas las suspensiones
        public List<Suspension> Listar()
        {
            return objcapa_dato.Listar(); // Llama al método de la capa de datos para listar suspensiones
        }

        // Método para registrar una nueva suspensión
        public int Registrar_suspension(Suspension obj, out string Mensaje)
        {
            Mensaje = string.Empty; // Inicializa el mensaje como cadena vacía

            // Validacion de campos
            if (string.IsNullOrEmpty(obj.id_aprendiz.ToString()))
            {
                Mensaje = "Debe ingresar el ID del Aprendiz.";
            }
            else if (string.IsNullOrEmpty(obj.descripcion_suspension))
            {
                Mensaje = "Debe ingresar una descripción."; 
            }
            else if (string.IsNullOrEmpty(obj.fecha_inicio_suspension))
            {
                Mensaje = "Debe ingresar la fecha de inicio."; 
            }
            else if (string.IsNullOrEmpty(obj.estado_suspension))
            {
                Mensaje = "Debe ingresar el estado de la suspensión."; 
            }

            // Si no hay errores, intenta registrar la suspensión llamando al método de la capa de datos
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objcapa_dato.Registrar_suspension(obj, out Mensaje);
            }
            else
            {
                return 0; // Retorna 0 si hay errores de validación
            }
        }

        // Método para consultar una suspensión por ID de aprendiz
        public Suspension Consultar_suspension(int id_aprendiz)
        {
            return objcapa_dato.Consultar_suspension(id_aprendiz); // Llama al método de la capa de datos para consultar la suspensión
        }
    }
}