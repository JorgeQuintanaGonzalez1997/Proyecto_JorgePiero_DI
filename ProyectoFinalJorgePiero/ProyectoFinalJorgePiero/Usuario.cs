using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoFinalJorgePiero
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string usuario { get; set; }

        public string password { get; set; }


    }
}
