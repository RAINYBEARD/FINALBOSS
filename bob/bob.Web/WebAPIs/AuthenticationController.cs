using System.Web.Configuration;
using System.Web.Http;
using bob.Data;
using bob.Data.Entities;
using bob.Data.Usuario;
using bob.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bob.Controllers
{
    [RoutePrefix("api/v1/auth")]
    public class AuthenticationController : ApiController
    {
        private CaeceDBContext _ctx;
        private string _token = WebConfigurationManager.AppSettings.Get("CaeceWSToken");
        private CaeceWS.wbsTrans caeceWS = new CaeceWS.wbsTrans();

        public AuthenticationController()
        {
            _ctx = new CaeceDBContext(); 
        }

        // POST api/Account/Validate
        [AllowAnonymous]
        [Route("validate")]
        [HttpPost]
        public IHttpActionResult Validate(Usuario userModel) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (userModel.UserName.Length < 7) {
                userModel.UserName = " " + userModel.UserName;
            }
            var result = (bool)JObject.Parse(caeceWS.autenticacion(_token, userModel.UserName, userModel.Password))["autenticacion"][0]["esValido"];
            if (result)
            {
                return Ok(result);
            }
            else {
                return BadRequest("Validation Failed");
            }
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public IHttpActionResult Register(Register userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var aux = _ctx.Alumnos.Find(userModel.UserName);

            if (aux != null)
            {
                return BadRequest("User already exists");
            }
            
            var alumno = new Alumno();
            alumno.Matricula = userModel.UserName;
            alumno.Password = PasswordHash.HashPassword(userModel.Password);
            var result = _ctx.Alumnos.Add(alumno);
            _ctx.SaveChanges();

            return Ok();
        }

        // POST api/Account/ChangePassword
        [AllowAnonymous]
        [Route("changePassword")]
        [HttpPost]
        public IHttpActionResult ChangePassword(Register userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var alumno = _ctx.Alumnos.Find(userModel.UserName);

            if (alumno == null)
            {
                return BadRequest("User does not exist");
            }

            alumno.Matricula = userModel.UserName;
            alumno.Password = PasswordHash.HashPassword(userModel.Password);
            _ctx.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ctx.Dispose();
            }

            base.Dispose(disposing);
        }


    }
    

}
