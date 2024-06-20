using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Entidad
{
    public class Ejercicio
    {
        public int id_ejercicio { get; set; }
        public string nombre_ejercicio { get; set; }
        public int dias_ejercicio { get; set; }
        public int horas_ejercicio { get; set; }
        public Rutina oRutina { get; set; }
    }
}
