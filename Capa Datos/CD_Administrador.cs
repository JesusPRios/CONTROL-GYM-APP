using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Capa_Entidad; 
using System.Configuration;

namespace Capa_Datos
{
    public class CD_Administrador
    {
        // Cadena de conexión obtenida del archivo de configuración
        private string connectionString = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        // Método para obtener un administrador por su ID
        public Administrador ObtenerPorId(string idAdministrador)
        {
            Administrador administrador = null;

            try
            {
                // Abrir conexión a la base de datos utilizando using para garantizar la disposición correcta
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();  // Abrir la conexión
                    string query = "SELECT * FROM Administrador WHERE id_administrador = @id_administrador";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_administrador", idAdministrador);

                    // Ejecutar consulta SELECT y leer resultados usando SqlDataReader
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Crear objeto Administrador y asignar valores desde el SqlDataReader
                            administrador = new Administrador
                            {
                                id_administrador = Convert.ToInt32(reader["id_administrador"]),
                                nombre_administrador = reader["nombre_administrador"].ToString(),
                                tipo_id_admin = reader["tipo_id_admin"].ToString(),
                                edad_administrador = Convert.ToInt32(reader["edad_administrador"]),
                                contraseña_administrador = reader["contraseña_administrador"].ToString(),
                                correo_admin = reader["correo_admin"].ToString(),
                                telefono_admin = Convert.ToInt64(reader["telefono_admin"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener administrador por ID: " + ex.Message);  // Manejo de excepciones
            }

            return administrador;
        }

        // Método para registrar un nuevo administrador
        public int Registrar_administrador(Administrador obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            int resultado = 0;

            try
            {
                // Abrir conexión a la base de datos utilizando using para garantizar la disposición correcta
                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    oconexion.Open();  // Abrir la conexión

                    // Definir y configurar el comando SQL para ejecutar el procedimiento almacenado
                    SqlCommand cmd = new SqlCommand("Registrar_administrador", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;  // Tipo de comando es un procedimiento almacenado
                    cmd.Parameters.AddWithValue("@id_administrador", obj.id_administrador);  // Parámetros de entrada
                    cmd.Parameters.AddWithValue("@tipo_id_admin", obj.tipo_id_admin);
                    cmd.Parameters.AddWithValue("@nombre_administrador", obj.nombre_administrador);
                    cmd.Parameters.AddWithValue("@edad_administrador", obj.edad_administrador);
                    cmd.Parameters.AddWithValue("@correo_admin", obj.correo_admin);
                    cmd.Parameters.AddWithValue("@telefono_admin", obj.telefono_admin);
                    cmd.Parameters.AddWithValue("@contraseña_administrador", obj.contraseña_administrador);
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;  // Parámetro de salida
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;  // Parámetro de salida

                    // Ejecutar el procedimiento almacenado
                    cmd.ExecuteNonQuery();

                    // Obtener valores de los parámetros de salida
                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Mensaje = "Error al intentar registrar administrador: " + ex.Message;
                Console.WriteLine(Mensaje);
            }

            return resultado;
        }
    }
}