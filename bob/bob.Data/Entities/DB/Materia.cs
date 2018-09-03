namespace bob.Data.Entities.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("caece.Materia")]
    public partial class Materia
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


        public short? Anio { get; set; }

        public short? Cuatrim { get; set; }

        public float? Mat_Modulos { get; set; }

        public virtual Correlativa Correlativa { get; set; }

        public virtual Materia_Descripcion Materia_Descripcion { get; set; }

        public virtual Titulo Titulo { get; set; }
    }
}
