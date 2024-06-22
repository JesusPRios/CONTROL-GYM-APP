using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capa_Entidad;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Capa_Datos
{
    public class CD_Rutina
    {

        // Método para listar las rutinas
        public List<Rutina> Listar(int id_aprendiz)
        {
            List<Rutina> lista = new List<Rutina>();  // Crea una lista vacía para almacenar las rutinas

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))  // Crea una conexión utilizando la cadena de conexión de la clase Conexion
                {
                    string query = @"SELECT r.id_rutina, r.nombre_rutina, r.duracion_rutina, e.nombre_ejercicio, e.dias_ejercicio, e.horas_ejercicio, ISNULL(m.id_maquina, 0) AS id_maquina, ISNULL(m.nombre_maquina, '') AS nombre_maquina FROM Rutinas r INNER JOIN Ejercicios e ON r.id_rutina = e.id_rutina LEFT JOIN Rutina_Maquina rm ON r.id_rutina = rm.id_rutina LEFT JOIN Maquinas m ON rm.id_maquina = m.id_maquina WHERE r.id_aprendiz = @id_aprendiz";
                    SqlCommand cmd = new SqlCommand(query, oconexion);  // Crea un comando SQL para ejecutar la consulta
                    cmd.Parameters.AddWithValue("@id_aprendiz", id_aprendiz); 
                    cmd.CommandType = CommandType.Text;  
                    oconexion.Open();  // Abre la conexión a la base de datos

                    using (SqlDataReader dr = cmd.ExecuteReader())  // Ejecuta la consulta y crea un lector de datos
                    {
                        while (dr.Read())  // Itera sobre cada fila del resultado
                        {
                            int id_rutina = Convert.ToInt32(dr["id_rutina"]);  // Obtiene el ID de la rutina

                            // Busca si la rutina ya existe en la lista
                            var rutina = lista.FirstOrDefault(r => r.id_rutina == id_rutina);
                            if (rutina == null)
                            {
                                // Si no existe, crea una nueva rutina
                                rutina = new Rutina
                                {
                                    id_rutina = id_rutina,
                                    nombre_rutina = dr["nombre_rutina"].ToString(),
                                    duracion_rutina = dr["duracion_rutina"].ToString(),
                                    oEjercicio = new Ejercicio
                                    {
                                        nombre_ejercicio = dr["nombre_ejercicio"].ToString(),
                                        dias_ejercicio = Convert.ToInt32(dr["dias_ejercicio"]),
                                        horas_ejercicio = Convert.ToInt32(dr["horas_ejercicio"])
                                    },
                                    oRutina_Maquina = new List<Rutina_Maquina>(),
                                    oAprendiz = new Aprendiz
                                    {
                                        id_aprendiz = id_aprendiz
                                    }
                                };
                                lista.Add(rutina);  // Agrega la rutina a la lista
                            }

                            // Si existe, agrega las máquinas asociadas a la rutina
                            if (dr["id_maquina"] != DBNull.Value)
                            {
                                rutina.oRutina_Maquina.Add(new Rutina_Maquina
                                {
                                    id_maquina = Convert.ToInt32(dr["id_maquina"]),
                                    oMaquina = new Maquinas
                                    {
                                        id_maquina = Convert.ToInt32(dr["id_maquina"]),
                                        nombre_maquina = dr["nombre_maquina"].ToString()
                                    }
                                });
                            }
                        }
                    }
                }
            }
            catch
            {
                lista = new List<Rutina>();  // En caso de excepción, reinicia la lista
            }

            return lista;  // Devuelve la lista de rutinas
        }

        // Método para registrar una nueva rutina
        public bool Registrar_rutina(Rutina obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    oconexion.Open();

                    using (SqlTransaction transaction = oconexion.BeginTransaction())
                    {
                        try
                        {
                            // Preparar el comando para el procedimiento almacenado
                            SqlCommand cmd = new SqlCommand("Registrar_rutina", oconexion, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;

                            // Añadir parámetros al comando
                            cmd.Parameters.AddWithValue("@nombre_rutina", obj.nombre_rutina);
                            cmd.Parameters.AddWithValue("@duracion_rutina", obj.duracion_rutina);
                            cmd.Parameters.AddWithValue("@nombre_ejercicio", obj.oEjercicio.nombre_ejercicio);
                            cmd.Parameters.AddWithValue("@dias_ejercicio", obj.oEjercicio.dias_ejercicio);
                            cmd.Parameters.AddWithValue("@horas_ejercicio", obj.oEjercicio.horas_ejercicio);
                            cmd.Parameters.AddWithValue("@id_aprendiz", obj.oAprendiz.id_aprendiz);

                            // Parámetros de salida
                            SqlParameter outputMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500) { Direction = ParameterDirection.Output };
                            SqlParameter outputResultado = new SqlParameter("@Resultado", SqlDbType.Int) { Direction = ParameterDirection.Output };
                            cmd.Parameters.Add(outputMensaje);
                            cmd.Parameters.Add(outputResultado);

                            // Ejecutar el procedimiento almacenado
                            cmd.ExecuteNonQuery();

                            // Obtener los valores de los parámetros de salida
                            Mensaje = outputMensaje.Value.ToString();
                            int idRutina = (int)outputResultado.Value;

                            if (idRutina > 0)
                            {
                                // Inserta las máquinas asociadas a la rutina
                                foreach (var maquina in obj.oRutina_Maquina)
                                {
                                    SqlCommand cmdMaquina = new SqlCommand("INSERT INTO Rutina_Maquina (id_rutina, id_maquina) VALUES (@id_rutina, @id_maquina)", oconexion, transaction);
                                    cmdMaquina.Parameters.AddWithValue("@id_rutina", idRutina);
                                    cmdMaquina.Parameters.AddWithValue("@id_maquina", maquina.id_maquina);
                                    cmdMaquina.ExecuteNonQuery();
                                }

                                resultado = true;
                                Mensaje = "La rutina se ha registrado exitosamente.";
                            }

                            // Confirmar la transacción
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            // Revertir la transacción en caso de error
                            transaction.Rollback();
                            Mensaje = "Error durante la transacción: " + ex.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error de conexión: " + ex.Message;
            }

            return resultado;
        }

        // Método para elimianr una rutina
        public bool Eliminar_rutina(int id_rutina)
        {
            bool eliminacionExitosa = false; 
            try
            {
                using (SqlConnection connection = new SqlConnection(Conexion.cn))  // Crea una conexión utilizando la cadena de conexión de la clase Conexion
                {
                    connection.Open();  // Abre la conexión a la base de datos

                    SqlCommand command = new SqlCommand("Eliminar_rutina", connection);  // Crea un comando SQL para llamar al procedimiento almacenado Eliminar_rutina
                    command.CommandType = CommandType.StoredProcedure;  

                    command.Parameters.AddWithValue("@id_rutina", id_rutina);  // Añade parámetros al comando SQL

                    SqlParameter resultParam = new SqlParameter("@result", SqlDbType.Bit);  // Parámetro de salida para el resultado de la eliminación
                    resultParam.Direction = ParameterDirection.Output;  // Especifica que es un parámetro de salida
                    command.Parameters.Add(resultParam);  // Añade el parámetro de salida al comando SQL

                    // Ejecutar el procedimiento almacenado
                    command.ExecuteNonQuery();

                    // Obtener el valor del parámetro de salida
                    eliminacionExitosa = (bool)resultParam.Value;
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                Console.WriteLine("Error al intentar eliminar la rutina: " + ex.Message);
            }

            return eliminacionExitosa;  // Devuelve el resultado de la eliminación
        }
    }
}
