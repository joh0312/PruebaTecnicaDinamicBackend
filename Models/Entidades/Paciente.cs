using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Documento { get; set; } // Documento de identidad
        public DateTime FechaNacimiento { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
    }
}
