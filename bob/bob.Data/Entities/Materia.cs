namespace bob.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("caece.Materia")]
    public partial class Materia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Materia()
        {
            Correlativas = new HashSet<Correlativa>();
        }

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



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Correlativa> Correlativas { get; set; }
        public virtual Materia_Descripcion Materia_Descripcion { get; set; }

        public virtual Titulo Titulo { get; set; }
    }
}
