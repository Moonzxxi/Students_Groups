using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace Grupos
{
   public class Program
   { 
        public static List<Tema> LeerArchivoTemas(string archivo)
        {
            List<Tema> result = new List<Tema>();
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
                string text = File.ReadAllText(Path.Combine(path, archivo));
                result = text.Split('\n').Select(y => new Tema() { NombreTema = y.Replace('\r', ' ') }).ToList();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
            return result;
        }

        public static List<Persona> LeerArchivoPersonas( string archivo)
        {
            List<Persona> result = new List<Persona>();

            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
                string text = File.ReadAllText(Path.Combine(path, archivo));
                result = text.Split('\n').Select(x => new Persona() { Nombre = x.Replace('\r', ' ') }).ToList();
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }

            return result;
        }

        public static List<grupo> Asignacion( List<Tema> temas, List<Persona> personas, int cantidadGrupos)
        {
            var result = new List<grupo>();
            Random rnd = new Random();

            personas = personas.Select(y => new Persona { Nombre = y.Nombre, Order = rnd.Next(1, 1000) }).OrderBy(y => y.Order).
                ToList();

            temas = temas.Select(x => new Tema { NombreTema = x.NombreTema, Order = rnd.Next(1, 1000) }).OrderBy(x => x.Order).
                ToList();

            var TemasPorGrupo = temas.Count() / cantidadGrupos;
            var PersonasPorGrupo = personas.Count() / cantidadGrupos;

            var PersonasYaSeleccionadas = new List<string>();
            var TemasYaSeleccionados = new List<string>();

            for (int i = 0; i < cantidadGrupos; i++)
            {
                var personasGrupoActual = personas.Where(y => !PersonasYaSeleccionadas.Contains(y.Nombre)).Take(PersonasPorGrupo).ToList();
                var temasGrupoActual = temas.Where(y => !TemasYaSeleccionados.Contains(y.NombreTema)).Take(TemasPorGrupo).ToList();
               
                TemasYaSeleccionados.AddRange(temasGrupoActual.Select(y => y.NombreTema));
                PersonasYaSeleccionadas.AddRange(personasGrupoActual.Select(y => y.Nombre));

                var gp = new grupo()
                {
                    Id = i + 1,
                    Temas = temasGrupoActual,
                    Personas = personasGrupoActual,

                };
                result.Add(gp);
            }
            temas.RemoveRange(0, TemasPorGrupo * cantidadGrupos);
            personas.RemoveRange(0, PersonasPorGrupo * cantidadGrupos);

            if (temas.Count()>0)
            {
                foreach (var tema in temas)
                {
                    int i = rnd.Next(1, cantidadGrupos);
                    var gp = result.FirstOrDefault(y => y.Id == i);
                    gp.Temas.Add(tema);
                }
            }
            if (personas.Count()>0)
            {
                foreach (var persona in personas)
                {
                    int i = rnd.Next(1, cantidadGrupos);
                    var gp = result.FirstOrDefault(x => x.Id == i);
                    gp.Personas.Add(persona);
                }
            }
            return result;
        }

        public static void MostrarEnPantalla(List<grupo> grupos)
        {
            int i = 1;
            foreach (var grupo in grupos)
            {
                Console.WriteLine("Grupo {0}", i);
                Console.WriteLine("Temas: {0}",string.Join(",  ", grupo.Temas.Select(y => y.NombreTema)));
                Console.WriteLine("Miembros: {0}",string.Join(",   ", grupo.Personas.Select(y => y.Nombre)));
                Console.WriteLine("----------------------------------------------------------------------");
                i++;
            }
        }

        static void Main(string[] args)
        {
            int cantidadGrupos = -1;
            Console.WriteLine("Iniciando la lectura de los archivos...");
            var personas = LeerArchivoPersonas("Personas.txt");
            var temas = LeerArchivoTemas("Temas.txt");

            Console.WriteLine("Lectura Finalizada...");
            Thread.Sleep(700);

            Console.Clear();

            while (cantidadGrupos == -1)
            {
                Console.WriteLine("Favor insertar la cantidad de grupos: ");
                var isValidNumber = int.TryParse(Console.ReadLine(),out cantidadGrupos);

                if (!isValidNumber|| cantidadGrupos<=0)
                {
                    cantidadGrupos = -1;
                }
            }

            var gruposResultado = Asignacion(temas, personas, cantidadGrupos);
            MostrarEnPantalla(gruposResultado);

            Console.ReadKey();
        }
    }
}
