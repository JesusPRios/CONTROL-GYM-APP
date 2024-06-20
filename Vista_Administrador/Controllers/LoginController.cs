using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;
using Vista_Administrador.Models;
using Capa_Entidad;
using Capa_Negocio;
using System.Data;

namespace Vista_Administrador.Controllers
{
    public class LoginController : Controller
    {
        // Obtener la cadena de conexión desde web.config
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

        // Acción para mostrar la vista principal del login
        public ActionResult Index()
        {
            return View();
        }

        // Acción para mostrar la vista de login
        public ActionResult Login()
        {
            return View();
        }

        // Acción POST para manejar el proceso de login
        [HttpPost]
        public ActionResult Login(string id, string contraseña)
        {
            // Obtener la cadena de conexión desde web.config
            string connectionString = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

            // Abrir la conexión a la base de datos
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Verificar si el usuario ha proporcionado un ID y una contraseña
                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(contraseña))
                {
                    // Mostrar mensaje de error si falta algún dato
                    ViewBag.Error = "Debe llenar primero todos los datos";
                    return View();
                }
                else
                {
                    // Verificar si el usuario es un aprendiz
                    string query = "SELECT * FROM Aprendiz WHERE id_aprendiz = @id_aprendiz AND contraseña_aprendiz = @contraseña_aprendiz";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_aprendiz", id);
                    command.Parameters.AddWithValue("@contraseña_aprendiz", contraseña);

                    // Ejecutar la consulta y leer los resultados
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Leer el estado del aprendiz
                            string estado = reader["estado_aprendiz"].ToString();

                            // Verificar si el estado es Inactivo
                            if (estado == "Inactivo")
                            {
                                // Mostrar mensaje de error si el estado es Inactivo
                                ViewBag.Error = "Su cuenta está Inactiva.";
                                return View();
                            }
                            else
                            {
                                // Cerrar el lector actual
                                reader.Close();

                                // Verificar si el aprendiz tiene una suspensión activa
                                query = "SELECT COUNT(*) FROM Suspension WHERE id_aprendiz = @id_aprendiz AND fecha_fin_suspension >= GETDATE()";
                                command = new SqlCommand(query, connection);
                                command.Parameters.AddWithValue("@id_aprendiz", id);

                                // Ejecutar la consulta y leer el resultado
                                int suspensionCount = (int)command.ExecuteScalar();

                                // Verificar si el aprendiz tiene suspensiones activas
                                if (suspensionCount > 0)
                                {
                                    // Mostrar mensaje de error si hay suspensiones activas
                                    ViewBag.Error = "Su cuenta está suspendida.";
                                    return View();
                                }

                                // Usuario es un aprendiz sin suspensiones activas y con estado Activo
                                FormsAuthentication.SetAuthCookie(id, false); // Establecer la cookie de autenticación
                                Session["IdAprendiz"] = id; // Almacenar el id_aprendiz en la sesión
                                return RedirectToAction("Index", "Aprendiz");
                            }
                        }
                    }

                    // Verificar si el usuario es un administrador
                    query = "SELECT * FROM Administrador WHERE id_administrador = @id_administrador AND contraseña_administrador = @contraseña_administrador";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id_administrador", id);
                    command.Parameters.AddWithValue("@contraseña_administrador", contraseña);

