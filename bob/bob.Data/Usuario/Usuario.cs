using System.ComponentModel.DataAnnotations;

namespace bob.Data.Usuario
{
    public class Usuario
    {
        [Required]
        [Display(Name = "Matricula")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de largo.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Password")]
        [Compare("Password", ErrorMessage = "El password y el password de confirmacion no son iguales" )]
        public string ConfirmPassword{ get; set; }
    }
}
