using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading;
using bob.Data;
using bob.Data.Entities;
using bob.Data.DTOs;
using bob.Data.Dictionaries;
using Newtonsoft.Json.Linq;
using bob.CaeceWS;
using System.Web.Configuration;
using bob.Mocks;
using bob.Helpers;

namespace bob.Controllers
{
    public class CaeceController : Controller
    {
        private readonly CaeceDBContext context = new CaeceDBContext();
        private string _token = WebConfigurationManager.AppSettings.Get("CaeceWSToken");
        private CaeceWS.wbsTrans caeceWS = new CaeceWS.wbsTrans();
        

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/GetPlanDeEstudio/?matricula=951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("GetPlanDeEstudio/{matricula}")]
        public void GetPlanDeEstudio(string matricula)
        {
            // Para hacer llamada al Mock
            //var PlanDeEstudio = MockService.LoadJson<PlanEstudio>(MockMethod.PlanDeEstudio);

            // Para hacer la llamada al WS
            var JSON = caeceWS.getPlanEstudioJSON(_token, " " + matricula); 
            var PlanDeEstudio = ((JArray)JObject.Parse(JSON)["PlanEstudio"]).ToObject<List<PlanEstudio>>();

            using (var context = new CaeceDBContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (PlanEstudio dato in PlanDeEstudio)
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

            foreach (var dato in MockService.LoadJson<Curso>(MockMethod.Cursos))
            {
                if (notCurDictionary.ContainsKey(dato.Materia_Id + "/" + dato.Plan_Id))
                {
                    CursosValue curso = new CursosValue();
                    AutoMapper.Mapper.Map(dato, curso);
                    cursosDictionary.Add(dato.Materia_Id, curso);
                }

            }

            //ACA SE CARGAN LOS DICCIONARIOS 
            SessionManager.DiccionarioAprobadas = aprDictionary;
            SessionManager.DiccionarioCursadas = curDictionary;
            SessionManager.DiccionarioPendientes = penDictionary;
            SessionManager.DiccionarioNoCursadas = notCurDictionary;
            SessionManager.DiccionarioCursos = cursosDictionary;

            // OJO QUE ESTA HARCODEADO CAMBIAR ESTO
            SessionManager.TituloId = 7290;
            SessionManager.PlanTit = "10Z";
        }

        public void CargarDiccionarioDeCorrelativas()
        {
            var context = new CaeceDBContext();
            // Modificar lo hardcodeado
            var listCorrelativas = context.Correlativas.Where(a => a.Titulo_Id == SessionManager.TituloId && a.Plan_Tit == SessionManager.PlanTit).ToList();

            var existentes = new List<CorrValue>();
            var dicCorrelativas = new CorrDictionary();

            foreach (var correl in listCorrelativas)
            {
                var corr = new CorrValue();
                AutoMapper.Mapper.Map(correl, corr);

                if (!dicCorrelativas.ContainsKey(correl.Materia_Id))
                {
                    var listCorrel = new List<CorrValue>();
                    listCorrel.Add(corr);
                    dicCorrelativas.Add(correl.Materia_Id, listCorrel);
                }
                else
                {
                    if (dicCorrelativas.TryGetValue(correl.Materia_Id, out existentes))
                    {
                        existentes.Add(corr);
                        dicCorrelativas[correl.Materia_Id] = existentes;
                    }
                }
            }
            SessionManager.DiccionarioCorr = dicCorrelativas;
        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/GetMateriasACursar/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("GetMateriasACursar/{matricula}")]
        public List<string> GetMateriasACursar(string matricula)
        {
            var tiempoInicio=DateTime.Now;
            GetDictionaries(matricula);
            CargarDiccionarioDeCorrelativas();
            List<string> materiasACursar = new List<string>();
            List<CorrValue> materiasParaBuscarCorrelativas = new List<CorrValue>();
            string materiaAnt = "";

            System.Diagnostics.Debug.WriteLine("Entro en el controller Cursos");
            //CHEQUEAR QUE LOS DICCIONARIOS ESTEN CARGADOS ANTES DE EMPEZAR A PROCESAR
            if (SessionManager.DiccionarioCursadas != null)
            {

                foreach (var resultado0 in SessionManager.DiccionarioNoCursadas)
                {
                    // Descompongo la materiaid del planid
                    string[] matriculaId = resultado0.Key.Split(new Char[] { '/' });
                    var query2 = BuscarCorrelativa(int.Parse(matriculaId[0]));

                    // Evaluacion para la materias que no tienen correlativas
                    if (query2.Count == 1)
                        materiasACursar.Add(query2[0].materia_id + "/" + query2[0].plan_id);

                    foreach (var resultado in query2)
                    {
                        if (resultado.materia_id != resultado.codigo_correlativa)
                        {
                            // Elimino los resultados repetidos
                            if (!materiasParaBuscarCorrelativas.Any(x => x.materia_id == resultado.materia_id))
                                materiasParaBuscarCorrelativas.Add(resultado);
                        }
                    }
                }
                foreach (var materia in materiasParaBuscarCorrelativas)
                {
                    BuscarMateriasACursar(materia, ref materiasACursar, materiaAnt);
                }
            }
            var tiempoFin = DateTime.Now;
            System.Diagnostics.Debug.WriteLine("tardo : " + (tiempoFin - tiempoInicio));
            return materiasACursar;
        }

        public List<CorrValue> BuscarCorrelativa(int idMateria)
        {
            if (idMateria > 100)
            {
                return SessionManager.DiccionarioCorr[idMateria];
            }
            else
            {
                var lcorr = new List<CorrValue>();
                return lcorr;
            }
        }

        public List<Data.Entities.Materia> BuscarUltimasMateriasDelPlanDeEstudio()
        {
            using (var context = new CaeceDBContext())
            {
                short ultimoAnioDeLaCarrera = (short)(context.Materias.Max(a => a.Anio));
                return context.Materias.Where(a => a.Anio == ultimoAnioDeLaCarrera && a.Cuatrim == 2).ToList();
            }
        }

        private void BuscarMateriasACursar(CorrValue correlativa,ref List<string> materiasACursar,string materiaAnt)
        {
            //CHEQUEAR QUE LOS DICCIONARIOS ESTEN CARGADOS ANTES DE EMPEZAR A PROCESAR
            if (SessionManager.DiccionarioCursadas != null)
            {

                // Busco las correlativas dentro del diccionario de las materias no cursadas
                if (SessionManager.DiccionarioNoCursadas.ContainsKey(correlativa.materia_id + "/" + correlativa.plan_id))
                {
                    // Verifico asignaturas que requieren cantidad de materias aprobadas
                    if (correlativa.codigo_correlativa < 100 && correlativa.codigo_correlativa <= (SessionManager.DiccionarioAprobadas.Count() + SessionManager.DiccionarioCursadas.Count()) )
                    {
                        materiasACursar.Add(correlativa.materia_id + "/" + correlativa.plan_id);
                    }

                    // Almaceno la materia para evaluar si la correlativa es la que le resta cursar al alumno
                    materiaAnt = correlativa.materia_id + "/" + correlativa.plan_id;

                    var resulCorrelativa = BuscarCorrelativa(correlativa.codigo_correlativa);
                    bool flagMateriaACursar = true;
                    foreach (var resultado in resulCorrelativa)
                    {
                        if (resultado.materia_id != resultado.codigo_correlativa)
                        {
                            if (!SessionManager.DiccionarioAprobadas.ContainsKey(resultado.materia_id + "/" + resultado.plan_id) && !SessionManager.DiccionarioCursadas.ContainsKey(resultado.materia_id + "/" + resultado.plan_id))
                            {
                                flagMateriaACursar = false;
                            }
                            // Hago llamada recursiva para recorrer el arbol de materias correlativas
                            BuscarMateriasACursar(resultado, ref materiasACursar, materiaAnt);
                        }
                    }

                    if (flagMateriaACursar)
                    {
                        if (!materiasACursar.Contains(materiaAnt))
                            materiasACursar.Add(materiaAnt);
                    }
                }
            }
        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/GetMateriasACursarCuatrimestreActual/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("GetMateriasACursarCuatrimestreActual/{matricula}")]
        public List<Curso> GetMateriasACursarCuatrimestreActual(string matricula)
        {
            GetDictionaries(matricula);
            List<string> materiasACursar = GetMateriasACursar(matricula);
            List<Curso> materiasACursarEsteCuatri = new List<Curso>();
            

            foreach (var materia in materiasACursar)
            {
                // Descompongo la materiaid del planid
                string[] materiaid = materia.Split(new Char[] { '/' });

                // Verifico los cursos que puede cursar este cuatrimestre
                if (SessionManager.DiccionarioCursos.ContainsKey(materiaid[0]))
                {
                    Curso cursomateria = new Curso();
                    cursomateria.Materia_Id = materiaid[0];
                    cursomateria.Dia = SessionManager.DiccionarioCursos[materiaid[0]].Dia;
                    cursomateria.M_Acobrar = SessionManager.DiccionarioCursos[materiaid[0]].M_Acobrar;
                    cursomateria.Plan_Id = SessionManager.DiccionarioCursos[materiaid[0]].Plan_Id;
                    cursomateria.Turno_Id = SessionManager.DiccionarioCursos[materiaid[0]].Turno_Id;

                    // Agrego a la lista los cursos a los cuales se puede inscribir
                    materiasACursarEsteCuatri.Add(cursomateria);                    
                }
            }
            return materiasACursarEsteCuatri;
        }

        public bool VerificarFiltroDias(string filtro,string diaMateria)
        {
            int i = 0;

            // Verifico si el filtro de dias en los cuales se cursa una materia machea con la cantidad de dias que se cursa la materia
            while (i < 7 && ((filtro.Substring(i, 1) == "1" && diaMateria.Substring(i, 1) == "1") || 
                             (filtro.Substring(i, 1) == "1" && diaMateria.Substring(i, 1) == "0") ||
                             (filtro.Substring(i, 1) == "0" && diaMateria.Substring(i, 1) == "0")))
            {
                i++;
            }
            if (i == 7)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static string ReplaceAtIndex(int i, char value, string word)
        {
            char[] letters = word.ToCharArray();
            letters[i] = value;
            return string.Join("", letters);
        }

        public bool AgregarDiasACursar(string diaMateria,ref string diasACursar, int filtroCantDias)
        {
            int i = 0;
            List<int> dias = new List<int>();
            int cantDias = 0;
            while ( i<7 && ((diaMateria.Substring(i, 1) == "1" && diasACursar.Substring(i, 1) == "0") ||
                            (diaMateria.Substring(i, 1) == "0" && diasACursar.Substring(i, 1) == "0") ||
                            (diaMateria.Substring(i, 1) == "0" && diasACursar.Substring(i, 1) == "1")))
            {
                if (diaMateria.Substring(i, 1) == "1" && diasACursar.Substring(i, 1) == "0")
                    dias.Add(i);
                i++;
            }
            for (int j = 0; j < 7; j++)
            {
                if (diasACursar.Substring(j, 1) != "0")
                    cantDias++;
            }

            // Le sumo la cantidad de dias de la materia nueva
            cantDias = cantDias + dias.Count();

            if (i == 7 && (cantDias <= filtroCantDias))
            {
                foreach (var indice in dias)
                {
                    diasACursar = ReplaceAtIndex(indice, '1', diasACursar);
                }
                
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Curso> MostrarMateriasACursarCuatrimestreActual(string matricula,string filtroDias,int filtroCantDias,string modo)
        {
            List<Curso> materiasACursarEsteCuatri = GetMateriasACursarCuatrimestreActual(matricula);
            List<Curso> mostrarMateriasACursarEsteCuatri = new List<Curso>();
            List<Curso> auxMateriasACursar = new List<Curso>();
            string diasQueCursa = "0000000";

            if (modo == "manual")
            {
                foreach (var materia in materiasACursarEsteCuatri)
                {
                    // Verifico si el filtro de dias en los cuales se cursa una materia machea con la cantidad de dias que se cursa la materia
                    if (VerificarFiltroDias(filtroDias, materia.Dia))
                    {
                        mostrarMateriasACursarEsteCuatri.Add(materia);
                    }
                }
            }
            else
            {
                if (modo == "auto")
                {
                    foreach (var materia in materiasACursarEsteCuatri)
                    {
                        // Para contar cantidad de dias que se cursa una materia
                        string[] auxdiasacursar = materia.Dia.Split(new Char[] { '1' });
                        int cantidad_dias_que_se_cursa_materia = auxdiasacursar.Length - 1;
                        
                        if (cantidad_dias_que_se_cursa_materia > 1 && AgregarDiasACursar(materia.Dia, ref diasQueCursa, filtroCantDias) && VerificarFiltroDias(filtroDias, materia.Dia))
                        {
                            mostrarMateriasACursarEsteCuatri.Insert(0, materia);
                        }
                        else
                        {
                                auxMateriasACursar.Add(materia);
                        }
                        
                    }
                    foreach (var materia in auxMateriasACursar)
                    {
                        if (AgregarDiasACursar(materia.Dia, ref diasQueCursa, filtroCantDias) && VerificarFiltroDias(filtroDias, materia.Dia))
                        {
                            mostrarMateriasACursarEsteCuatri.Add(materia);
                        }
                    }
                }
            }
            return mostrarMateriasACursarEsteCuatri;
        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/PlanificadorFinales/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("PlanificadorFinales/{matricula}")]
        public List<CursadoStatus> PlanificadorFinales(string matricula)
        {
            GetDictionaries(matricula);
            List<CursadoStatus> cursados = new List<CursadoStatus>();
            //List<CorrelativasCursadas> correlativa = new List<CorrelativasCursadas>();
            var aprDictionary = SessionManager.DiccionarioAprobadas as AprDictionary;
            var curDictionary = SessionManager.DiccionarioCursadas as CurDictionary;
            var repDictionary = SessionManager.DiccionarioReprobadas as RepDictionary;
            foreach (KeyValuePair<string, CurValue> entry in curDictionary)
            {
                bool reprobadas = false;
                int correlativas = 0;
                DateTime fechaAuxiliar = DateTime.Parse(entry.Value.Fecha);
                string fechaDeCursada = fechaAuxiliar.ToString("MMMM yyyy");
                string fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy");
                //Fecha de Vencimiento
                if (fechaAuxiliar.Month == 6)
                {
                    fechaAuxiliar = fechaAuxiliar.AddMonths(6);
                    fechaAuxiliar = fechaAuxiliar.AddYears(1);
                    fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy");
                }
                else
                {
                    if (fechaAuxiliar.Month == 7)
                    {
                        fechaAuxiliar = fechaAuxiliar.AddMonths(5);
                        fechaAuxiliar = fechaAuxiliar.AddYears(1);
                        fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy");
                    }
                    else
                    {
                        fechaAuxiliar = fechaAuxiliar.AddMonths(-6);
                        fechaAuxiliar = fechaAuxiliar.AddYears(2);
                        fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy");
                    }
                }
                correlativas = context.Correlativas.Where(a => (a.Codigo_Correlativa + "/" + a.Plan_Id) == entry.Key).ToList().Count;
                //Numero de Correlativas de la Materia Cursada

                //Si se reprobo o no
                if (repDictionary.ContainsKey(entry.Key))
                {
                    if (DateTime.Parse(repDictionary[entry.Key].Fecha) > DateTime.Parse(entry.Value.Fecha))
                    {
                        reprobadas = true;
                    }
                }
                //Usar AutoMapper
                cursados.Add(new CursadoStatus() { materiaCod = entry.Key, fechaCursada = fechaDeCursada, fechaVencimiento = fechaDeVencimiento, abr = entry.Value.Abr, nCorrelativas = correlativas, reprobado = reprobadas });
            }
            //Filtro materias que no se pueden rendir aunque esten cursadas
            foreach (CursadoStatus cur in cursados)
            {
                var correlativaAuxiliar = context.Correlativas.Where(x => (x.Materia_Id + "/" + x.Plan_Id) == cur.materiaCod).ToList();
                foreach (Correlativa corr in correlativaAuxiliar)
                {
                    string materia_correlativa = (corr.Codigo_Correlativa + "/" + corr.Plan_Id);
                    if ((!aprDictionary.ContainsKey(materia_correlativa)) && (!curDictionary.ContainsKey(materia_correlativa)))
                    {
                        cursados.Remove(cur);
                    }
                }

            }
            //Agrego sublista de correlativas cursadas pero no aprobadas de las materias que se pueden rendir
            int i = 0;
            foreach (CursadoStatus cur in cursados)
            {
                List<CorrelativasCursadas> correlativ = new List<CorrelativasCursadas>();
                var correlativaAuxiliar = context.Correlativas.Where(x => (x.Materia_Id + "/" + x.Plan_Id) == cur.materiaCod).ToList();
                foreach (Correlativa corr in correlativaAuxiliar)
                {
                    string materia_cursada = (corr.Codigo_Correlativa + "/" + corr.Plan_Id);
                    if ((curDictionary.ContainsKey(materia_cursada)) && (materia_cursada != cur.materiaCod))
                    {
                        string abreviatura = curDictionary[cur.materiaCod].Abr;
                        correlativ.Add(new CorrelativasCursadas() { materiaCod = materia_cursada, abr = abreviatura });
                    }
                }
                cursados[i].correlativasCursadas = correlativ;
                i++;
            }
            return cursados;
        }
        //Orden por fecha de vencimiento
        public List<CursadoStatus> FinalesPorVecimiento(string matricula)
        {
            List<CursadoStatus> Ls = PlanificadorFinales(matricula);
            Ls.OrderByDescending(p => p.fechaVencimiento);

            return Ls;


        }
        //Orden por numero de correlativas asociadas
        public List<CursadoStatus> FinalesPorCorrelativas(string matricula)
        {
            List<CursadoStatus> Ls = PlanificadorFinales(matricula);
            Ls.OrderByDescending(p => p.nCorrelativas);

            return Ls;
        }

        /// <summary>
        /// Devuelve el porcentaje aprobado de la carrera 
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public Estadisticas PorcentajeAprobado(string matricula)
        {
            GetDictionaries(matricula);

            var estadistica = new Estadisticas();

            estadistica.Aprobadas = SessionManager.DiccionarioAprobadas.Count;

            estadistica.Total = SessionManager.DiccionarioAprobadas.Count + SessionManager.DiccionarioCursadas.Count + SessionManager.DiccionarioNoCursadas.Count + SessionManager.DiccionarioPendientes.Count;

            var aux = (estadistica.Aprobadas * 100);

            estadistica.Porcentaje_Aprobado = decimal.Divide(aux, estadistica.Total);

            estadistica.Porcentaje_Faltante = 100 - estadistica.Porcentaje_Aprobado;

            return estadistica;
        }

        /// <summary>
        /// Devuelve el porcentaje cursado de la carrera
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public Estadisticas PorcentajeCursado(string matricula)
        {
            GetDictionaries(matricula);

            var estadistica = new Estadisticas();

            estadistica.Aprobadas = SessionManager.DiccionarioAprobadas.Count + SessionManager.DiccionarioCursadas.Count;

            estadistica.Total = SessionManager.DiccionarioAprobadas.Count + SessionManager.DiccionarioCursadas.Count + SessionManager.DiccionarioNoCursadas.Count + SessionManager.DiccionarioPendientes.Count;

            var aux = ((estadistica.Aprobadas) * 100);

            estadistica.Porcentaje_Aprobado = decimal.Divide(aux, estadistica.Total);

            estadistica.Porcentaje_Faltante = 100 - estadistica.Porcentaje_Aprobado;

            return estadistica;

        }

        /// <summary>
        /// Devuelve cuantas materias aprobó por año
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        public List<AprobadasPorAnio> EstadisticaPorAnio(string matricula)
        {
            GetDictionaries(matricula);

            List<AprobadasPorAnio> Ls = new List<AprobadasPorAnio>();

            foreach (KeyValuePair<string, AprValue> entry in SessionManager.DiccionarioAprobadas)
            {
                int fecha = int.Parse(entry.Value.Fecha.Substring(6, 4));

                var resultado = Ls.Exists(a => a.Anio == fecha);

                if (resultado == true)
                {
                    AprobadasPorAnio Reg = Ls.Find(a => a.Anio == fecha);
                    Reg.Aprobadas++;
                }
                else
                {
                    AprobadasPorAnio Registro = new AprobadasPorAnio();
                    Registro.Anio = int.Parse(entry.Value.Fecha.Substring(6, 4));
                    Registro.Aprobadas = 1;

                    Ls.Add(Registro);
                }
            }
            Ls.OrderBy(p => p.Anio);

            return Ls;
        }

        //clase para almacenar la cantidad de materias aprobadas por año
        public class AprobadasPorAnio
        {
            public int Anio { get; set; }
            public int Aprobadas { get; set; }
        }

        //clase para devolver los datos para los porcentajes de aprobado y cursado
        public class Estadisticas
        {
            public int Aprobadas { get; set; }
            public int Total { get; set; }
            public decimal Porcentaje_Aprobado { get; set; }
            public decimal Porcentaje_Faltante { get; set; }
        }

    }
}























