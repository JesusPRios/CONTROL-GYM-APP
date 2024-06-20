using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos;
using Capa_Entidad;

namespace Capa_Negocio
{
    public class CN_Rutina
    {
        private CD_Rutina objcapa_dato = new CD_Rutina(); // Instancia de la capa de datos para interactuar con la base de datos

        // Método para listar rutinas de un aprendiz
        public List<Rutina> Listar(int id_aprendiz)
        {
            return objcapa_dato.Listar(id_aprendiz);  // Llama al método de la capa de datos para listar rutinas
        }

        // Método para registrar una rutina
        public bool Registrar_rutina(Rutina obj, out string Mensaje)
        {
            bool resultado = false;  // Inicializa el resultado como falso
            Mensaje = string.Empty;  // Inicializa el mensaje como cadena vacía

            // Validaciones
            if (string.IsNullOrEmpty(obj.nombre_rutina))
            {
                Mensaje = "Debe ingresar el nombre de la rutina";
            }
            else if (string.IsNullOrEmpty(obj.oEjercicio.nombre_ejercicio))
            {
                Mensaje = "Debe ingresar el nombre del ejercicio";
            }
            else if (string.IsNullOrEmpty(obj.duracion_rutina))
            {
                Mensaje = "Debe ingresar los días y las horas de ejercicio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                // Llama al método de la capa de datos para registrar la rutina
                return objcapa_dato.Registrar_rutina(obj, out Mensaje);
            }
            else
            {
                return resultado;  // Retorna falso si hay errores de validación
            }
        }

        // Método para eliminar una rutina
        public bool Eliminar_rutina(int id_rutina)
        {
            return objcapa_dato.Eliminar_rutina(id_rutina);  // Llama al método de la capa de datos para eliminar la rutina
        }
    }
}