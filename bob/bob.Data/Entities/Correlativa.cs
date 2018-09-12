namespace bob.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("caece.Correlativa")]
    public partial class Correlativa
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Materia_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string Plan_Id { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(3)]
        public string Plan_Tit { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Titulo_Id { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Codigo_Correlativa { get; set; }

        [StringLength(5)]
        public string PCursar { get; set; }

        [StringLength(5)]
        public string PAprobar { get; set; }

        public virtual Materia Materia { get; set; }
    }
}