                    // Ejecutar la consulta y leer los resultados
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Usuario es un administrador
                            FormsAuthentication.SetAuthCookie(id, false); // Establecer la cookie de autenticación
                            return RedirectToAction("Index", "Administrador");
                        }
                    }
                }
            }

            // Si no se encuentra ningún usuario con las credenciales proporcionadas, mostrar mensaje de error
            ViewBag.Error = "Identificación o contraseña incorrectos";
            return View();
        }

        // Acción para manejar el proceso de cerrar sesión
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut(); // Cerrar sesión
            return RedirectToAction("Login", "Login");
        }

        // Acción para mostrar la vista de registro
        public ActionResult Registro()
        {
            return View();
        }

        // Acción para mostrar la vista de restablecer contraseña
        public ActionResult ReestablecerContraseña()
        {
            return View();
        }

        // Acción POST para registrar un aprendiz
        [HttpPost]
        public JsonResult RegistrarAprendiz(Aprendiz objeto)
        {
            // Variable para almacenar el mensaje de resultado
            string Mensaje = string.Empty;

            // Llama al método de negocio para registrar el aprendiz y obtiene el resultado
            int resultado = new CN_Aprendiz_gym().RegistrarAprendiz(objeto, out Mensaje);

            // Verifica si el resultado es diferente de cero para determinar el éxito del registro
            if (resultado != 0)
            {
                // Retorna un JSON indicando que la operación fue exitosa
                return Json(new { oper_exitosa = true, mensaje = "Registro exitoso" });
            }
            else
            {
                // Retorna un JSON indicando que hubo un error durante el registro, junto con el mensaje de error
                return Json(new { oper_exitosa = false, mensaje = Mensaje });
            }
        }



        // Acción POST para consultar si un usuario es un aprendiz o un administrador para restablecer contraseña
        [HttpPost]
        public JsonResult ConsultarUsuarioReestablecer(int id)
        {
            // Obtener la cadena de conexión desde web.config
            string connectionString = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

            try
            {
                // Usar una conexión para interactuar con la base de datos
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Abrir la conexión a la base de datos

                    // Consultar si el usuario es un aprendiz
                    var query = "SELECT id_aprendiz FROM Aprendiz WHERE id_aprendiz = @id";
                    var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id); // Agregar parámetro @id con el valor recibido

                    // Ejecutar la consulta y leer los resultados
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Si hay resultados
                        {
                            // El usuario es un aprendiz
                            return Json(new { id = id, es_administrador = false });
                        }
                    }

                    // Consultar si el usuario es un administrador
                    query = "SELECT id_administrador FROM Administrador WHERE id_administrador = @id";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", id); // Agregar parámetro @id con el valor recibido

                    // Ejecutar la consulta y leer los resultados
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Si hay resultados
                        {
                            // El usuario es un administrador
                            return Json(new { id = id, es_administrador = true });
                        }
                    }

                    // Si no se encuentra el usuario, devolvemos un código de estado 404
                    Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                    return Json(new { error = "El usuario no existe en la base de datos." });
                }
            }
            catch
            {
                // Si hay un error, devolvemos un código de estado 500
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return Json(new { error = "Ocurrió un error al intentar consultar el usuario." });
            }
        }


        // Acción POST para actualizar la contraseña de un usuario
        [HttpPost]
        public JsonResult ActualizarContraseña(int id, string nueva_contraseña, bool es_administrador)
        {
            // Lógica para actualizar la contraseña
            string connectionString = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

            try
            {
                // Establece una conexión con la base de datos
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open(); // Abre la conexión

                    // Determina la consulta SQL según el tipo de usuario (administrador o aprendiz)
                    string query = es_administrador ?
                        "UPDATE Administrador SET contraseña_administrador = @nueva_contraseña WHERE id_administrador = @id" :
                        "UPDATE Aprendiz SET contraseña_aprendiz = @nueva_contraseña WHERE id_aprendiz = @id";

                    var command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nueva_contraseña", nueva_contraseña); // Parámetro: nueva contraseña
                    command.Parameters.AddWithValue("@id", id); // Parámetro: ID del usuario

                    // Ejecuta la consulta para actualizar la contraseña
                    command.ExecuteNonQuery();
                }

                // Retorna un JSON indicando que la operación fue exitosa
                return Json(new { oper_exitosa = true, mensaje = "Contraseña actualizada con éxito" });
            }
            catch (Exception ex)
            {
                // Maneja el error, si ocurre alguno
                Console.WriteLine(ex.Message);

                // Retorna un JSON indicando que hubo un error al actualizar la contraseña
                return Json(new { oper_exitosa = false, mensaje = "Error al actualizar la contraseña" });
            }
        }


        // Acción para mostrar la vista de validación de código
        public ActionResult Validar_codigo()
        {
            return View();
        }

        // Acción para mostrar la vista de registro de administrador
        public ActionResult Registro_administrador()
        {
            return View();
        }

        // Acción POST para registrar un administrador
        [HttpPost]
        public JsonResult Registrar_administrador(Administrador objeto)
        {
            // Variable para almacenar el mensaje de resultado
            string Mensaje = string.Empty;

            // Llama al método de negocio para registrar el administrador y obtiene el resultado
            int resultado = new CN_Administrador().Registrar_administrador(objeto, out Mensaje);

            // Verifica si el resultado es diferente de cero para determinar el éxito del registro
            if (resultado != 0)
            {
                // Retorna un JSON indicando que la operación fue exitosa
                return Json(new { oper_exitosa = true, mensaje = "Registro exitoso" });
            }
            else
            {
                // Retorna un JSON indicando que hubo un error durante el registro, junto con el mensaje de error
                return Json(new { oper_exitosa = false, mensaje = Mensaje });
            }
        }


        // Acción POST para validar un código de administrador
        [HttpPost]
        public ActionResult Validar_codigo(string codigo)
        {
            bool isValid = false;

            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("Validar_codigo_admin", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@codigo", codigo);

                        // Agregar parámetro de salida
                        var outputParameter = new SqlParameter
                        {
                            ParameterName = "@IsValid",
                            SqlDbType = SqlDbType.Bit,
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParameter);

                        // Ejecutar el comando
                        command.ExecuteNonQuery();

                        // Obtener el valor del parámetro de salida
                        isValid = (bool)outputParameter.Value;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Manejar errores específicos de SQL
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                return Json(new { isValid = false, error = "SQL error: " + sqlEx.Message });
            }
            catch (Exception ex)
            {
                // Manejar otros errores
                Console.WriteLine($"Error: {ex.Message}");
                return Json(new { isValid = false, error = ex.Message });
            }

            // Retorna un JSON indicando si el código es válido
            return Json(new { isValid = isValid });
        }


        // Acción para mostrar la vista de validación de rol
        public ActionResult Validar_rol()
        {
            return View();
        }
    }
}