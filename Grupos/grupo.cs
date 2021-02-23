using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupos
{
    public class grupo
    {
        public int Id { get; set; }
        public List<Persona> Personas { get; set; }
        public List<Tema> Temas { get; set; }

    }
}
