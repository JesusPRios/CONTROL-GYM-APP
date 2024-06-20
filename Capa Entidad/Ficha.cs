using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Ficha
    {
        public int numero_ficha { get; set; }
        public string estado_ficha { get; set; }
        public DateTime fecha_inicio_ficha { get; set; }
        public DateTime fecha_fin_ficha { get; set; }
        public Programa_form oPrograma_Form { get; set; }
    }
}
