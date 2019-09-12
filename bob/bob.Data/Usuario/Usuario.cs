using System.ComponentModel.DataAnnotations;

namespace bob.Controllers
{
    public class Usuario
    {
        [Required]
        [StringLength(7, ErrorMessage = "El {0} debe tener al menos {2} caracteres de largo.", MinimumLength = 6)]
        [Display(Name = "Matricula")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres de largo.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}