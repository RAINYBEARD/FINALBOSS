using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private string _token;
        private CaeceWS.wbsTrans _caeceWS;

        public AuthenticationController()
        {
            _ctx = new CaeceDBContext();
            _token = WebConfigurationManager.AppSettings.Get("CaeceWSToken");
            _caeceWS = new CaeceWS.wbsTrans();
        }

        // POST api/Account/Validate
        [AllowAnonymous]
        [Route("validate")]
        [HttpPost]
        public IHttpActionResult Validate(Usuario userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Matricula o contraseña incorrectos. Recuerde que la matricula debe tener al menos 6 caracteres y la contraseña debe tener al menos 6 caracteres.");
            }
            if (WebConfigurationManager.AppSettings.Get("Validacion") == "true")
            {

                userModel.UserName = userModel.UserName.PadLeft(7, ' ');
                var result = (bool)JObject.Parse(_caeceWS.autenticacion(_token, userModel.UserName, userModel.Password))["autenticacion"][0]["esValido"];
                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest("La validación ha fallado. Por favor revise su matricula y contraseña e intentelo nuevamente.");
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
                return BadRequest("Matricula o contraseña incorrectos. Recuerde que la matricula debe tener al menos 6 caracteres y la contraseña debe tener al menos 6 caracteres.");
            }

            userModel.UserName = userModel.UserName.PadLeft(7, ' ');
            var aux = _ctx.Alumnos.Find(userModel.UserName);

            if (aux != null)
            {
                return BadRequest("La matricula ingresada ya existe.");
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
                return BadRequest("Matricula o contraseña incorrectos. Recuerde que la matricula debe tener al menos 6 caracteres y la contraseña debe tener al menos 6 caracteres.");
            }

            userModel.UserName = userModel.UserName.PadLeft(7, ' ');

            var alumno = _ctx.Alumnos.Find(userModel.UserName);

            if (alumno == null)
            {
                return BadRequest("La matricula ingresada no existe.");
            }

            alumno.Matricula = userModel.UserName;
            alumno.Password = PasswordHash.HashPassword(userModel.Password);
            _ctx.SaveChanges();

            return Ok();
        }

        [Authorize]
        [Route("is-admin")]
        [HttpGet]
        public IHttpActionResult IsAdmin()
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;

            return Ok(claimsIdentity.Claims.Any(x => x.Type == "role" && x.Value == "admin"));
        }

        [Authorize]
        [HttpGet]
        [Route("get-alumnos")]
        public IHttpActionResult GetUsuarios()
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity.Claims.Any(x => x.Type == "role" && x.Value == "admin"))
            {
                var usuarios = _ctx.Alumnos.Where(x => !x.Matricula.Equals("0000000")).ToList();
                return Ok(usuarios);
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpDelete]
        [Route("borrar-alumno/{matricula}")]
        public IHttpActionResult BorrarUsuario(string matricula)
        {
            ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity.Claims.Any(x => x.Type == "role" && x.Value == "admin"))
            {
                var usuario = _ctx.Alumnos.First(x => !x.Matricula.Equals("0000000") && x.Matricula.Equals(matricula));
                _ctx.Alumnos.Remove(usuario);
                _ctx.SaveChanges();
                return Ok();
            }
            return Unauthorized();
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
