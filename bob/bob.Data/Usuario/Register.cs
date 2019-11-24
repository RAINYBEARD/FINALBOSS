using System.ComponentModel.DataAnnotations;

namespace bob.Data.Usuario
{
    public class Register
    {
        [Required]
        [StringLength(7, ErrorMessage = "El {0} debe tener al menos {2} caracteres de largo.", MinimumLength = 6)]
        [Display(Name = "Matricula")]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "El {0} debe tener al menos {2} caracteres de largo.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Password")]
        [Compare("Password", ErrorMessage = "La contraseña y la contraseña de confirmacion deben ser iguales" )]
        public string ConfirmPassword{ get; set; }
    }
}
