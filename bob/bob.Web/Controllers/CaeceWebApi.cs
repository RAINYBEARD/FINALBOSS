using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using bob.Data;
using Newtonsoft.Json.Linq;

namespace bob.Controllers
{
    public class CaeceWebApi : ApiController
    {
        private readonly CaeceDbContext context = new CaeceDbContext();

        public void GetPlanDeEstudio()
        {
            var json = MockService.MockService.PlanDeEstudio();

            var jobjPlanEstudio = JObject.Parse(json);
            var jarrMateria = (JArray)jobjPlanEstudio["PlanEstudio"];

        }

    }
}
