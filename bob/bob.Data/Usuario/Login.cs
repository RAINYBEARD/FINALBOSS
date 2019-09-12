using System.ComponentModel.DataAnnotations;

namespace bob.Data.Usuario
{
    public class Login
    {
        [Required]
        [Display(Name = "Matricula")]
        public string Matricula { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de largo.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}
