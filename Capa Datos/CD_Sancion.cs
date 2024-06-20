using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Capa_Datos;
using Capa_Entidad;
namespace Capa_Datos
{
    public class CD_Sancion
    {

        // Método para listar las sanciones
        public List<Sancion> Listar()
        {
            List<Sancion> lista = new List<Sancion>();  

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))  // Crea una conexión utilizando la cadena de conexión de la clase Conexion
                {
                    string query = "SELECT id_sancion, tipo_sancion, id_aprendiz, nombre_aprendiz FROM Sancion";  // Consulta SQL para seleccionar todas las sanciones
                    SqlCommand cmd = new SqlCommand(query, oconexion);  // Crea un comando SQL con la consulta y la conexión
                    cmd.CommandType = CommandType.Text;  

                    oconexion.Open();  // Abre la conexión a la base de datos

                    using (SqlDataReader dr = cmd.ExecuteReader())  // Crea un lector de datos SQL para leer los resultados de la consulta
                    {
                        while (dr.Read())  // Itera sobre cada fila del resultado
                        {
                            lista.Add(new Sancion()  // Añade una nueva instancia de Sancion a la lista
                            {
                                id_sancion = Convert.ToInt32(dr["id_sancion"]), 
                                tipo_sancion = dr["tipo_sancion"].ToString(),
                                id_aprendiz = Convert.ToInt32(dr["id_aprendiz"]),  
                                nombre_aprendiz = dr["nombre_aprendiz"].ToString() 
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones: registra el error en la consola
                Console.WriteLine("Error al listar las sanciones: " + ex.Message);
                lista = new List<Sancion>();  // Reinicia la lista en caso de error
            }

            return lista;  // Devuelve la lista de sanciones
        }

        // Método para registrar una nueva sancion
        public int Registrar_sancion(Sancion obj, out string Mensaje)
        {
            Mensaje = string.Empty;  
            int resultado = 0;  

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))  // Crea una conexión utilizando la cadena de conexión de la clase Conexion
                {
                    oconexion.Open();  // Abre la conexión a la base de datos

                    using (SqlCommand cmd = new SqlCommand("Registrar_sancion", oconexion))  // Crea un comando SQL para llamar al procedimiento almacenado Registrar_sancion
                    {
                        cmd.CommandType = CommandType.StoredProcedure;  // Tipo de comando es procedimiento almacenado

                        // Añade parámetros al comando SQL
                        cmd.Parameters.Add(new SqlParameter("@tipo_sancion", SqlDbType.VarChar, 255)).Value = obj.tipo_sancion;
                        cmd.Parameters.Add(new SqlParameter("@id_aprendiz", SqlDbType.Int)).Value = obj.id_aprendiz;
                        cmd.Parameters.Add(new SqlParameter("@nombre_aprendiz", SqlDbType.VarChar, 50)).Value = obj.nombre_aprendiz;
                        cmd.Parameters.Add(new SqlParameter("@Resultado", SqlDbType.Int)).Direction = ParameterDirection.Output;  
                        cmd.Parameters.Add(new SqlParameter("@Mensaje", SqlDbType.VarChar, 500)).Direction = ParameterDirection.Output; 

                        cmd.ExecuteNonQuery();  // Ejecuta el comando SQL

                        // Obtiene el resultado y el mensaje de la base de datos
                        resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                        Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error al registrar sanción: " + ex.Message;  // Guarda el mensaje de excepción
                                                                      
                Console.WriteLine(ex.ToString());
            }

            return resultado;  // Devuelve el resultado
        }
    }
}
