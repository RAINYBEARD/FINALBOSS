namespace bob.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("caece.Alumno")]
    public partial class Alumno
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(7)]
        public string Matricula { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(3)]
        public string Plan_Tit { get; set; }

        public int? Titulo_Id { get; set; }

        [StringLength(100)]
        public string Abr_Titulo { get; set; }

    }
}
