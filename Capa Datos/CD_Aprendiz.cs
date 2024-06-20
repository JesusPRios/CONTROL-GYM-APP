using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Capa_Entidad;

namespace Capa_Datos
{
    public class CD_Aprendiz
    {
        //Configura la conexión a la base de datos que será utilizada por otras partes del código para realizar consultas y operaciones en la base de datos.
        private string connectionString = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        // Método para obtener un aprendiz por su ID
        public Aprendiz ObtenerPorId(string id_aprendiz)
        {
            Aprendiz aprendiz = null;

            try
            {
                // Abrir conexión a la base de datos utilizando using para garantizar la disposición correcta
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();  // Abrir la conexión

                    // Definir la consulta SQL para seleccionar un aprendiz por su ID
                    string query = "SELECT * FROM Aprendiz WHERE id_aprendiz = @id_aprendiz";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_aprendiz", id_aprendiz);  // Parámetro de la consulta

                    // Ejecutar la consulta SELECT y leer los resultados utilizando SqlDataReader
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Crear objeto Aprendiz y asignar valores desde SqlDataReader
                            aprendiz = new Aprendiz
                            {
                                id_aprendiz = Convert.ToInt32(reader["id_aprendiz"]),
                                nombre_aprendiz = reader["nombre_aprendiz"].ToString(),
                                tipo_id_aprendiz = reader["tipo_id_aprendiz"].ToString(),
                                correo_aprendiz = reader["correo_aprendiz"].ToString(),
                                telefono_aprendiz = Convert.ToInt64(reader["telefono_aprendiz"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades
                Console.WriteLine("Error al obtener aprendiz por ID: " + ex.Message);
            }

            return aprendiz;  // Devolver el aprendiz encontrado o null si no se encontró ninguno
        }


        public List<Aprendiz> Listar()
        {
            List<Aprendiz> lista = new List<Aprendiz>();  // Lista para almacenar los aprendices obtenidos de la base de datos

            try
            {
                // Establecer conexión a la base de datos
                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    // Definir la consulta SQL para obtener los datos de los aprendices y sus fichas
                    string query = "SELECT Aprendiz.id_aprendiz, Aprendiz.tipo_id_aprendiz, Aprendiz.nombre_aprendiz, Aprendiz.telefono_aprendiz, " +
                                   "Aprendiz.estado_aprendiz, Aprendiz.correo_aprendiz, Ficha.numero_ficha, Ficha.fecha_inicio_ficha, " +
                                   "Ficha.fecha_fin_ficha, Programa_formacion.nombre_programa " +
                                   "FROM Aprendiz " +
                                   "INNER JOIN Ficha ON Aprendiz.numero_ficha = Ficha.numero_ficha " +
                                   "INNER JOIN Programa_formacion ON Ficha.id_programa = Programa_formacion.id_programa";

                    SqlCommand cmd = new SqlCommand(query, oconexion);  // Crear comando SQL con la consulta y la conexión
                    cmd.CommandType = CommandType.Text;  // Tipo de comando es de texto
                    oconexion.Open();  // Abrir la conexión a la base de datos

                    // Ejecutar la consulta SELECT y leer los resultados utilizando SqlDataReader
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        // Iterar sobre cada fila de resultados
                        while (dr.Read())
                        {
                            // Crear un nuevo objeto Aprendiz y asignar los valores desde SqlDataReader
                            lista.Add(
                                new Aprendiz()
                                {
                                    id_aprendiz = Convert.ToInt32(dr["id_aprendiz"]),  
                            tipo_id_aprendiz = dr["tipo_id_aprendiz"].ToString(),  
                            nombre_aprendiz = dr["nombre_aprendiz"].ToString(), 
                            telefono_aprendiz = Convert.ToInt64(dr["telefono_aprendiz"]),  
                            estado_aprendiz = dr["estado_aprendiz"].ToString(),  
                            correo_aprendiz = dr["correo_aprendiz"].ToString(),  
                            oFicha = new Ficha  
                            {
                                        numero_ficha = Convert.ToInt32(dr["numero_ficha"]),  
                                fecha_inicio_ficha = Convert.ToDateTime(dr["fecha_inicio_ficha"]),  
                                fecha_fin_ficha = Convert.ToDateTime(dr["fecha_fin_ficha"]),  
                                oPrograma_Form = new Programa_form  
                                {
                                            nombre_programa = dr["nombre_programa"].ToString() 
                                }
                                    }
                                }
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción según tus necesidades
                Console.WriteLine("Error al listar aprendices: " + ex.Message);
            }

            return lista;  // Devolver la lista de aprendices
        }



        public int RegistrarAprendiz(Aprendiz obj, out string Mensaje)
        {
            Mensaje = string.Empty;  // Inicializar el mensaje de salida
            int resultado = 0;  // Inicializar el resultado del registro

            try
            {
                // Establecer conexión a la base de datos utilizando using para garantizar la disposición correcta
                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    oconexion.Open();  // Abrir la conexión a la base de datos

                    // Crear un comando SQL para ejecutar el procedimiento almacenado "RegistrarAprendiz"
                    SqlCommand cmd = new SqlCommand("RegistrarAprendiz", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;  
                    cmd.Parameters.AddWithValue("@id_aprendiz", obj.id_aprendiz);  
                    cmd.Parameters.AddWithValue("@tipo_id_aprendiz", obj.tipo_id_aprendiz);
                    cmd.Parameters.AddWithValue("@nombre_aprendiz", obj.nombre_aprendiz); 
                    cmd.Parameters.AddWithValue("@telefono_aprendiz", obj.telefono_aprendiz);  
                    cmd.Parameters.AddWithValue("@contraseña_aprendiz", obj.contraseña_aprendiz); 
                    cmd.Parameters.AddWithValue("@correo_aprendiz", obj.correo_aprendiz);  
                    cmd.Parameters.AddWithValue("@numero_ficha", obj.oFicha.numero_ficha);  
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output; 
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output; 

                    cmd.ExecuteNonQuery();  // Ejecutar el procedimiento almacenado

                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);  // Obtener el resultado de la ejecución
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();  // Obtener el mensaje de salida
                }
            }
            catch (Exception ex)
            {
                Mensaje = "Error al intentar registrar aprendiz: " + ex.Message;  // Capturar y manejar la excepción
                Console.WriteLine(Mensaje);  // Mostrar mensaje de error en la consola
            }

            return resultado;  // Devolver el resultado del registro
        }


        public Aprendiz ConsultarAprendiz(int id_aprendiz)
        {
            Aprendiz aprendiz = null;  // Inicializa el objeto Aprendiz que se va a consultar

            try
            {
                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    oconexion.Open();  // Abre la conexión a la base de datos

                    // Consulta SQL para obtener los datos del aprendiz y su ficha de formación
                    string query = "SELECT Aprendiz.id_aprendiz, Aprendiz.tipo_id_aprendiz, Aprendiz.nombre_aprendiz, " +
                                   "Aprendiz.telefono_aprendiz, Aprendiz.estado_aprendiz, Ficha.numero_ficha, " +
                                   "Programa_formacion.nombre_programa " +
                                   "FROM Aprendiz " +
                                   "INNER JOIN Ficha ON Aprendiz.numero_ficha = Ficha.numero_ficha " +
                                   "INNER JOIN Programa_formacion ON Ficha.id_programa = Programa_formacion.id_programa " +
                                   "WHERE Aprendiz.id_aprendiz = @id_aprendiz";  // Filtra por el ID del aprendiz

                    SqlCommand cmd = new SqlCommand(query, oconexion);  // Crea un comando SQL con la consulta y la conexión
                    cmd.Parameters.AddWithValue("@id_aprendiz", id_aprendiz);  // Asigna el valor del parámetro @id_aprendiz

                    // Ejecuta la consulta y lee los resultados utilizando SqlDataReader
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // Crea un nuevo objeto Aprendiz y asigna los valores desde SqlDataReader
                            aprendiz = new Aprendiz
                            {
                                id_aprendiz = Convert.ToInt32(dr["id_aprendiz"]), 
                                tipo_id_aprendiz = dr["tipo_id_aprendiz"].ToString(), 
                                nombre_aprendiz = dr["nombre_aprendiz"].ToString(), 
                                telefono_aprendiz = Convert.ToInt64(dr["telefono_aprendiz"]),  
                                estado_aprendiz = dr["estado_aprendiz"].ToString(),  
                                oFicha = new Ficha  
                                {
                                    numero_ficha = Convert.ToInt32(dr["numero_ficha"]), 
                                    oPrograma_Form = new Programa_form 
                                    {
                                        nombre_programa = dr["nombre_programa"].ToString()
                                    }
                                }
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Captura y maneja la excepción según tus necesidades
                Console.WriteLine("Error al consultar aprendiz por ID: " + ex.Message);
            }

            return aprendiz;  // Devuelve el objeto Aprendiz consultado, o null si no se encontró
        }



        public Aprendiz ConsultarAprendizReestablecer(int id_aprendiz)
        {
            Aprendiz aprendiz = null;  // Inicializa el objeto Aprendiz que se va a consultar

            try
            {
                using (SqlConnection oconexion = new SqlConnection(connectionString))
                {
                    oconexion.Open();  // Abre la conexión a la base de datos

                    // Consulta SQL para obtener los datos del aprendiz y su ficha de formación
                    string query = "SELECT Aprendiz.id_aprendiz, Aprendiz.tipo_id_aprendiz, Aprendiz.nombre_aprendiz, " +
                                   "Aprendiz.telefono_aprendiz, Aprendiz.estado_aprendiz, Ficha.numero_ficha, " +
                                   "Programa_formacion.nombre_programa " +
                                   "FROM Aprendiz " +
                                   "INNER JOIN Ficha ON Aprendiz.numero_ficha = Ficha.numero_ficha " +
                                   "INNER JOIN Programa_formacion ON Ficha.id_programa = Programa_formacion.id_programa " +
                                   "WHERE Aprendiz.id_aprendiz = @id_aprendiz";  

                    SqlCommand cmd = new SqlCommand(query, oconexion);  // Crea un comando SQL con la consulta y la conexión
                    cmd.Parameters.AddWithValue("@id_aprendiz", id_aprendiz);  

                    // Ejecuta la consulta y lee los resultados utilizando SqlDataReader
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            // Crea un nuevo objeto Aprendiz y asigna los valores desde SqlDataReader
                            aprendiz = new Aprendiz
                            {
                                id_aprendiz = Convert.ToInt32(dr["id_aprendiz"]),  
                                tipo_id_aprendiz = dr["tipo_id_aprendiz"].ToString(),  
                                nombre_aprendiz = dr["nombre_aprendiz"].ToString(),  
                                telefono_aprendiz = Convert.ToInt64(dr["telefono_aprendiz"]), 
                                estado_aprendiz = dr["estado_aprendiz"].ToString(), 
                                oFicha = new Ficha  // Objeto Ficha asociado al aprendiz
                                {
                                    numero_ficha = Convert.ToInt32(dr["numero_ficha"]),  
                                    oPrograma_Form = new Programa_form  
                                    {
                                        nombre_programa = dr["nombre_programa"].ToString()  
                                    }
                                }
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Aquí podrías registrar o manejar la excepción según tus necesidades
                Console.WriteLine("Error al consultar aprendiz por ID: " + ex.Message);
            }

            return aprendiz;  // Devuelve el objeto Aprendiz consultado, o null si no se encontró
        }
    }
}
