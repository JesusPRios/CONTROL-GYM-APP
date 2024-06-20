using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Aprendiz
    {
        public int id_aprendiz { get; set; }
        public string tipo_id_aprendiz { get; set; }
        public string nombre_aprendiz { get; set; }
        public string contraseña_aprendiz { get; set; }
        public long telefono_aprendiz { get; set; }
        public string estado_aprendiz { get; set; }
        public string correo_aprendiz { get; set; }
        public Ficha oFicha { get; set; }
        public Programa_form oPrograma { get; set; }
    }
}
