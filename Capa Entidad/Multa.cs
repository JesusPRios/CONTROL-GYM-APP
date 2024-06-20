using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Multa
    {
        public int id_multa { get; set; }
        public decimal valor_multa { get; set; }
        public string estado_multa { get; set; }
        public int id_sancion { get; set; }
        public Sancion oSancion { get; set; }
    }
}