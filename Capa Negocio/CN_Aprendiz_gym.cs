using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Datos;
using Capa_Entidad;

namespace Capa_Negocio
{
    public class CN_Aprendiz_gym
    {
        public List<Aprendiz> Listar()
        {
            // Método para listar todos los aprendices
            return new CD_Aprendiz().Listar();
        }

        // Método para registrar un nuevo aprendiz
        public int RegistrarAprendiz(Aprendiz obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            int resultado = 0;

            try
            {
                // Validación de campos
                if (obj == null)
                {
                    Mensaje = "No se ha proporcionado información del aprendiz.";
                    return 0;
                }

                if (string.IsNullOrEmpty(obj.nombre_aprendiz))
                {
                    Mensaje = "El nombre del aprendiz es requerido.";
                    return 0;
                }

                if (string.IsNullOrEmpty(obj.tipo_id_aprendiz))
                {
                    Mensaje = "El tipo de identificación del aprendiz es requerido.";
                    return 0;
                }

                if (string.IsNullOrEmpty(obj.contraseña_aprendiz))
                {
                    Mensaje = "La contraseña del aprendiz es requerida.";
                    return 0;
                }

                if (obj.telefono_aprendiz == 0)
                {
                    Mensaje = "El teléfono del aprendiz es requerido.";
                    return 0;
                }

                if (string.IsNullOrEmpty(obj.correo_aprendiz))
                {
                    Mensaje = "El correo del aprendiz es requerido.";
                    return 0;
                }

                if (obj.oFicha == null || obj.oFicha.numero_ficha <= 0)
                {
                    Mensaje = "El número de ficha es requerido y debe ser un número válido.";
                    return 0;
                }

                // Llamar al método de registro en la capa de datos
                resultado = new CD_Aprendiz().RegistrarAprendiz(obj, out Mensaje);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar registrar aprendiz: " + ex.Message);
                Mensaje = "Error al intentar registrar aprendiz: " + ex.Message;
            }

            return resultado;
        }

        // Método para consultar un aprendiz por su ID
        public Aprendiz ConsultarAprendiz(int id_aprendiz)
        {
            return new CD_Aprendiz().ConsultarAprendiz(id_aprendiz);
        }

        //Método para consultar un aprendiz para restablecer contraseña
        public Aprendiz ConsultarAprendizReestablecer(int id_aprendiz)
        {
            return new CD_Aprendiz().ConsultarAprendizReestablecer(id_aprendiz);
        }

        //Método para obtener un aprendiz por su ID que se usa para saber que aprendiz se encuentra autenticado
        public Aprendiz ObtenerPorId(string id_aprendiz)
        {
            return new CD_Aprendiz().ObtenerPorId(id_aprendiz);
        }
    }
}
