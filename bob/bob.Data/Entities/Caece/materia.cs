namespace bob.Data.Entities.Caece
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("caece.materia")]
    public partial class materia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public materia()
        {
            materia_has_plan = new HashSet<materia_has_plan>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int materiaid { get; set; }

        [StringLength(45)]
        public string abr { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<materia_has_plan> materia_has_plan { get; set; }
    }
}
