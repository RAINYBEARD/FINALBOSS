namespace bob.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("caece.titulo_plan")]
    public partial class titulo_plan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public titulo_plan()
        {
            materia_has_plan = new HashSet<materia_has_plan>();
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int planid { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string plantit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<materia_has_plan> materia_has_plan { get; set; }

        public virtual titulo titulo { get; set; }
    }
}
