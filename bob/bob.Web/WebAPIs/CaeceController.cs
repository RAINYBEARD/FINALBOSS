using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using bob.Data;
using bob.Data.DTOs;
using bob.Data.Dictionaries;
using Newtonsoft.Json.Linq;
using bob.CaeceWS;
using System.Web.Configuration;
using bob.Mocks;
using bob.Data.Entities;

namespace bob.Controllers
{
    public class CaeceController : Controller
    {
        private readonly CaeceDBContext context = new CaeceDBContext();
        //private string _token = WebConfigurationManager.AppSettings.Get("CaeceWSToken");
        //private CaeceWS.wbsTrans caeceWS = new CaeceWS.wbsTrans();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/GetPlanDeEstudio/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("GetPlanDeEstudio/{matricula}")]
        public void GetPlanDeEstudio(string matricula)
        {
            var PlanDeEstudioJSON = MockService.LoadJson<PlanEstudio>(MockMethod.PlanDeEstudio);

            using (var context = new CaeceDBContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (PlanEstudio dato in PlanDeEstudioJSON)
                        {
                            // Cargo a la base los datos de las materias
                            bool resultado = context.Materias_Descripciones.Any(a => a.Materia_Id == dato.materia_id);

                            if (resultado == false)
                            {
                                var materia_descripcion = context.Materias_Descripciones.Create();
                                AutoMapper.Mapper.Map(dato, materia_descripcion);
                                context.Materias_Descripciones.Add(materia_descripcion);
                                context.SaveChanges();
                            }

                            // Cargo a la base los datos de los titulos
                            resultado = context.Titulos.Any(a => a.Plan_Tit == dato.plan_tit && a.Titulo_Id == dato.titulo_id);

                            if (resultado == false)
                            {
                                var titulo = context.Titulos.Create();
                                AutoMapper.Mapper.Map(dato, titulo);
                                context.Titulos.Add(titulo);
                                context.SaveChanges();
                            }

                            // Cargo a la base la relacion materia titulo
                            resultado = context.Materias.Any(a => a.Materia_Id == dato.materia_id && a.Plan_Id == dato.plan_id && a.Plan_Tit == dato.plan_tit && a.Titulo_Id == dato.titulo_id);
                            if (resultado == false)
                            {
                                var materia = context.Materias.Create();
                                AutoMapper.Mapper.Map(dato, materia);
                                context.Materias.Add(materia);
                                context.SaveChanges();
                            }

                            // Cargo a la base las materias correlativas
                            var correlativa = context.Correlativas.Create();
                            AutoMapper.Mapper.Map(dato, correlativa);
                            context.Correlativas.Add(correlativa);
                            context.SaveChanges();
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/GetDictionaries/951282 
        /// CARGA LOS DICCIONARIOS
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("GetDictionaries/{matricula}")]
        public void GetDictionaries(string matricula)
        {
            var aprDictionary = new AprDictionary();
            var curDictionary = new CurDictionary();
            var penDictionary = new PenDictionary();
            var repDictionary = new RepDictionary();
            var notCurDictionary = new NotCurDictionary();
            var mesaFinalDictionary = new MesasDictionary();
            var cursosDictionary = new CursosDictionary();

            foreach (HistoriaAcademica dato in MockService.LoadJson<HistoriaAcademica>(MockMethod.HistoriaAcademica))
            {
                string estado_materia = dato.Descrip;
                string matcod = dato.Matcod;
                switch (estado_materia)
                {
                    case ("APR"):
                        AprValue apr = new AprValue();
                        AutoMapper.Mapper.Map(dato, apr);
                        aprDictionary.Add(matcod, apr);
                        break;
                    case ("CUR"):
                        CurValue cur = new CurValue();
                        AutoMapper.Mapper.Map(dato, cur);
                        curDictionary.Add(matcod, cur);
                        break;
                    case ("PEN"):
                        PenValue equiv = new PenValue();
                        AutoMapper.Mapper.Map(dato, equiv);
                        penDictionary.Add(matcod, equiv);
                        break;
                    case ("REP"):
                        string fecha = dato.Fecha;
                        repDictionary.Add(matcod, fecha);
                        break;
                    default:
                        if ((estado_materia.Equals("   ")) || (estado_materia.Equals("?  ")))
                        {
                            NotCurValue notcur = new NotCurValue();
                            AutoMapper.Mapper.Map(dato, notcur);
                            notCurDictionary.Add(matcod, notcur);
                        }
                        break;
                }
            }
            //ACA SE CARGAN LOS DICCIONARIOS 
            Helpers.SessionManager.DiccionarioAprobadas = aprDictionary;
            Helpers.SessionManager.DiccionarioCursadas = curDictionary;
            Helpers.SessionManager.DiccionarioPendientes = penDictionary;
            Helpers.SessionManager.DiccionarioNoCursadas = notCurDictionary;

        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/GetCorrelativas/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("GetCorrelativas/{matricula}")]
        public void GetCorrelativas(string matricula)
        {
            System.Diagnostics.Debug.WriteLine("Entro en el controller Cursos");
            //CHEQUEAR QUE LOS DICCIONARIOS ESTEN CARGADOS ANTES DE EMPEZAR A PROCESAR
            if (Helpers.SessionManager.DiccionarioCursadas != null)
            {

                var query1 = BuscarUltimasMateriasDelPlanDeEstudio();

                foreach (var resultado0 in query1)
                {
                    var query2 = BuscarCorrelativa(resultado0.Materia_Id);

                    foreach (var resultado in query2)
                    {
                        if (resultado.Materia_Id != resultado.Codigo_Correlativa)
                        {
                            RecorrerCorrelativas(resultado);
                        }
                    }
                }
            }
        }

        public List<Data.Entities.Correlativa> BuscarCorrelativa(int idmateria)
        {
            using (var context = new CaeceDBContext())
            {
                return context.Correlativas.Where(a => a.Materia_Id == idmateria).ToList();
            }
        }

        public List<Data.Entities.Materia> BuscarUltimasMateriasDelPlanDeEstudio()
        {
            using (var context = new CaeceDBContext())
            {
                short ultimo_anio_de_la_carrera = (short)(context.Materias.Max(a => a.Anio));
                return context.Materias.Where(a => a.Anio == ultimo_anio_de_la_carrera && a.Cuatrim == 2).ToList();
            }
        }

        private void RecorrerCorrelativas(Data.Entities.Correlativa correlativa)
        {
            //CHEQUEAR QUE LOS DICCIONARIOS ESTEN CARGADOS ANTES DE EMPEZAR A PROCESAR
            //if(Helpers.SessionManager.DiccionarioCursadas != null);

            // Print the node.  
            System.Diagnostics.Debug.WriteLine("Materia : " + correlativa.Materia_Id + " Correlativa : " + correlativa.Codigo_Correlativa);
            //if (!this.aprDictionary.Equals(correlativa.Materia_Id))
            //{
            //    this.materia_ant = correlativa.Materia_Id;
            //}
            //else
            //{
            //    System.Diagnostics.Debug.WriteLine("Materia que puede cursar : " + this.materia_ant);
            //}

            var resulcorrelativa = BuscarCorrelativa(correlativa.Codigo_Correlativa);

            foreach (var resultado in resulcorrelativa)
            {
                if (resultado.Materia_Id != resultado.Codigo_Correlativa)
                {
                    RecorrerCorrelativas(resultado);
                }
            }

        }

        public int prioridad = 0;

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/PlanificadorFinales/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("PlanificadorFinales/{matricula}")]
        public List<CursadoStatus> PlanificadorFinales()
        {
            List<CursadoStatus> cursados = new List<CursadoStatus>();
            var aprDictionary = Session["APR"] as AprDictionary;
            var curDictionary = Session["CUR"] as CurDictionary;
            var repDictionary = Session["REP"] as RepDictionary;
            foreach (KeyValuePair<string, CurValue> entry in curDictionary)
            {
                bool reprobadas = false;
                int correlativas = 0;
                string fechaauxiliar = string.Format("Y", DateTime.Parse(entry.Value.Fecha));
                DateTime fecur = DateTime.Parse(fechaauxiliar);
                DateTime feven = DateTime.Parse(fechaauxiliar);
                //Fecha de Vencimiento
                if (feven.ToString("MMMM") == "Jun")
                {
                    feven.AddMonths(6);
                    feven.AddYears(1);
                }
                else
                {
                    feven.AddMonths(-6);
                    feven.AddYears(2);
                }
                //Numero de Correlativas de la Materia Cursada
                foreach (Correlativa corr in  context.Correlativas.Where(a => (a.Codigo_Correlativa+"/"+ a.Plan_Id) == entry.Key)) 
                {
                    correlativas = correlativas + 1;
                }
                //Si se reprobo o no
                if (repDictionary.ContainsKey(entry.Key))
                {
                    string fechareprobado;
                    repDictionary.TryGetValue(entry.Key, out fechareprobado);

                    if (DateTime.Parse(fechareprobado) > DateTime.Parse(entry.Value.Fecha))
                    {
                        reprobadas = true;
                    }
                }
                //Usar AutoMapper
                cursados.Add(new CursadoStatus() { materia_cod = entry.Key, fecha_cursada = fecur, fecha_vencimiento = feven, n_correlativas = correlativas, reprobado = reprobadas });
            }

            foreach (CursadoStatus cur in cursados)
            {
                foreach (Correlativa corr in context.Correlativas)
                {
                    if (corr.Materia_Id+ "/" + corr.Plan_Id == cur.materia_cod)
                    {
                        if ((!aprDictionary.ContainsKey(cur.materia_cod)) && (!curDictionary.ContainsKey(cur.materia_cod)))
                        {
                            cursados.Remove(cur);
                        }
                    }
                }
            }

            return cursados;
        } 
        //Orden por fecha de vencimiento
        public List<CursadoStatus> FinalesPorVecimiento()
        {
            List<CursadoStatus> Ls = PlanificadorFinales();
            //int prioridad = 1;
            //foreach (CursadoStatus curr in Ls)
            //{
            //    if((DateTime.Now == curr.fecha_vencimiento) || (DateTime.Now.AddMonths(1) == curr.fecha_vencimiento))
            //    {
            //        curr.n_prioridad = prioridad;
            //        prioridad = prioridad + 1;

            //    }
            //    else
            //    {
            //        Ls.Remove(curr);
            //    }
            //}
            Ls.OrderByDescending(p => p.fecha_vencimiento);

            return Ls;


        }
        //Orden por numero de correlativas asociadas
        public List<CursadoStatus> FinalesPorCorrelativas()
        {
            List<CursadoStatus> Ls = PlanificadorFinales();         
            Ls.OrderByDescending(p => p.n_correlativas);

            return Ls;
        }



    }      
}























