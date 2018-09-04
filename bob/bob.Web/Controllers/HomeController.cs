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

            var PlanDeEstudioJSON = LoadJson();
            foreach (PlanDeEstudio dato in PlanDeEstudioJSON)
            {
                // Cargo a la base los datos de las materias
                bool resultado = context.Materias_Descripciones.Any(a => a.Materia_Id == dato.materia_id);

                if (resultado == false)
                {
                    var materia_descripcion = context.Materias_Descripciones.Create();
                    materia_descripcion.Materia_Id = dato.materia_id;
                    materia_descripcion.Mat_Des = dato.mat_des.Trim();
                    context.Materias_Descripciones.Add(materia_descripcion);
                    context.SaveChanges();
                }

                // Cargo a la base los datos de los titulos
                resultado = context.Titulos.Any(a => a.Plan_Tit == dato.plan_tit && a.Titulo_Id == dato.titulo_id);

                if (resultado == false)
                {
                    var titulo = context.Titulos.Create();
                    titulo.Plan_Tit = dato.plan_tit;
                    titulo.Titulo_Id = dato.titulo_id;
                    context.Titulos.Add(titulo);
                    context.SaveChanges();
                }

                // Cargo a la base la relacion materia titulo
                resultado = context.Materias.Any(a => a.Materia_Id == dato.materia_id && a.Plan_Id == dato.plan_id && a.Plan_Tit == dato.plan_tit && a.Titulo_Id == dato.titulo_id );

                if (resultado == false)
                {
                    var materia = context.Materias.Create();
                    materia.Materia_Id = dato.materia_id;
                    materia.Plan_Id = dato.plan_id;
                    materia.Plan_Tit = dato.plan_tit;
                    materia.Titulo_Id = dato.titulo_id;
                    materia.Anio = dato.anio;
                    materia.Cuatrim = dato.cuatrim;
                    materia.Mat_Modulos = dato.mat_modulos;
                    context.Materias.Add(materia);
                    context.SaveChanges();
                }
            }




            return View();
        }

        public List<PlanDeEstudio> LoadJson()
        {
            var json = MockService.MockService.PlanDeEstudio();
            var jobject = JObject.Parse(json);
            var sCursos = (JArray)jobject["PlanEstudio"];
            return sCursos.ToObject<List<PlanDeEstudio>>();
        }

    }

    public class PlanDeEstudio
    {
        public int materia_id { get; set; }
        public string mat_des { get; set; }
        public string plan_tit { get; set; }
        public int titulo_id { get; set; }
        public string plan_id { get; set; }
        public short anio { get; set; }
        public short cuatrim { get; set; }
        public float mat_modulos { get; set; }
    }

    public class Curso
    {
        public string periodo_id { get; set; }
        public string materia_id { get; set; }
        public string perso_id { get; set; }
    }
}
