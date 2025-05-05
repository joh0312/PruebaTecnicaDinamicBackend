using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entidades
{
    public class Cita
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public int IdMedico { get; set; }
        public Medico Medico { get; set; } // Relación con Medico
        public string Estado { get; set; } = "Disponible"; // Disponible o Reservada
        public int? IdPaciente { get; set; } // Relación con Paciente (opcional)
        public Paciente Paciente { get; set; } // Relación con Paciente
    }
}
