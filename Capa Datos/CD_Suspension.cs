using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Capa_Datos;
using Capa_Entidad;
using System.Data;

namespace Capa_Datos
{
    public class CD_Suspension
    {

        // Método para listar las suspenciones
        public List<Suspension> Listar()
        {
            List<Suspension> lista = new List<Suspension>();  // Crea una lista vacía de Suspensiones

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))  // Crea una conexión utilizando la cadena de conexión de la clase Conexion
                {
                    string query = "SELECT Suspension.id_suspension, Suspension.descripcion_suspension, Suspension.fecha_inicio_suspension, Suspension.fecha_fin_suspension, Suspension.duracion_suspension, Suspension.estado_suspension, Aprendiz.id_aprendiz, Aprendiz.nombre_aprendiz FROM Suspension INNER JOIN Aprendiz ON Suspension.id_aprendiz = Aprendiz.id_aprendiz";  // Consulta SQL para seleccionar todas las suspensiones con información adicional del aprendiz
                    SqlCommand cmd = new SqlCommand(query, oconexion);  // Crea un comando SQL con la consulta y la conexión
                    cmd.CommandType = CommandType.Text;  // Tipo de comando es texto (consulta SQL)

                    oconexion.Open();  // Abre la conexión a la base de datos

                    using (SqlDataReader dr = cmd.ExecuteReader())  // Crea un lector de datos SQL para leer los resultados de la consulta
                    {
                        while (dr.Read())  // Mientras haya filas para leer
                        {
                            lista.Add(  // Agrega una nueva suspensión a la lista
                                new Suspension()
                                {
                                    id_suspension = Convert.ToInt32(dr["id_suspension"]),  
                            descripcion_suspension = dr["descripcion_suspension"].ToString(),  
                            fecha_inicio_suspension = dr["fecha_inicio_suspension"].ToString(),  
                            fecha_fin_suspension = dr["fecha_fin_suspension"].ToString(), 
                            duracion_suspension = dr["duracion_suspension"].ToString(),  
                            estado_suspension = dr["estado_suspension"].ToString(), 

                            oAprendiz = new Aprendiz  // Crea un nuevo objeto Aprendiz
                            {
                                        id_aprendiz = Convert.ToInt32(dr["id_aprendiz"]),  // Convierte y asigna el valor de id_aprendiz
                                nombre_aprendiz = dr["nombre_aprendiz"].ToString()  // Asigna el nombre del aprendiz
                            }
                                }
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Captura de excepciones
                Console.WriteLine("Error al listar las suspensiones: " + ex.Message);  // Muestra el mensaje de error en la consola
                lista = new List<Suspension>();  // Crea una nueva lista vacía de Suspensiones
            }

            return lista;  // Devuelve la lista de Suspensiones
        }

        // Método para registrar nuevas suspenciones
        public int Registrar_suspension(Suspension obj, out string Mensaje)
        {
            Mensaje = string.Empty;  
            int resultado = 0;  

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))  // Crea una conexión utilizando la cadena de conexión de la clase Conexion
                {
                    oconexion.Open();  // Abre la conexión a la base de datos

                    SqlCommand cmd = new SqlCommand("Guardar_suspension", oconexion);  // Crea un comando SQL para llamar al procedimiento almacenado Guardar_suspension
                    cmd.CommandType = CommandType.StoredProcedure;  

                    // Añade parámetros al comando SQL
                    cmd.Parameters.AddWithValue("@descripcion_suspension", obj.descripcion_suspension);
                    cmd.Parameters.AddWithValue("@fecha_inicio_suspension", obj.fecha_inicio_suspension);
                    cmd.Parameters.AddWithValue("@fecha_fin_suspension", obj.fecha_fin_suspension);
                    cmd.Parameters.AddWithValue("@duracion_suspension", obj.duracion_suspension);
                    cmd.Parameters.AddWithValue("@estado_suspension", obj.estado_suspension);
                    cmd.Parameters.AddWithValue("@nombre_aprendiz", obj.nombre_aprendiz); // Asegúrate de que este parámetro está en el objeto 'obj'
                    cmd.Parameters.AddWithValue("@id_aprendiz", obj.oAprendiz.id_aprendiz);
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output; 
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;  

                    cmd.ExecuteNonQuery();  // Ejecuta el comando SQL

                    // Obtiene el resultado y el mensaje de la base de datos
                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;  // Guarda el mensaje de excepción
            }

            return resultado;  // Devuelve el resultado
        }

        // Método para consultar las suspenciones
        public Suspension Consultar_suspension(int id_aprendiz)
        {
            Suspension suspension = null;  // Inicializa la suspensión como nula

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))  // Crea una conexión utilizando la cadena de conexión de la clase Conexion
                {
                    oconexion.Open();  // Abre la conexión a la base de datos
                    string query = "SELECT id_suspension, descripcion_suspension, fecha_inicio_suspension, fecha_fin_suspension, duracion_suspension, estado_suspension, id_aprendiz FROM Suspension WHERE id_aprendiz = @id_aprendiz";  // Consulta SQL para seleccionar la suspensión por el ID del aprendiz

                    SqlCommand cmd = new SqlCommand(query, oconexion);  // Crea un comando SQL para ejecutar la consulta
                    cmd.Parameters.AddWithValue("@id_aprendiz", id_aprendiz);  // Añade el parámetro para el ID del aprendiz a la consulta

                    using (SqlDataReader dr = cmd.ExecuteReader())  // Ejecuta la consulta y obtiene el lector de datos
                    {
                        if (dr.Read())  // Si hay datos para leer
                        {
                            // Crea un objeto de tipo Suspension con los datos obtenidos de la consulta
                            suspension = new Suspension
                            {
                                id_suspension = Convert.ToInt32(dr["id_suspension"]),
                                descripcion_suspension = dr["descripcion_suspension"].ToString(),
                                fecha_inicio_suspension = dr["fecha_inicio_suspension"].ToString(),
                                fecha_fin_suspension = dr["fecha_fin_suspension"].ToString(),
                                duracion_suspension = dr["duracion_suspension"].ToString(),
                                estado_suspension = dr["estado_suspension"].ToString()
                            };
                        }
                    }
                }
            }
            catch (Exception)
            {
                suspension = null;  // En caso de excepción, asigna null a la suspensión
            }

            return suspension;  // Devuelve la suspensión encontrada o null si no se encontró ninguna
        }
    }
}