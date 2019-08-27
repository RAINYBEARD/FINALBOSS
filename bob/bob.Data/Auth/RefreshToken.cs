using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bob.Data.Auth
{
    [Table("caece.Refresh_Token")]
    public class RefreshToken
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Subject { get; set; }
        [Required]
        [MaxLength(50)]
        public string Client_Id { get; set; }
        public DateTime Issued_Utc { get; set; }
        public DateTime Expires_Utc { get; set; }
        [Required]
        public string Protected_Ticket { get; set; }
    }
}
