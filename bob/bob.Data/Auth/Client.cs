using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bob.Data.Auth
{
    [Table("caece.Client")]
    public class Client
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Secret { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public ApplicationTypes Application_Type { get; set; }
        public bool Active { get; set; }
        public int Refresh_Token_Life_Time { get; set; }
        [MaxLength(100)]
        public string Allowed_Origin { get; set; }
    }
}
