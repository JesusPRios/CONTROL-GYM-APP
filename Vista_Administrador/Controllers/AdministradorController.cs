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
using Newtonsoft.Json;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace Vista_Administrador.Controllers
{
    [Authorize]
    public class AdministradorController : Controller
    {

        // Acción para mostrar la vista principal del administrador
        public ActionResult Index()
        {
            return View();
        }

        // Acción para mostrar la vista de información personal del administrador
        public ActionResult Info_personal()
        {
            return View();
        }

        // Acción para mostrar la vista de lista de aprendices
        public ActionResult Aprendices()
        {
            return View();
        }

        // Acción POST para mostrar la información del administrador autenticado
        [HttpPost]
        public ActionResult MostrarInfo()
        {
            string mensaje = string.Empty;
            bool operacionExitosa = true;

            try
            {
                // Obtener el nombre de usuario del administrador autenticado
                string idAdmin = User.Identity.Name;

                // Obtener los datos del administrador actual usando la capa de negocio (CN_Administrador)
                Administrador administrador = new CN_Administrador().ObtenerPorId(idAdmin);

                // Verificar si se encontró al administrador
                if (administrador != null)
                {
                    // Devolver los datos del administrador como JSON
                    return Json(administrador, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    // Mensaje si no se encontró el administrador
                    mensaje = "No se encontró el administrador autenticado";
                    operacionExitosa = false;
                }
            }
            catch (Exception ex)
            {
                // Manejar errores si ocurre una excepción al obtener los datos del administrador
                mensaje = "Error al obtener los datos del administrador: " + ex.Message;
                operacionExitosa = false;
            }

            // Devolver un JSON con el resultado de la operación (éxito o fallo) y el mensaje correspondiente
            return Json(new { operacionExitosa, mensaje }, JsonRequestBehavior.AllowGet);
        }


        // Acción GET para listar los aprendices
        [HttpGet]
        public JsonResult Listar_aprendices()
        {
            List<Aprendiz> olista = new List<Aprendiz>();
            olista = new CN_Aprendiz_gym().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        // Acción para mostrar la vista de máquinas
        public ActionResult Maquinas()
        {
            return View();
        }

        // Acción GET para listar las máquinas
        [HttpGet]
        public JsonResult Listar_maquinas()
        {
            List<Maquinas> olista = new List<Maquinas>();
            olista = new CN_Maquinas().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        // Acción para mostrar la vista de sanciones
        public ActionResult Sancion()
        {
            return View();
        }

        // Acción GET para listar sanciones
        [HttpGet]
        public ActionResult Listar_sanciones()
        {
            // Crear una lista para almacenar las sanciones
            List<Sancion> olista = new List<Sancion>();

            // Obtener la lista de sanciones desde la capa de negocios
            olista = new CN_Sancion().Listar();

            // Devolver un JSON con los datos de las sanciones
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        // Acción GET para listar suspensiones
        public JsonResult Listar_suspensiones()
        {
            // Crear una lista para almacenar las suspensiones
            List<Suspension> olista = new List<Suspension>();

            // Obtener la lista de suspensiones desde la capa de negocios
            olista = new CN_Suspension().Listar();

            // Devolver un JSON con los datos de las suspensiones
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        // Acción POST para consultar un aprendiz por su ID
        [HttpPost]
        public JsonResult ConsultarAprendiz(int id_aprendiz)
        {
            // Consultar el aprendiz utilizando la capa de negocios
            Aprendiz aprendiz = new CN_Aprendiz_gym().ConsultarAprendiz(id_aprendiz);

            // Devolver un JSON con los datos del aprendiz consultado
            return Json(aprendiz, JsonRequestBehavior.AllowGet);
        }



        // Acción POST para registrar un aprendiz
        [HttpPost]
        public JsonResult RegistrarAprendiz(Aprendiz objeto)
        {
            // Mensaje para devolver en caso de error
            string Mensaje = string.Empty;

            // Instancia de la capa de negocios para registrar el aprendiz
            int resultado = new CN_Aprendiz_gym().RegistrarAprendiz(objeto, out Mensaje);

            // Verificar el resultado del registro
            if (resultado != 0)
            {
                // Registro exitoso, devolver JSON con éxito y mensaje
                return Json(new { oper_exitosa = true, mensaje = "Registro exitoso" });
            }
            else
            {
                // Error durante el registro, devolver JSON con operación no exitosa y mensaje de error
                return Json(new { oper_exitosa = false, mensaje = Mensaje });
            }
        }


        // Acción POST para editar el estado de un aprendiz
        [HttpPost]
        public ActionResult Editar_estado_aprendiz(int id_aprendiz, string estado_aprendiz)
        {
            try
            {
                // Establecer la conexión utilizando la cadena de conexión del archivo de configuración web.config
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ToString()))
                {
                    // Crear el comando para llamar al procedimiento almacenado "Editar_aprendiz"
                    SqlCommand command = new SqlCommand("Editar_aprendiz", con);
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros necesarios para el procedimiento almacenado
                    command.Parameters.AddWithValue("@id_aprendiz", id_aprendiz);
                    command.Parameters.AddWithValue("@estado_aprendiz", estado_aprendiz);

                    // Abrir la conexión con la base de datos
                    con.Open();

                    // Ejecutar el comando para actualizar el estado del aprendiz
                    command.ExecuteNonQuery();

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


        // Acción para mostrar la vista de suspensiones
        public ActionResult Suspension()
        {
            return View();
        }

        // Acción para mostrar la vista de registro de suspensiones
        public ActionResult Reg_suspension()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Guardar_suspension(Suspension objeto)
        {
            // Inicializar variables para almacenar el resultado y el mensaje de la operación
            object resultado = 0;
            string Mensaje = string.Empty;

            // Verificar si el ID de la suspensión es igual a cero, lo que indica que es una nueva suspensión por registrar
            if (objeto.id_suspension == 0)
            {
                // Si es una nueva suspensión, llamar al método para registrarla en la capa de negocio y obtener el resultado y mensaje
                resultado = new CN_Suspension().Registrar_suspension(objeto, out Mensaje);
            }

            // Devolver una respuesta JSON con el resultado y el mensaje de la operación
            return Json(new { Resultado = resultado, Mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        public void VerificarSuspensiones()
        {
            // Cadena de conexión
            string connectionString = ConfigurationManager.ConnectionStrings["cadena"].ToString();

            // Consulta para seleccionar las suspensiones activas
            string selectQuery = "SELECT id_suspension, fecha_fin_suspension FROM Suspension WHERE estado_suspension = @estado_suspension";

            // Consulta para actualizar el estado de la suspensión
            string updateQuery = "UPDATE Suspension SET estado_suspension = @estado_suspension WHERE id_suspension = @id_suspension";

            // Fecha y hora actual
            DateTime now = DateTime.Now;

            // Ejecutar la consulta de selección
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand selectCommand = new SqlCommand(selectQuery, connection))
            {
                selectCommand.Parameters.AddWithValue("@estado_suspension", "Activo");

                connection.Open();

                using (SqlDataReader reader = selectCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idSuspension = reader.GetInt32(reader.GetOrdinal("id_suspension"));
                        DateTime fechaFinSuspension = reader.GetDateTime(reader.GetOrdinal("fecha_fin_suspension"));

                        // Verificar si la fecha de fin de la suspensión ha pasado
                        if (fechaFinSuspension <= now)
                        {
                            // Ejecutar la consulta de actualización
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                            {
                                updateCommand.Parameters.AddWithValue("@estado_suspension", "Inactivo");
                                updateCommand.Parameters.AddWithValue("@id_suspension", idSuspension);

                                updateCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        // Acción HTTP POST para editar una suspensión en la base de datos
        [HttpPost]
        public ActionResult EditarSuspension(int id_suspension, int id_aprendiz, string descripcion_suspension, string nombre_aprendiz, string fecha_inicio_suspension, string fecha_fin_suspension, string duracion_suspension, string estado_suspension)
        {
            try
            {

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ToString()))
                {
                    SqlCommand command = new SqlCommand("Editar_suspension", con);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@id_suspension", id_suspension);
                    command.Parameters.AddWithValue("@id_aprendiz", id_aprendiz);
                    command.Parameters.AddWithValue("@nombre_aprendiz", nombre_aprendiz);
                    command.Parameters.AddWithValue("@fecha_inicio_suspension", fecha_inicio_suspension);
                    command.Parameters.AddWithValue("@descripcion_suspension", descripcion_suspension);
                    command.Parameters.AddWithValue("@fecha_fin_suspension", fecha_fin_suspension);
                    command.Parameters.AddWithValue("@duracion_suspension", duracion_suspension);
                    command.Parameters.AddWithValue("@estado_suspension", estado_suspension);

                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                return Json(new { success = false, error = ex.Message });
            }
        }

        // Este método se encarga de eliminar una suspensión de la base de datos.
        [HttpPost]
        public ActionResult Eliminar_suspension(int id_suspension)
        {
            try
            {
                // Se crea una conexión a la base de datos utilizando la cadena de conexión especificada en la configuración.
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ToString()))
                {
                    // Se crea un objeto SqlCommand que representa la ejecución de un comando SQL en la base de datos.
                    SqlCommand cmd = new SqlCommand("Eliminar_suspension", con);

                    // Se especifica que el tipo de comando es un procedimiento almacenado.
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Se agrega un parámetro al comando que representa el identificador único de la suspensión que se desea eliminar.
                    cmd.Parameters.AddWithValue("@id_suspension", id_suspension);

                    // Se abre la conexión a la base de datos antes de ejecutar el comando.
                    con.Open();

                    // Se ejecuta el comando en la base de datos para llevar a cabo la eliminación de la suspensión.
                    cmd.ExecuteNonQuery();

                    // Se cierra la conexión después de completar la operación.
                    con.Close();
                }

                // Se devuelve una respuesta JSON que indica que la operación de eliminación fue exitosa.
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Si se produce un error durante la ejecución, se captura en el bloque catch para manejarlo adecuadamente.
                // Se devuelve un objeto JSON con el mensaje de error correspondiente.
                return Json(new { success = false, error = ex.Message });
            }
        }

        // Acción para mostrar la vista de registro de máquinas
        public ActionResult Reg_maquinas()
        {
            return View();
        }

        // Acción POST para guardar una máquina
        [HttpPost]
        public JsonResult GuardarMaquina(Maquinas maquina)
        {
            string mensaje = string.Empty;
            bool operacionExitosa = true;

            // Verificar si el objeto maquina es nulo
            if (maquina == null)
            {
                return Json(new { oper_exitosa = false, mensaje = "El objeto Maquinas es nulo" });
            }

            try
            {
                // Verificar si alguna propiedad requerida de maquina es nula o vacía
                if (string.IsNullOrEmpty(maquina.nombre_maquina) || string.IsNullOrEmpty(maquina.tipo_maquina) || string.IsNullOrEmpty(maquina.estado_maquina) || maquina.cantidad_maquinas <= 0)
                {
                    return Json(new { oper_exitosa = false, mensaje = "El objeto Maquinas tiene propiedades nulas o vacías" });
                }

                // Llamar al método de la capa de negocio para registrar la máquina
                int idMaquina = new CN_Maquinas().Registrar(maquina, out mensaje);

                // Verificar si el registro fue exitoso
                if (idMaquina != 0)
                {
                    maquina.id_maquina = idMaquina;
                }
                else
                {
                    operacionExitosa = false;
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return Json(new { oper_exitosa = false, mensaje = ex.Message });
            }

            // Devolver un JSON indicando el resultado de la operación
            return Json(new { oper_exitosa = operacionExitosa, id_maquina = maquina != null ? maquina.id_maquina : 0, mensaje = mensaje });
        }



        // Acción POST para actualizar la información de una máquina
        [HttpPost]
        public JsonResult ActualizarMaquina(int id_maquina, string nombre_maquina, string tipo_maquina, string estado_maquina, int cantidad_maquinas)
        {
            try
            {
                // Establecer la conexión utilizando la cadena de conexión del archivo de configuración
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ToString()))
                {
                    // Crear un comando para llamar al procedimiento almacenado "ActualizarMaquina"
                    SqlCommand cmd = new SqlCommand("ActualizarMaquina", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Agregar parámetros al comando
                    cmd.Parameters.AddWithValue("@id_maquina", id_maquina);
                    cmd.Parameters.AddWithValue("@nombre_maquina", nombre_maquina);
                    cmd.Parameters.AddWithValue("@tipo_maquina", tipo_maquina);
                    cmd.Parameters.AddWithValue("@estado_maquina", estado_maquina);
                    cmd.Parameters.AddWithValue("@cantidad_maquinas", cantidad_maquinas);

                    // Abrir la conexión
                    con.Open();

                    // Ejecutar el comando sin esperar un resultado
                    cmd.ExecuteNonQuery();
                }

                // Si todo ha ido bien, devolver un JSON indicando éxito
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // En caso de error, manejar la excepción y devolver un JSON con éxito false y el mensaje de error
                return Json(new { success = false, error = ex.Message });
            }
        }




        // Acción POST para eliminar una máquina
        [HttpPost]
        public JsonResult EliminarMaquina(int id_maquina)
        {
            try
            {
                // Establecer la conexión utilizando la cadena de conexión del archivo de configuración
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ToString()))
                {
                    // Crear un comando para llamar al procedimiento almacenado "sp_EliminarMaquina"
                    SqlCommand cmd = new SqlCommand("sp_EliminarMaquina", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Agregar parámetros al comando
                    cmd.Parameters.AddWithValue("@id_maquina", id_maquina);

                    // Abrir la conexión
                    con.Open();

                    // Ejecutar el comando sin esperar un resultado
                    cmd.ExecuteNonQuery();
                }

                // Si todo ha ido bien, devolver un JSON indicando éxito
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // En caso de error, manejar la excepción y devolver un JSON con éxito false y el mensaje de error
                return Json(new { success = false, error = ex.Message });
            }
        }




        // Acción POST para editar la información de un administrador
        [HttpPost]
        public JsonResult EditarInfoAdministrador(int id_administrador, string nombre_administrador, string correo_admin, string telefono_admin, int edad_administrador)
        {
            try
            {
                // Establecer la conexión utilizando la cadena de conexión del archivo de configuración
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["cadena"].ToString()))
                {
                    // Crear un comando para llamar al procedimiento almacenado "EditarInfoAdmin"
                    SqlCommand cmd = new SqlCommand("EditarInfoAdmin", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    // Agregar parámetros al comando
                    cmd.Parameters.AddWithValue("@id_administrador", id_administrador);
                    cmd.Parameters.AddWithValue("@nombre_administrador", nombre_administrador);
                    cmd.Parameters.AddWithValue("@correo_admin", correo_admin);
                    cmd.Parameters.AddWithValue("@telefono_admin", telefono_admin);
                    cmd.Parameters.AddWithValue("@edad_administrador", edad_administrador);

                    // Abrir la conexión
                    con.Open();

                    // Ejecutar el comando sin esperar un resultado
                    cmd.ExecuteNonQuery();
                }

                // Si todo ha ido bien, devolver un JSON indicando éxito
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // En caso de error, manejar la excepción y devolver un JSON con éxito false y el mensaje de error
                return Json(new { success = false, error = ex.Message });
            }
        }



        // Este método se encarga de generar y visualizar el reporte de suspensión en formato PDF.
        public ActionResult Ver_reporte_suspension()
        {
            // Se crea una lista para almacenar los datos de las suspensiones.
            var datos = new List<Suspension>();

            // Se instancia un objeto ReportDocument que representa el reporte de Crystal Reports.
            ReportDocument rd = new ReportDocument();

            // Se carga el archivo del reporte desde la ruta física del servidor.
            rd.Load(Path.Combine(Server.MapPath("~/Reportes/Reporte_suspension.rpt")));

            // Se obtiene la cadena de conexión desde el archivo de configuración.
            var connectionString = ConfigurationManager.ConnectionStrings["cadena"].ToString();

            // Se crea una conexión a la base de datos utilizando la cadena de conexión especificada.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Se crea un comando SQL para ejecutar el procedimiento almacenado que obtiene los datos del reporte.
                using (SqlCommand command = new SqlCommand("Obtener_datos_reporte_suspension", connection))
                {
                    // Se especifica que el tipo de comando es un procedimiento almacenado.
                    command.CommandType = CommandType.StoredProcedure;

                    // Se abre la conexión a la base de datos.
                    connection.Open();

                    // Se ejecuta el comando SQL y se obtiene un lector de datos.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Se recorre el resultado del comando y se agregan las suspensiones a la lista de datos.
                        while (reader.Read())
                        {
                            datos.Add(new Suspension
                            {
                                id_suspension = Convert.ToInt32(reader["id_suspension"]),
                                descripcion_suspension = reader["descripcion_suspension"].ToString(),
                                fecha_inicio_suspension = reader["fecha_inicio_suspension"].ToString(),
                                fecha_fin_suspension = reader["fecha_fin_suspension"].ToString(),
                                duracion_suspension = reader["duracion_suspension"].ToString(),
                                estado_suspension = reader["estado_suspension"].ToString(),
                                id_aprendiz = Convert.ToInt32(reader["id_aprendiz"]),
                                nombre_aprendiz = reader["nombre_aprendiz"].ToString()
                            });
                        }
                    }
                }
            }

            // Se establece la fuente de datos del informe con la lista de suspensiones obtenidas.
            rd.SetDataSource(datos);

            // Se exporta el informe a un flujo (Stream) en formato PDF.
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            // Se devuelve el archivo PDF como una respuesta del servidor.
            return File(stream, "application/pdf");
        }

        // Este método se encarga de generar y visualizar el reporte de máquinas en formato PDF.
        public ActionResult Ver_reporte_maquinas()
        {
            // Se crea una lista para almacenar los datos de las máquinas.
            var datos = new List<Maquinas>();

            // Se instancia un objeto ReportDocument que representa el reporte de Crystal Reports.
            ReportDocument rd = new ReportDocument();
            // Se carga el archivo del reporte desde la ruta física del servidor.
            rd.Load(Path.Combine(Server.MapPath("~/Reportes/Reporte_maquinas.rpt")));

            // Obtén la cadena de conexión desde el archivo de configuración.
            var connectionString = ConfigurationManager.ConnectionStrings["cadena"].ToString();

            // Se crea una conexión a la base de datos utilizando la cadena de conexión especificada.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Se crea un comando SQL para seleccionar los datos de las máquinas.
                using (SqlCommand command = new SqlCommand("SELECT id_maquina, nombre_maquina, tipo_maquina, cantidad_maquinas, estado_maquina FROM Maquinas", connection))
                {
                    // Se abre la conexión a la base de datos.
                    connection.Open();

                    // Se ejecuta el comando SQL y se obtiene un lector de datos.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Se recorre el resultado del comando y se agregan las máquinas a la lista de datos.
                        while (reader.Read())
                        {
                            datos.Add(new Maquinas
                            {
                                id_maquina = Convert.ToInt32(reader["id_maquina"]),
                                nombre_maquina = reader["nombre_maquina"].ToString(),
                                tipo_maquina = reader["tipo_maquina"].ToString(),
                                cantidad_maquinas = Convert.ToInt32(reader["cantidad_maquinas"]),
                                estado_maquina = reader["estado_maquina"].ToString()
                            });
                        }
                    }
                }
            }

            // Se establece la fuente de datos del informe con la lista de máquinas obtenidas.
            rd.SetDataSource(datos);

            // Se exporta el informe a un flujo (Stream) en formato PDF.
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            // Se devuelve el archivo PDF como una respuesta del servidor.
            return File(stream, "application/pdf");
        }

        // Acción para mostrar la vista de reporte de aprendices
        public ActionResult Reporte_asistencia()
        {
            return View();
        }

        public ActionResult Ver_reporte_asistencia()
        {
            // Se instancia un objeto ReportDocument que representa el reporte de Crystal Reports.
            ReportDocument rd = new ReportDocument();

            // Se carga el archivo del reporte desde la ruta física del servidor.
            rd.Load(Path.Combine(Server.MapPath("~/Reportes/Reporte_Asistencia.rpt")));

            // Obtén la cadena de conexión desde el archivo de configuración.
            var connectionString = ConfigurationManager.ConnectionStrings["cadena"].ToString();

            // Se crea una lista para almacenar los datos de las asistencias.
            var datos = new List<object>();

            // Se crea una conexión a la base de datos utilizando la cadena de conexión especificada.
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Se abre la conexión a la base de datos.
                connection.Open();

                // Se crea un comando SQL para ejecutar el procedimiento almacenado.
                using (SqlCommand command = new SqlCommand("Obtener_datos_reporte_asistencia", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Se ejecuta el comando SQL y se obtiene un lector de datos.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Se recorre el resultado del comando y se agregan los IDs de aprendiz a la lista de datos.
                        while (reader.Read())
                        {
                            datos.Add(new
                            {
                                id_asistencia = Convert.ToInt32(reader["id_asistencia"]),
                                fecha_ingreso = reader["fecha_ingreso"].ToString(),
                                fecha_salida = reader["fecha_salida"].ToString(),
                                id_aprendiz = Convert.ToInt32(reader["id_aprendiz"])
                            });
                        }
                    }
                }
            }

            // Se establece la fuente de datos del informe con la lista de datos obtenidos.
            rd.SetDataSource(datos);

            // Se exporta el informe a un flujo (Stream) en formato PDF.
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);

            // Se devuelve el archivo PDF como una respuesta del servidor.
            return File(stream, "application/pdf");
        }


        // Acción para mostrar la vista de reporte de suspensión
        public ActionResult Reporte_suspension()
        {
            return View();
        }

        // Acción para mostrar la vista de reporte de máquinas
        public ActionResult Reporte_maquinas()
        {
            return View();
        }

        // Acción para mostrar la vista de reporte de aprendices
        public ActionResult Reporte_aprendices()
        {
            return View();
        }

        // Acción para mostrar la vista de detalle de suspensión
        public ActionResult DetalleSuspencion()
        {
            return View();
        }

        // Acción para mostrar la vista de detalle de multa
        public ActionResult DetalleMulta()
        {
            return View();
        }

        // Acción para mostrar la vista de edición de máquina
        public ActionResult EditarMaquina()
        {
            return View();
        }


    }
}