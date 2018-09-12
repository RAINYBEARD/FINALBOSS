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

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string Plan_Tit { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Titulo_Id { get; set; }

        [StringLength(40)]
        public string Password { get; set; }

        public virtual Titulo Titulo { get; set; }
    }
}
