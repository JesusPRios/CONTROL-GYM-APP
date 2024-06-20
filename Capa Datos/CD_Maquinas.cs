using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Capa_Entidad; 

namespace Capa_Datos
{
    public class CD_Maquinas
    {
        // Método para listar todas las máquinas
        public List<Maquinas> Listar()
        {
            List<Maquinas> lista = new List<Maquinas>();  // Inicializa una lista de objetos Maquinas

            try
            {
                // Establece la conexión utilizando la cadena de conexión desde la clase Conexion
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    // Consulta SQL para seleccionar todas las máquinas
                    string query = "select id_maquina, nombre_maquina, tipo_maquina, cantidad_maquinas, estado_maquina from Maquinas";
                    SqlCommand cmd = new SqlCommand(query, oconexion); 
                    cmd.CommandType = CommandType.Text; 

                    oconexion.Open();  // Abre la conexión a la base de datos

                    using (SqlDataReader dr = cmd.ExecuteReader())  // Ejecuta la consulta y obtiene un lector de datos
                    {
                        while (dr.Read())  // Itera sobre cada fila del resultado
                        {
                            // Crea un objeto Maquinas y lo agrega a la lista
                            lista.Add(new Maquinas()
                            {
                                id_maquina = Convert.ToInt32(dr["id_maquina"]),
                                nombre_maquina = dr["nombre_maquina"].ToString(),
                                tipo_maquina = dr["tipo_maquina"].ToString(),
                                cantidad_maquinas = Convert.ToInt32(dr["cantidad_maquinas"]),
                                estado_maquina = dr["estado_maquina"].ToString()
                            });
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Maquinas>();  // Si hay excepción, inicializa la lista como una lista vacía
            }

            return lista;  // Devuelve la lista de máquinas
        }

        // Método para registrar una máquina
        public int Registrar(Maquinas obj, out string Mensaje)
        {
            int idautogenerado = 0; 
            Mensaje = string.Empty;  

            try
            {
                // Establece la conexión utilizando la cadena de conexión desde la clase Conexion
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("RegistrarMaquina", oconexion);  // Crea un comando SQL para llamar al procedimiento almacenado
                    cmd.Parameters.AddWithValue("nombre_maquina", obj.nombre_maquina); 
                    cmd.Parameters.AddWithValue("tipo_maquina", obj.tipo_maquina);
                    cmd.Parameters.AddWithValue("cantidad_maquinas", obj.cantidad_maquinas);
                    cmd.Parameters.AddWithValue("estado_maquina", obj.estado_maquina);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;  
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;  

                    oconexion.Open();  // Abre la conexión a la base de datos

                    cmd.ExecuteNonQuery();  // Ejecuta el comando SQL

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);  // Obtiene el ID autogenerado
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();  // Obtiene el mensaje de la base de datos
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;  // Si hay excepción, establece el ID autogenerado como 0
                Mensaje = ex.Message;  // Guarda el mensaje de excepción
            }

            return idautogenerado;  // Devuelve el ID autogenerado
        }
    }
}