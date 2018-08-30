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
            CaeceDbContext context = new CaeceDbContext();

            ViewBag.Title = "Home Page";
            //var materia = context.materias.Create();
            //materia.materiaid = 8015;
            //materia.abr = "INTRO. A LA INFORMATICA";
            //context.materias.Add(materia);
            //context.SaveChanges();
            ViewBag.Cursos = LoadJson();
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
