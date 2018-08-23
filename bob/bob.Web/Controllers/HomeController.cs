using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using bob.Data;


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



            return View();
        }
    }
}
