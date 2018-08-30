namespace bob.Data.Entities.Caece
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("caece.correlativas")]
    public partial class correlativa
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

        public int? codigo_correlativa { get; set; }

        public int? pcursar { get; set; }

        public int? paprobar { get; set; }

        public virtual materia_has_plan materia_has_plan { get; set; }
    }
}
