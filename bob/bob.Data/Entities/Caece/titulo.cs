namespace bob.Data.Entities.Caece

{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using bob.Data.Entities.Authentication;
    [Table("caece.titulo")]
    public partial class titulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public titulo()
        {
            alumnos = new HashSet<alumno>();
            titulo_plan = new HashSet<titulo_plan>();
        }

        [Key]
        [StringLength(20)]
        public string plantit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<alumno> alumnos { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<titulo_plan> titulo_plan { get; set; }
    }
}
