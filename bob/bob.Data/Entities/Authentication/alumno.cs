namespace bob.Data.Entities.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using bob.Data.Entities.Caece;

    [Table("caece.alumno")]
    public partial class alumno
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(20)]
        public string matricula { get; set; }

        [StringLength(45)]
        public string password { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string plantit { get; set; }

        public virtual titulo titulo { get; set; }
    }
}
