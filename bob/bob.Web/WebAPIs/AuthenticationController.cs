using System.Web.Configuration;
using System.Web.Http;
using bob.Data;
using bob.Data.Entities;
using bob.Data.Usuario;
using bob.Helpers;
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
        public IHttpActionResult Validate(Usuario userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("La Matricula o Contraseña es incorrecta");
            }
            if (WebConfigurationManager.AppSettings.Get("Validacion") == "true")
            {

                userModel.UserName = userModel.UserName.PadLeft(7, ' ');
                var result = (bool)JObject.Parse(caeceWS.autenticacion(_token, userModel.UserName, userModel.Password))["autenticacion"][0]["esValido"];
                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest("La Validacion ha fallado");
                }
            }
            else
            {
                return Ok(true);
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

            userModel.UserName = userModel.UserName.PadLeft(7, ' ');
            var aux = _ctx.Alumnos.Find(userModel.UserName);

            if (aux != null)
            {
                return BadRequest("El usuario ya existe");
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
        [Route("changepassword")]
        [HttpPost]
        public IHttpActionResult ChangePassword(Register userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userModel.UserName = userModel.UserName.PadLeft(7, ' ');

            var alumno = _ctx.Alumnos.Find(userModel.UserName);

            if (alumno == null)
            {
                return BadRequest("El usuario no existe");
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
