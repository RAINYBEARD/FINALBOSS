using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bob.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using bob.MockService;

namespace bob.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CaeceDBContext context = new CaeceDBContext();

            ViewBag.PlanDeEstudio = LoadJson();
            foreach (PlanDeEstudio dato in ViewBag.PlanDeEstudio)
            {
                int materia_a_buscar = int.Parse(dato.materia_id);
                bool resultado = context.Materias_Descripciones.Any(a => a.Materia_Id == materia_a_buscar);

                if (resultado == false)
                {
                    var materia = context.Materias_Descripciones.Create();
                    materia.Materia_Id = materia_a_buscar;
                    materia.Mat_Des = dato.mat_des.Trim();
                    context.Materias_Descripciones.Add(materia);
                    context.SaveChanges();
                }
            }

            return View();
        }

        public List<Curso> LoadJson()
        {
            var json = MockService.MockService.CursosJson();
            var jobject = JObject.Parse(json);
            var sCursos = (JArray)jobject["Cursos"];
            return sCursos.ToObject<List<Curso>>();
        }



    }

    public class Curso
    {
        public string periodo_id { get; set; }
        public string materia_id { get; set; }
        public string perso_id { get; set; }
    }
}
