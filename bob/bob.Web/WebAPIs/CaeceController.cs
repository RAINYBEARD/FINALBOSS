using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Threading;
using bob.Data;
using bob.Data.Entities;
using bob.Data.DTOs;
using bob.Data.Dictionaries;
using bob.Data.Finales;
using bob.Data.Arbol;
using bob.Data.Estadisticas;
using bob.Data.Pendientes;
using bob.Data.Vencerse;
using Newtonsoft.Json.Linq;
using bob.CaeceWS;
using System.Web.Configuration;
using bob.Mocks;
using bob.Helpers;
using System.Globalization;
using Newtonsoft.Json;

namespace bob.Controllers
{
    [Authorize]
    [RoutePrefix("api/v1/caece")]
    public class CaeceController : ApiController
    {
        private readonly CaeceDBContext context = new CaeceDBContext();
        private string _token = WebConfigurationManager.AppSettings.Get("CaeceWSToken");
        private CaeceWS.wbsTrans caeceWS = new CaeceWS.wbsTrans();

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/api/v1/caece/save-plan-estudio/951282 
        /// Guarda el Plan de Estudio de la Persona
        /// </summary>
        /// <param name="matricula"></param>
        [HttpPost]
        [Route("save-plan-estudio/{matricula}")]
        public void SavePlanDeEstudio(string matricula)
        {
            if (matricula.Length == 6)
            {
                matricula = " " + matricula;
            }
            var JSON = caeceWS.getPlanEstudioJSON(_token, matricula);
            var JSON2 = caeceWS.getHistoriaAcademicaJSON(_token, matricula);
            var PlanDeEstudio = ((JArray)JObject.Parse(JSON)["PlanEstudio"]).ToObject<List<PlanEstudio>>();
            if (PlanDeEstudio.Count > 0)
            {
                SessionManager.TituloId = PlanDeEstudio[0].titulo_id;
                SessionManager.PlanTit = PlanDeEstudio[0].plan_tit;

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
                                var correlativa = new Correlativa();
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
        }

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/api/v1/caece/set-sesion-usuario/951282
        /// Carga los diccionarios 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpPost]
        [Route("set-sesion-usuario/{matricula}")]
        public void SetSesionUsuario(string matricula)
        {
            // Para hacer la llamada al WS
            var JSONCursos = caeceWS.getCursosAbiertosJSON(_token);
            var cursosAbiertos = ((JArray)JObject.Parse(JSONCursos)["Cursos"]).ToObject<List<Curso>>();
            var JSONHistoriaAcademica = caeceWS.getHistoriaAcademicaJSON(_token, " " + matricula);
            var historiaAcademiaCompleta = ((JArray)JObject.Parse(JSONHistoriaAcademica)["HistoriaAcademica"]).ToObject<List<HistoriaAcademica>>();

            var aprDictionary = new AprDictionary();
            var curDictionary = new CurDictionary();
            var penDictionary = new PenDictionary();
            var notCurDictionary = new NotCurDictionary();
            var mesaFinalDictionary = new MesasDictionary();
            var cursosDictionary = new CursosDictionary();
            var repDictionary = new RepDictionary();

            try
            {
                foreach (HistoriaAcademica dato in historiaAcademiaCompleta)
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
                        case ("EQP"):
                            CurValue cur2 = new CurValue();
                            AutoMapper.Mapper.Map(dato, cur2);
                            curDictionary.Add(matcod, cur2);
                            break;
                        case ("PEN"):
                            PenValue equiv = new PenValue();
                            AutoMapper.Mapper.Map(dato, equiv);
                            penDictionary.Add(matcod, equiv);
                            break;
                        case ("REP"):
                            if ((repDictionary != null) && (repDictionary.ContainsKey(matcod)))
                            {
                                if (DateTime.ParseExact(dato.Fecha, "dd/MM/yyyy", null) > DateTime.ParseExact(repDictionary[matcod].Fecha, "dd/MM/yyyy", null))
                                {
                                    repDictionary[matcod].Fecha = dato.Fecha;
                                }
                            }
                            else
                            {
                                RepValue rep = new RepValue();
                                AutoMapper.Mapper.Map(dato, rep);
                                repDictionary.Add(matcod, rep);
                            }
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
            }
            catch (Exception e)
            {

                throw;
            }

            foreach (var dato in cursosAbiertos)
            {
                int i = 0;
                while (i < 7 && (dato.Dia.Substring(i, 1) == "0" || dato.Dia.Substring(i, 1) == "1"))
                {
                    i++;
                }
                if (i < 7)
                {
                    System.Diagnostics.Debug.WriteLine("Materia con cursada de medio dia : " + dato.Materia_Id + "/" + dato.Plan_Id + "  El string de dias es : " + dato.Dia);
                }

                // Comento para hacer la prueba con los dias que tienen 2 y 3
                // if (!cursosDictionary.ContainsKey(dato.Materia_Id + "/" + dato.Plan_Id) && notCurDictionary.ContainsKey(dato.Materia_Id + "/" + dato.Plan_Id))
                if (!cursosDictionary.ContainsKey(dato.Materia_Id + "/" + dato.Plan_Id))
                {
                    CursosValue curso = new CursosValue();
                    AutoMapper.Mapper.Map(dato, curso);
                    cursosDictionary.Add(dato.Materia_Id + "/" + dato.Plan_Id, curso);
                }
            }

            SessionManager.DiccionarioAprobadas = aprDictionary;
            SessionManager.DiccionarioCursadas = curDictionary;
            SessionManager.DiccionarioPendientes = penDictionary;
            SessionManager.DiccionarioNoCursadas = notCurDictionary;
            SessionManager.DiccionarioCursos = cursosDictionary;
            SessionManager.DiccionarioReprobadas = repDictionary;

            #region Diccionario Correlativas
            using (var context = new CaeceDBContext())
            {
                try
                {
                    var listCorrelativas = context.Correlativas.Where(a => a.Titulo_Id == SessionManager.TituloId && a.Plan_Tit == SessionManager.PlanTit).ToList();
                    var existentes = new List<CorrValue>();
                    var dicCorrelativas = new CorrDictionary();

                    foreach (var correlativa in listCorrelativas)
                    {
                        var corr = new CorrValue();
                        AutoMapper.Mapper.Map(correlativa, corr);

                        if (!dicCorrelativas.ContainsKey(correlativa.Materia_Id))
                        {
                            var listCorrel = new List<CorrValue>();
                            listCorrel.Add(corr);
                            dicCorrelativas.Add(correlativa.Materia_Id, listCorrel);
                        }
                        else
                        {
                            if (dicCorrelativas.TryGetValue(correlativa.Materia_Id, out existentes))
                            {
                                existentes.Add(corr);
                                dicCorrelativas[correlativa.Materia_Id] = existentes;
                            }
                        }
                    }
                    SessionManager.DiccionarioCorrelativas = dicCorrelativas;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            #endregion

        }


        #region Cursos

        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/GetMateriasACursar/951282 
        /// </summary>
        /// <param name="matricula"></param>
        public List<string> GetMateriasACursar(string matricula)
        {
            var tiempoInicio = DateTime.Now;

            List<string> materiasACursar = new List<string>();
            List<CorrValue> materiasParaBuscarCorrelativas = new List<CorrValue>();
            string materiaAnt = "";

            System.Diagnostics.Debug.WriteLine("Entro en el controller Cursos");

            if (SessionManager.DiccionarioCursadas != null)
            {

                foreach (var resultado0 in SessionManager.DiccionarioNoCursadas)
                {
                    // Descompongo la materiaid del planid
                    string[] matriculaId = resultado0.Key.Split(new Char[] { '/' });
                    var listaCorrelativas = BuscarCorrelativa(int.Parse(matriculaId[0]));

                    // Evaluacion para la materias que no tienen correlativas
                    if (listaCorrelativas.Count == 1)
                        materiasACursar.Add(listaCorrelativas[0].materia_id + "/" + listaCorrelativas[0].plan_id);

                    foreach (var correlativa in listaCorrelativas)
                    {
                        // Para no evaluar las misma materia id que figura como correlativa
                        if (correlativa.materia_id != correlativa.codigo_correlativa)
                        {
                            // Elimino los resultados repetidos
                            if (!materiasParaBuscarCorrelativas.Any(x => x.materia_id == correlativa.materia_id))
                                materiasParaBuscarCorrelativas.Add(correlativa);
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
                return SessionManager.DiccionarioCorrelativas[idMateria];
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

        public string ObtenerNombreMateria(int materiaid)
        {
            using (var context = new CaeceDBContext())
            {
                return context.Materias_Descripciones.First(x => x.Materia_Id == materiaid).Mat_Des;
            }
        }

        private void BuscarMateriasACursar(CorrValue correlativa, ref List<string> materiasACursar, string materiaAnt)
        {
            //CHEQUEAR QUE LOS DICCIONARIOS ESTEN CARGADOS ANTES DE EMPEZAR A PROCESAR
            if (SessionManager.DiccionarioCursadas != null)
            {

                // Busco las correlativas dentro del diccionario de las materias no cursadas
                if (SessionManager.DiccionarioNoCursadas.ContainsKey(correlativa.materia_id + "/" + correlativa.plan_id))
                {
                    // Verifico asignaturas que requieren cantidad de materias aprobadas
                    if (correlativa.codigo_correlativa < 100 && correlativa.codigo_correlativa <= (SessionManager.DiccionarioAprobadas.Count() + SessionManager.DiccionarioCursadas.Count()))
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
        /// Ejemplo de llamada: http://localhost:52178/Caece/GetMateriasACursarCuatrimestreActual/?matricula=951282 
        /// 
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-cursos/{matricula}")]
        public List<Curso> GetCursosCuatrimestreActual(string matricula)
        {
            List<string> materiasACursar = GetMateriasACursar(matricula);
            List<Curso> materiasACursarEsteCuatri = new List<Curso>();


            foreach (var materia in materiasACursar)
            {
                // Descompongo la materiaid del planid
                string materiaid = materia.Split(new Char[] { '/' })[0];

                // Verifico los cursos que puede cursar este cuatrimestre
                if (SessionManager.DiccionarioCursos.ContainsKey(materia))
                {
                    var curso = SessionManager.DiccionarioCursos[materia];
                    Curso cursomateria = new Curso();
                    cursomateria.Materia_Id = materiaid;
                    cursomateria.Dia = curso.Dia;
                    cursomateria.M_Acobrar = curso.M_Acobrar;
                    cursomateria.Plan_Id = curso.Plan_Id;
                    cursomateria.Turno_Id = curso.Turno_Id;
                    cursomateria.Abr = ObtenerNombreMateria(int.Parse(materiaid));

                    // Agrego a la lista los cursos a los cuales se puede inscribir
                    materiasACursarEsteCuatri.Add(cursomateria);
                }
            }
            return materiasACursarEsteCuatri;
        }

        public bool VerificarFiltroDias(string filtro, string diaMateria)
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

        public bool AgregarDiasACursar(string diaMateria, ref string diasACursar, int filtroCantDias)
        {
            int i = 0;
            List<int> dias = new List<int>();
            int cantDias = 0;
            while (i < 7 && ((diaMateria.Substring(i, 1) == "1" && diasACursar.Substring(i, 1) == "0") ||
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

        public List<Curso> MostrarMateriasACursarCuatrimestreActual(string matricula, string filtroDias, int filtroCantDias, string modo)
        {
            List<Curso> materiasACursarEsteCuatri = GetCursosCuatrimestreActual(matricula);
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
        #endregion

        #region Pendientes
        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/Pendientes/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("get-pendientes/{matricula}")]
        public List<Pendientes> Pendientes(string matricula)
        {
            try
            {
                List<Pendientes> pendientes = new List<Pendientes>();
                var aprDictionary = SessionManager.DiccionarioAprobadas as AprDictionary;
                var penDictionary = SessionManager.DiccionarioPendientes as PenDictionary;
                //Agrego las materias pendientes a una lista
                foreach (KeyValuePair<string, PenValue> entry in penDictionary)
                {
                    List<CorrelativasNoAprobadas> correlativ = new List<CorrelativasNoAprobadas>();
                    var correlativaAuxiliar = context.Correlativas.Where(x => x.Titulo_Id == SessionManager.TituloId && x.Plan_Tit == SessionManager.PlanTit && (x.Materia_Id + "/" + x.Plan_Id) == entry.Key).ToList();

                    foreach (Correlativa corr in correlativaAuxiliar)
                    {
                        string materiaCursada = (corr.Codigo_Correlativa + "/" + corr.Plan_Id);

                        if ((!aprDictionary.ContainsKey(materiaCursada)) && (materiaCursada != entry.Key))
                        {
                            string abreviatura = corr.Materia.Materia_Descripcion.Mat_Des;
                            correlativ.Add(new CorrelativasNoAprobadas() { materiaCod = materiaCursada, abr = abreviatura });
                        }

                    }

                    pendientes.Add(new Pendientes() { materiaCod = entry.Key, abr = entry.Value.Abr, correlativasNoAprobadas = correlativ });
                }
                return pendientes;
            }
            catch (Exception e)
            {

                throw;
            }
        }

        #endregion

        #region PorVencerse
        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/AlertaPorVencer/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("get-porvencerse/{matricula}")]
        public List<PorVencerse> AlertaPorVencer(string matricula)
        {
            List<PorVencerse> materias = new List<PorVencerse>();
            var curDictionary = SessionManager.DiccionarioCursadas as CurDictionary;
            foreach (KeyValuePair<string, CurValue> entry in curDictionary)
            {
                DateTime fechaAuxiliar = DateTime.ParseExact(entry.Value.Fecha, "dd/MM/yyyy", null);
                string fechaDeCursada = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                string fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                //Fecha de Vencimiento
                if (fechaAuxiliar.Month == 6)
                {
                    fechaAuxiliar = fechaAuxiliar.AddMonths(6);
                    fechaAuxiliar = fechaAuxiliar.AddYears(1);
                    fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                }
                else
                {
                    if (fechaAuxiliar.Month == 7)
                    {
                        fechaAuxiliar = fechaAuxiliar.AddMonths(5);
                        fechaAuxiliar = fechaAuxiliar.AddYears(1);
                        fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                    }
                    else
                    {
                        if (fechaAuxiliar.Month == 8)
                        {
                            fechaAuxiliar = fechaAuxiliar.AddMonths(4);
                            fechaAuxiliar = fechaAuxiliar.AddYears(1);
                            fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                        }

                        else
                        {
                            if (fechaAuxiliar.Month == 11)
                            {
                                fechaAuxiliar = fechaAuxiliar.AddMonths(-4);
                                fechaAuxiliar = fechaAuxiliar.AddYears(2);
                                fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                            }
                            else
                            {
                                fechaAuxiliar = fechaAuxiliar.AddMonths(-5);
                                fechaAuxiliar = fechaAuxiliar.AddYears(2);
                                fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                            }
                        }
                    }
                }

                //Chequea si la materia se esta por vencer
                if (fechaDeVencimiento == DateTime.Now.ToString("MMMM yyyy", new CultureInfo("es-ES")) || fechaDeVencimiento == DateTime.Now.AddMonths(1).ToString("MMMM yyyy", new CultureInfo("es-ES")) || fechaDeVencimiento == DateTime.Now.AddMonths(2).ToString("MMMM yyyy", new CultureInfo("es-ES")) || fechaDeVencimiento == DateTime.Now.AddMonths(3).ToString("MMMM yyyy", new CultureInfo("es-ES")))
                {
                    materias.Add(new PorVencerse() { materiaCod = entry.Key, abr = entry.Value.Abr });
                }
            }
            return materias;
        }
        #endregion

        #region Finales
        /// <summary>
        /// Ejemplo de llamada: http://localhost:52178/Caece/PlanificadorFinales/951282 
        /// </summary>
        /// <param name="matricula"></param>
        [HttpGet]
        [Route("get-finales/{matricula}")]
        public List<CursadoStatus> PlanificadorFinales(string matricula)
        {
            try
            {
                List<CursadoStatus> cursados = new List<CursadoStatus>();
                var aprDictionary = SessionManager.DiccionarioAprobadas as AprDictionary;
                var curDictionary = SessionManager.DiccionarioCursadas as CurDictionary;
                var repDictionary = SessionManager.DiccionarioReprobadas as RepDictionary;
                var penDictionary = SessionManager.DiccionarioPendientes as PenDictionary;
                var notCurDictionary = SessionManager.DiccionarioNoCursadas as NotCurDictionary;
                foreach (KeyValuePair<string, CurValue> entry in curDictionary)
                {
                    string reprobadas = "No";
                    int correlativas = 0;

                    DateTime fechaAuxiliar = DateTime.ParseExact(entry.Value.Fecha, "dd/MM/yyyy", null);
                    string fechaDeCursada = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                    string fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                    //Fecha de Vencimiento
                    if (fechaAuxiliar.Month == 6)
                    {
                        fechaAuxiliar = fechaAuxiliar.AddMonths(6);
                        fechaAuxiliar = fechaAuxiliar.AddYears(1);
                        fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                    }
                    else
                    {
                        if (fechaAuxiliar.Month == 7)
                        {
                            fechaAuxiliar = fechaAuxiliar.AddMonths(5);
                            fechaAuxiliar = fechaAuxiliar.AddYears(1);
                            fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                        }
                        else
                        {
                            if (fechaAuxiliar.Month == 8)
                            {
                                fechaAuxiliar = fechaAuxiliar.AddMonths(4);
                                fechaAuxiliar = fechaAuxiliar.AddYears(1);
                                fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                            }

                            else
                            {
                                if (fechaAuxiliar.Month == 11)
                                {
                                    fechaAuxiliar = fechaAuxiliar.AddMonths(-4);
                                    fechaAuxiliar = fechaAuxiliar.AddYears(2);
                                    fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                                }
                                else
                                {
                                    fechaAuxiliar = fechaAuxiliar.AddMonths(-5);
                                    fechaAuxiliar = fechaAuxiliar.AddYears(2);
                                    fechaDeVencimiento = fechaAuxiliar.ToString("MMMM yyyy", new CultureInfo("es-ES"));
                                }
                            }
                        }
                    }
                    correlativas = context.Correlativas.Where(a => a.Titulo_Id == SessionManager.TituloId && a.Plan_Tit == SessionManager.PlanTit && (a.Codigo_Correlativa + "/" + a.Plan_Id) == entry.Key).ToList().Count;
                    //Numero de Correlativas de la Materia Cursada

                    //Si se reprobo o no

                    if ((repDictionary != null) && (repDictionary.ContainsKey(entry.Key)))
                    {
                        switch (entry.Value.Descrip)
                        {
                            case ("EQP"):
                                reprobadas = "Si";
                                break;
                            default:
                                if (DateTime.ParseExact(repDictionary[entry.Key].Fecha, "dd/MM/yyyy", null) > DateTime.ParseExact(entry.Value.Fecha, "dd/MM/yyyy", null))
                                {
                                    reprobadas = "Si";
                                }
                                break;

                        }
                    }

                    //Usar AutoMapper
                    cursados.Add(new CursadoStatus() { materiaCod = entry.Key, fechaCursada = fechaDeCursada, fechaVencimiento = fechaDeVencimiento, abr = entry.Value.Abr, descrip = entry.Value.Descrip, nCorrelativas = correlativas, reprobado = reprobadas });
                }
                //Filtro materias que no se pueden rendir aunque esten cursadas
                int totalCursadas = cursados.Count;
                int z = 0;
                var elimDictionary = new ElimDictionary();
                while (z < totalCursadas)
                {
                    bool seEleminoLaMateria = false;
                    CursadoStatus cur = cursados[z];
                    var correlativaAuxiliar = context.Correlativas.Where(x => x.Titulo_Id == SessionManager.TituloId && x.Plan_Tit == SessionManager.PlanTit && (x.Materia_Id + "/" + x.Plan_Id) == cur.materiaCod).ToList();
                    foreach (Correlativa corr in correlativaAuxiliar)
                    {
                        string materia_correlativa = (corr.Codigo_Correlativa + "/" + corr.Plan_Id);

                        if ((((!aprDictionary.ContainsKey(materia_correlativa)) && (!curDictionary.ContainsKey(materia_correlativa))) || (elimDictionary.ContainsKey(materia_correlativa))))
                        {
                            string materiaEliminada = cur.materiaCod;
                            string abr = cur.abr;
                            elimDictionary.Add(materiaEliminada, abr);
                            cursados.Remove(cur);
                            totalCursadas = cursados.Count;
                            seEleminoLaMateria = true;
                            break;
                        }
                        //}
                    }
                    if (seEleminoLaMateria == false)
                    {
                        z++;
                    }
                }

                //Agrego sublista de correlativas cursadas pero no aprobadas de las materias que se pueden rendir y de materias que destrabaria se se aprueba
                int i = 0;
                foreach (CursadoStatus cur in cursados)
                {
                    List<CorrelativasCursadas> correlativ = new List<CorrelativasCursadas>();
                    List<CorrelativasCursadas> correlativ2 = new List<CorrelativasCursadas>();
                    var correlativaAuxiliar = context.Correlativas.Where(x => x.Titulo_Id == SessionManager.TituloId && x.Plan_Tit == SessionManager.PlanTit && (x.Materia_Id + "/" + x.Plan_Id) == cur.materiaCod).ToList();
                    var correlativaAuxiliar2 = context.Correlativas.Where(y => y.Titulo_Id == SessionManager.TituloId && y.Plan_Tit == SessionManager.PlanTit && (y.Codigo_Correlativa + "/" + y.Plan_Id) == cur.materiaCod).ToList();
                    foreach (Correlativa corr in correlativaAuxiliar)
                    {
                        string materiaCursada = (corr.Codigo_Correlativa + "/" + corr.Plan_Id);
                        if ((curDictionary.ContainsKey(materiaCursada)) && (materiaCursada != cur.materiaCod))
                        {
                            string abreviatura = curDictionary[materiaCursada].Abr;
                            correlativ.Add(new CorrelativasCursadas() { materiaCod = materiaCursada, abr = abreviatura });
                        }
                    }
                    foreach (Correlativa corr in correlativaAuxiliar2)
                    {
                        string materiaNoCursada = (corr.Materia_Id + "/" + corr.Plan_Id);
                        bool correlativasHechas = true;
                        var correlativaAuxiliar3 = context.Correlativas.Where(w => w.Titulo_Id == SessionManager.TituloId && w.Plan_Tit == SessionManager.PlanTit && w.Codigo_Correlativa + "/" + w.Plan_Id == materiaNoCursada).ToList();
                        foreach (Correlativa corr2 in correlativaAuxiliar3)
                        {
                            if ((!curDictionary.ContainsKey(corr2.Codigo_Correlativa + "/" + corr2.Plan_Id)) && (!aprDictionary.ContainsKey(corr2.Codigo_Correlativa + "/" + corr2.Plan_Id) && (corr2.Codigo_Correlativa + "/" + corr2.Plan_Id != materiaNoCursada)))
                            {
                                correlativasHechas = false;
                            }
                        }
                        if ((correlativasHechas == true) && (materiaNoCursada != cur.materiaCod))
                        {
                            string abreviatura;
                            if (curDictionary.ContainsKey(materiaNoCursada))
                            {
                                abreviatura = curDictionary[materiaNoCursada].Abr;
                            }
                            else
                            {
                                abreviatura = notCurDictionary[materiaNoCursada].Abr;
                            }
                            correlativ2.Add(new CorrelativasCursadas() { materiaCod = materiaNoCursada, abr = abreviatura });
                        }

                    }
                    cursados[i].correlativasCursadas = correlativ;
                    cursados[i].correlativasFuturas = correlativ2;
                    i++;
                }
                return cursados;
            }
            catch (Exception e)
            {

                throw;
            }

        }
        #endregion

        #region Estadisticas
        /// <summary>
        /// Devuelvo todas las estadisticas en una sola estructura
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-estadisticas/{matricula}")]
        public Estadisticas EstadisticasAlumno(string matricula)
        {
            Estadisticas estadistica = new Estadisticas();
            estadistica.Lista = new List<AprobadasPorAnio>();
            //SetSesionUsuario(matricula);
            var JSON = caeceWS.getPlanEstudioJSON(_token, " " + matricula);
            var PlanDeEstudio = ((JArray)JObject.Parse(JSON)["PlanEstudio"]).ToObject<List<PlanEstudio>>();
            var aprDictionary = SessionManager.DiccionarioAprobadas as AprDictionary;
            var curDictionary = SessionManager.DiccionarioCursadas as CurDictionary;
            var notCurDictionary = SessionManager.DiccionarioNoCursadas as NotCurDictionary;
            var penDictionary = SessionManager.DiccionarioPendientes as PenDictionary;

            //Por cada aprobada veo si sumo una aprobada al registro existente de la lista, o agrego otro registro si no existe
            foreach (KeyValuePair<string, AprValue> entry in aprDictionary)
            {
                int fecha = int.Parse(entry.Value.Fecha.Substring(6, 4));

                var resultado = estadistica.Lista.Exists(a => a.Anio == fecha);

                if (resultado == true)
                {
                    AprobadasPorAnio Reg = estadistica.Lista.Find(a => a.Anio == fecha);
                    Reg.Aprobadas++;
                }
                else
                {
                    AprobadasPorAnio Registro = new AprobadasPorAnio();
                    Registro.Anio = int.Parse(entry.Value.Fecha.Substring(6, 4));
                    Registro.Aprobadas = 1;

                    estadistica.Lista.Add(Registro);
                }
            }
            estadistica.Lista.OrderBy(p => p.Anio);

            foreach (PlanEstudio dato in PlanDeEstudio)
            {
                var mat_id = (dato.materia_id.ToString()) + "/" + (dato.plan_id.ToString());
                estadistica.Total++;
                if (aprDictionary.ContainsKey(mat_id))
                {
                    estadistica.Aprobadas++;
                    estadistica.Cursadas++;
                }
                if ((curDictionary.ContainsKey(mat_id)) && (penDictionary.ContainsKey(mat_id)))
                {
                    estadistica.Cursadas++;
                }
            }



            return estadistica;

        }
        #endregion

        #region Arbol

        /// <summary>
        /// Devuelvo estructura del Arbol; que tiene una lista de nodos y una lista de arcos
        /// </summary>
        /// <param name="matricula"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-arbol/{matricula}")]

        public Tabla GetArbol(string matricula)
        {
            Tabla tabla = new Tabla();
            tabla.materias = new List<Materias>();

            var JSON = caeceWS.getPlanEstudioJSON(_token, " " + matricula);
            var PlanDeEstudio = ((JArray)JObject.Parse(JSON)["PlanEstudio"]).ToObject<List<PlanEstudio>>();

            try
            {
                foreach (PlanEstudio dato in PlanDeEstudio)
                {
                    //var id = dato.materia_id.ToString();
                    var resultado = tabla.materias.Exists(a => a.materia_id == dato.materia_id);

                    if (resultado == false)
                    {
                        var reg = new Materias();

                        reg.materia_id = dato.materia_id;
                        reg.plan_id = dato.plan_id;
                        reg.mat_des = dato.mat_des;
                        reg.mat_anio = dato.mat_anio;

                        //hacer el casteo de cuatrimestre para mostrar vacio en vez de un 3
                        if (dato.mat_cuatrim == 1 || dato.mat_cuatrim == 2)
                        {
                            reg.mat_cuatrim = dato.mat_cuatrim.ToString();
                        }
                        else
                        {
                            reg.mat_cuatrim = " ";
                        }


                        reg.correlativas = new List<Correlativas>();

                        tabla.total = tabla.total + 1;

                        tabla.materias.Add(reg);

                    }

                    if (dato.materia_id != dato.codigo_correlativa)
                    {

                        var materia = tabla.materias.Find(x => x.materia_id == dato.materia_id);
                        var reg = new Correlativas();

                        reg.materia_id = dato.codigo_correlativa;
                        reg.materia_des = dato.descripcion_correlativa;

                        materia.correlativas.Add(reg);

                    }


                }


            }
            catch (Exception e)
            {
                throw;
            }

            //SetSesionUsuario(matricula);
            var aprDictionary = SessionManager.DiccionarioAprobadas as AprDictionary;
            var curDictionary = SessionManager.DiccionarioCursadas as CurDictionary;
            var NoCurDictionary = SessionManager.DiccionarioNoCursadas as NotCurDictionary;
            var PenDictionary = SessionManager.DiccionarioPendientes as PenDictionary;

            foreach (Materias dato in tabla.materias)
            {
                var mat_id = (dato.materia_id.ToString()) + "/" + (dato.plan_id.ToString());

                if (aprDictionary.ContainsKey(mat_id))
                {
                    //dato.estado = aprDictionary[mat_id].Descrip;
                    dato.estado = "Aprobada";

                    tabla.aprobadas = tabla.aprobadas + 1;
                }

                if (curDictionary.ContainsKey(mat_id))
                {
                    //dato.estado = curDictionary[mat_id].Descrip;
                    dato.estado = "Cursada";
                }

                if (NoCurDictionary.ContainsKey(mat_id))
                {
                    //dato.estado = NoCurDictionary[mat_id].Descrip;
                    dato.estado = "Sin Cursar";
                }

                if (PenDictionary.ContainsKey(mat_id))
                {
                    //dato.estado = PenDictionary[mat_id].Descrip;
                    dato.estado = "Pendiente";
                }

            }
            //var json = JsonConvert.SerializeObject(tabla);
            return tabla;

        }
        #endregion
    }
}























