using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Asistencia
    {
        public int id_asistencia { get; set; }
        public DateTime fecha_asistencia { get; set; }
        public string hora_asistencia { get; set; }
        public Aprendiz oAprendiz { get; set; }
    }
}
