namespace bob.Data.Entities.Caece
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("caece.materia_has_plan")]
    public partial class materia_has_plan
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int materiaid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int planid { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string plantit { get; set; }

        public int? anio { get; set; }

        public int? cuatrim { get; set; }

        public int? mat_modulos { get; set; }

        public virtual correlativa correlativa { get; set; }

        public virtual materia materia { get; set; }

        public virtual titulo_plan titulo_plan { get; set; }
    }
}
