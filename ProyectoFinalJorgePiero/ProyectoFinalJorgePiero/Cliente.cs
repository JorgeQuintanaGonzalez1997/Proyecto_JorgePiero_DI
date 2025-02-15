using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalJorgePiero
{
    public class Cliente : Usuario
    {
        
        
        public string cita { get; set; }
        public string receta { get; set; }

    }
}
