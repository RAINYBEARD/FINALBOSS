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

            ViewBag.Title = "Home Page";
            //var materia_des = context.Materias_Descripciones.Create();
            //materia_des.Materia_Id = 8015;
            //materia_des.Mat_Des = "INTRO. A LA INFORMATICA";
            //context.Materias_Descripciones.Add(materia_des);
            var titulo = context.Titulos.Create();
            titulo.Plan_Tit = "10Z";
            titulo.Titulo_Id = 7290;
            titulo.Tit_Des = "Licenciado en Sistemas de Informacion";

            var alumno = context.Alumnos.Create();
            alumno.Matricula = " 951282";
            alumno.Password = "lalalalala";
            alumno.Titulo = titulo;
            context.Alumnos.Add(alumno);
            


            //var materia = context.Materias.Create();

            context.SaveChanges();
            //ViewBag.Cursos = LoadJson();
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
