using System.Collections.Generic;
using Capa_Entidad;
using Capa_Datos;
using System;

namespace Capa_Negocio
{
    public class CN_Administrador
    {
        public Administrador ObtenerPorId(string idAdministrador)
        {
            // Método para obtener un administrador por su ID que esta en la capa datos
            return new CD_Administrador().ObtenerPorId(idAdministrador);
        }

        // Método para registrar un nuevo administrador
        public int Registrar_administrador(Administrador obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            int resultado = 0;

            try
            {
                // Validación de campos 
                if (obj == null)
                {
                    Mensaje = "No se ha proporcionado información del administrador.";
                    return 0;
                }

                if (obj.id_administrador == 0)
                {
                    Mensaje = "El id del administrador es requerido.";
                    return 0;
                }

                if (string.IsNullOrEmpty(obj.tipo_id_admin))
                {
                    Mensaje = "El tipo de identificación del administrador es requerido.";
                    return 0;
                }

                if (string.IsNullOrEmpty(obj.nombre_administrador))
                {
                    Mensaje = "El nombre del administrador es requerida.";
                    return 0;
                }

                if (obj.edad_administrador == 0)
                {
                    Mensaje = "La edad del administador es requerido.";
                    return 0;
                }

                if (string.IsNullOrEmpty(obj.correo_admin))
                {
                    Mensaje = "El correo del administrador es requerido.";
                    return 0;
                }

                if (obj.telefono_admin == 0)
                {
                    Mensaje = "El telefono del administrador es requerido.";
                    return 0;
                }

                if (string.IsNullOrEmpty(obj.contraseña_administrador))
                {
                    Mensaje = "La contraseña del administrador es requerido.";
                    return 0;
                }


                // Llamar al método de registro en la capa de datos
                resultado = new CD_Administrador().Registrar_administrador(obj, out Mensaje);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al intentar registrar aprendiz: " + ex.Message); //Muestra el mensaje de error en la consola.
                Mensaje = "Error al intentar registrar aprendiz: " + ex.Message; // Asigna el mensaje de error a la variable Mensaje
            }

            return resultado; //Devuelve el resultado del registro
        }
    }
}