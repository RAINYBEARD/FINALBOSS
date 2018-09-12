using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob.Data.DTOs
{
    /// <summary>
    /// Maps from Cursos JSON
    /// </summary>
    public class Curso
    {
        public string Materia_Id { get; set; }
        public string Plan_Id { get; set; }
        public char Turno_Id { get; set; }
        public string Dia { get; set; }
        public int M_Acobrar { get; set; }
    }
}
