using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Capa_Entidad;
using Capa_Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Vista_Administrador.Controllers
{
    [Authorize]
    public class AprendizController : Controller
    {

        // Acción para mostrar la vista principal del administrador
        public ActionResult Index()
        {
            return View();
        }

        // Acción para mostrar la vista de rutinas
        public ActionResult Rutina()
        {
            return View();
        }


        public ActionResult CrearRutinas()
        {
            // Verificar si hay una sesión activa para el Id del Aprendiz
            if (Session["IdAprendiz"] != null)
            {
                // Asignar el Id del Aprendiz a la ViewBag para usar en la vista
                ViewBag.IdAprendiz = Session["IdAprendiz"];
            }
            else
            {
                // Redirigir al usuario a la página de inicio de sesión si no hay sesión activa
                return RedirectToAction("Login", "Login");
            }

            // Crear una instancia de CN_Maquinas para listar las máquinas
            CN_Maquinas cnMaquinas = new CN_Maquinas();
            List<Maquinas> maquinas = cnMaquinas.Listar();

            // Asignar la lista de máquinas a ViewBag para usar en la vista
            ViewBag.Maquinas = maquinas;

            // Retornar la vista para crear rutinas
            return View();
        }


        // Acción para mostrar la vista de modificar rutinas
        public ActionResult ModificarRutinas()
        {
            return View();
        }

        // Acción para mostrar la vista de eliminar rutinas
        public ActionResult EliminarRutinas()
        {
            return View();
        }

        // Acción para mostrar la vista de modificar información
        public ActionResult ModificarInformacion()
        {
            return View();
        }


        public ActionResult EditarRutina()
        {
            //Se hace llamado al método de listar máquinas para pasarlo a la vista de crear rutinas
            CN_Maquinas cnMaquinas = new CN_Maquinas();
            List<Maquinas> maquinas = cnMaquinas.Listar();
            ViewBag.Maquinas = maquinas;
            return View();
        }

        // Acción POST para mostrar la información del aprendiz autenticado
        [HttpPost]
        public ActionResult MostrarInfo()
        {
            // Inicialización de variables
            List<Aprendiz> olista = new List<Aprendiz>();
            string mensaje = string.Empty;
            bool operacionExitosa = true;

            try
            {
                // Obtener el nombre de usuario del aprendiz autenticado
                string idAprendiz = User.Identity.Name;

                // Obtener los datos del aprendiz actual
                Aprendiz aprendizgym = new CN_Aprendiz_gym().ObtenerPorId(idAprendiz);

                if (aprendizgym != null)
                {
                    // Devolver los datos del aprendiz
                    return Json(aprendizgym, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Si no se encuentra el aprendiz, se marca la operación como fallida
                    mensaje = "No se encontró el aprendiz autenticado";
                    operacionExitosa = false;
                }
            }
            catch (Exception ex)
            {
                // En caso de error, se captura y se devuelve un mensaje de error
                mensaje = "Error al obtener los datos del aprendiz: " + ex.Message;
                operacionExitosa = false;
            }

            // Devolver una respuesta JSON con el resultado de la operación
            return Json(new { operacionExitosa, mensaje, data = olista }, JsonRequestBehavior.AllowGet);
        }


        // Acción POST para editar la información de un aprendiz
        [HttpPost]
        public ActionResult EditarInfoAprendiz(int id_aprendiz, string nombre_aprendiz, string telefono_aprendiz, string correo_aprendiz)
        {
            try
            {
                // Establecer la conexión con la base de datos
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ToString()))
                {
                    // Definir el comando para llamar al procedimiento almacenado "EditarInfoAprendiz"
                    SqlCommand cmd = new SqlCommand("EditarInfoAprendiz", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Agregar los parámetros necesarios para el procedimiento almacenado
                    cmd.Parameters.AddWithValue("@id_aprendiz", id_aprendiz);
                    cmd.Parameters.AddWithValue("@nombre_aprendiz", nombre_aprendiz);
                    cmd.Parameters.AddWithValue("@telefono_aprendiz", telefono_aprendiz);
                    cmd.Parameters.AddWithValue("@correo_aprendiz", correo_aprendiz);

                    // Abrir la conexión con la base de datos
                    con.Open();

                    // Ejecutar el comando para actualizar la información del aprendiz
                    cmd.ExecuteNonQuery();

                    // Cerrar la conexión con la base de datos
                    con.Close();
                }

                // Devolver una respuesta JSON indicando que la operación fue exitosa
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // En caso de error, devolver una respuesta JSON indicando que la operación falló y el mensaje de error
                return Json(new { success = false, error = ex.Message });
            }
        }


        // Acción POST para guardar una rutina
        [HttpPost]
        public JsonResult Guardar_rutina(Rutina objeto, List<int> maquinasSeleccionadas)
        {
            int resultado = 0;
            string Mensaje = string.Empty;
            string connectionString = ConfigurationManager.ConnectionStrings["cadena"].ToString();

            // Verificar si se han seleccionado máquinas para la rutina
            if (maquinasSeleccionadas != null && maquinasSeleccionadas.Count > 0)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Consultar las máquinas seleccionadas para la rutina
                    string query = "SELECT id_maquina, nombre_maquina FROM Maquinas WHERE id_maquina IN (" + string.Join(",", maquinasSeleccionadas) + ")";
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    var maquinas = new List<Maquinas>();
                    while (reader.Read())
                    {
                        maquinas.Add(new Maquinas
                        {
                            id_maquina = reader.GetInt32(0),
                            nombre_maquina = reader.GetString(1)
                        });
                    }

                    // Asignar las máquinas seleccionadas a la rutina
                    objeto.oRutina_Maquina = maquinasSeleccionadas.Select(id => new Rutina_Maquina
                    {
                        id_rutina = objeto.id_rutina,
                        id_maquina = id,
                        oMaquina = maquinas.FirstOrDefault(m => m.id_maquina == id)
                    }).ToList();
                }
            }
            else
            {
                objeto.oRutina_Maquina = new List<Rutina_Maquina>();
            }

            // Registrar la rutina si es un nuevo registro
            if (objeto.id_rutina == 0)
            {
                resultado = new CN_Rutina().Registrar_rutina(objeto, out Mensaje) ? 1 : 0; // Si el registro es exitoso, resultado es 1, de lo contrario, será 0
            }

            // Devolver una respuesta JSON con el resultado de la operación
            return Json(new { resultado = resultado, Mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }


        // Acción GET para listar las rutinas del aprendiz
        [HttpGet]
        public JsonResult Listar_rutinas()
        {
            List<Rutina> olista = new List<Rutina>();

            // Verificar si existe una sesión activa de aprendiz
            if (Session["IdAprendiz"] != null)
            {
                int id_aprendiz = Convert.ToInt32(Session["IdAprendiz"]);
                olista = new CN_Rutina().Listar(id_aprendiz);
            }

            // Devolver una respuesta JSON con las rutinas obtenidas
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        // Acción POST para eliminar una rutina
        [HttpPost]
        public JsonResult Elimina_rutina(int id_rutina)
        {
            bool eliminacionExitosa = false;
            string mensajeError = string.Empty;

            try
            {
                // Llamar al método para eliminar la rutina
                eliminacionExitosa = new CN_Rutina().Eliminar_rutina(id_rutina);
            }
            catch (Exception ex)
            {
                mensajeError = ex.Message;
            }

            // Devolver una respuesta JSON con el resultado de la eliminación
            return Json(new
            {
                success = eliminacionExitosa,
                error = eliminacionExitosa ? null : mensajeError
            });
        }


        // Acción POST para editar una rutina
        [HttpPost]
        public JsonResult Editar_rutina(int idRutina, string nombreRutina, string nombreEjercicio, int diasEjercicio, int horasEjercicio, string duracionRutina, List<int> idMaquinas)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ToString()))
                {
                    // Comando para llamar al procedimiento almacenado "Editar_rutina"
                    SqlCommand command = new SqlCommand("Editar_rutina", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id_rutina", idRutina);
                    command.Parameters.AddWithValue("@nombre_rutina", nombreRutina);
                    command.Parameters.AddWithValue("@nombre_ejercicio", nombreEjercicio);
                    command.Parameters.AddWithValue("@dias_ejercicio", diasEjercicio);
                    command.Parameters.AddWithValue("@horas_ejercicio", horasEjercicio);
                    command.Parameters.AddWithValue("@duracion_rutina", duracionRutina);

                    con.Open();

                    // Ejecutar el comando para editar la rutina
                    command.ExecuteNonQuery();

                    // Eliminar las relaciones existentes de Rutina_Maquina
                    SqlCommand deleteCommand = new SqlCommand("DELETE FROM Rutina_Maquina WHERE id_rutina = @id_rutina", con);
                    deleteCommand.Parameters.AddWithValue("@id_rutina", idRutina);
                    deleteCommand.ExecuteNonQuery();

                    // Insertar las nuevas relaciones en Rutina_Maquina
                    if (idMaquinas != null)
                    {
                        foreach (var idMaquina in idMaquinas)
                        {
                            SqlCommand insertCommand = new SqlCommand("INSERT INTO Rutina_Maquina (id_rutina, id_maquina) VALUES (@id_rutina, @id_maquina)", con);
                            insertCommand.Parameters.AddWithValue("@id_rutina", idRutina);
                            insertCommand.Parameters.AddWithValue("@id_maquina", idMaquina);
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    // Cerrar la conexión con la base de datos
                    con.Close();
                }

                // Devolver una respuesta JSON indicando que la operación fue exitosa
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // En caso de error, devolver una respuesta JSON indicando que la operación falló y el mensaje de error
                System.Diagnostics.Debug.WriteLine("Error en Editar_rutina: " + ex.ToString());
                return Json(new { success = false, error = ex.Message });
            }
        }

        public ActionResult Asistencia()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Registrar_ingreso()
        {
            try
            {
                // Obtener el ID del aprendiz desde la sesión
                var idAprendiz = Session["IdAprendiz"];
                if (idAprendiz == null)
                {
                    throw new Exception("No se encontró el ID del aprendiz en la sesión.");
                }

                // Obtener la cadena de conexión desde la configuración
                string connectionString = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

                // Abrir una conexión SQL usando la cadena de conexión especificada
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Abrir la conexión a la base de datos
                    connection.Open();

                    // Definir la consulta SQL para insertar un nuevo registro de asistencia
                    string query = "INSERT INTO Asistencia (id_aprendiz, fecha_ingreso) VALUES (@id_aprendiz, @fecha_ingreso)";

                    // Crear un comando SQL para ejecutar la consulta dentro de la conexión abierta
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Asignar los parámetros necesarios para la consulta (ID del aprendiz y fecha de ingreso actual)
                        command.Parameters.AddWithValue("@id_aprendiz", idAprendiz);
                        command.Parameters.AddWithValue("@fecha_ingreso", DateTime.Now);

                        // Ejecutar la consulta que inserta un nuevo registro de asistencia
                        command.ExecuteNonQuery();
                    }
                }

                // Si todo ha ido bien, retornar un objeto JSON con éxito verdadero
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Si ocurre algún error durante el proceso, retornar un objeto JSON con éxito falso y el mensaje de error
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult Registrar_salida()
        {
            try
            {
                // Obtener el ID del aprendiz desde la sesión
                var idAprendiz = Session["IdAprendiz"];
                if (idAprendiz == null)
                {
                    throw new Exception("No se encontró el ID del aprendiz en la sesión.");
                }

                // Obtener la cadena de conexión desde la configuración
                string connectionString = ConfigurationManager.ConnectionStrings["cadena"].ConnectionString;

                // Abrir una conexión SQL usando la cadena de conexión especificada
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Abrir la conexión a la base de datos
                    connection.Open();

                    // Definir la consulta SQL para actualizar el registro de asistencia con fecha de salida
                    string query = "UPDATE Asistencia SET fecha_salida = @fecha_salida WHERE id_aprendiz = @id_aprendiz AND fecha_salida IS NULL";

                    // Crear un comando SQL para ejecutar la consulta dentro de la conexión abierta
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Asignar los parámetros necesarios para la consulta (ID del aprendiz y fecha de salida actual)
                        command.Parameters.AddWithValue("@id_aprendiz", idAprendiz);
                        command.Parameters.AddWithValue("@fecha_salida", DateTime.Now);

                        // Ejecutar la consulta que actualiza el registro de asistencia con la fecha de salida
                        int rowsAffected = command.ExecuteNonQuery();

                        // Verificar si se actualizó alguna fila (si no se actualiza ninguna, no había registro de ingreso sin salida)
                        if (rowsAffected == 0)
                        {
                            throw new Exception("No se encontró un registro de ingreso sin salida.");
                        }
                    }
                }

                // Si todo ha ido bien, retornar un objeto JSON con éxito verdadero
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Si ocurre algún error durante el proceso, retornar un objeto JSON con éxito falso y el mensaje de error
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}