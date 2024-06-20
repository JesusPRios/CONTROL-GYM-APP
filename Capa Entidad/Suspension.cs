using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Suspension
    {
        public int id_suspension { get; set; }
        public string descripcion_suspension { get; set; }
        public string fecha_inicio_suspension { get; set; }
        public string fecha_fin_suspension { get; set; }
        public string duracion_suspension { get; set; }
        public string estado_suspension { get; set; }
        public int id_aprendiz { get; set; }
        public string nombre_aprendiz { get; set; }
        public Aprendiz oAprendiz { get; set; }
    }
}
