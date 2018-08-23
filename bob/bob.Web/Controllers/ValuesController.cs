using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using bob.Data;

namespace bob.Controllers
{
    public class ValuesController : ApiController
    {
        public readonly CaeceDbContext context = new CaeceDbContext();
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [Route("api/values/{materiaId}")]
        public string Get(int materiaId)
        {
            var materia = context.materias.First(x => x.materiaid == materiaId);
            return materia.abr;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
