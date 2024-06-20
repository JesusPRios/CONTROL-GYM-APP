using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Capa_Entidad;

namespace Capa_Datos
{
    public class CD_Multa
    {
        public int Registrar_multa(Multa obj, out string Mensaje)
        {
            Mensaje = string.Empty; 
            int resultado = 0;  

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))  // Crea una conexión utilizando la cadena de conexión de la clase Conexion
                {
                    oconexion.Open();  // Abre la conexión a la base de datos

                    SqlCommand cmd = new SqlCommand("Registrar_multa", oconexion);  // Crea un comando SQL para llamar al procedimiento almacenado Registrar_multa
                    cmd.CommandType = CommandType.StoredProcedure; 

                    // Añade parámetros al comando SQL
                    cmd.Parameters.AddWithValue("@valor_multa", obj.valor_multa);  
                    cmd.Parameters.AddWithValue("@estado_multa", obj.estado_multa);  
                    cmd.Parameters.AddWithValue("@id_sancion", obj.id_sancion);  
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;  
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;  

                    cmd.ExecuteNonQuery();  // Ejecuta el comando SQL

                    // Obtiene el resultado y el mensaje de la base de datos
                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);  // Obtiene el resultado de la operación (puede ser un ID autogenerado)
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();  // Obtiene el mensaje de la base de datos
                }
            }
            catch (Exception ex)
            {
                Mensaje = ex.Message;  // Guarda el mensaje de excepción
            }

            return resultado;  // Devuelve el resultado
        }
    }
}
