namespace bob.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using bob.Data.Entities;
    public partial class CaeceDBContext : DbContext
    {
        public CaeceDBContext()
            : base("name=CaeceDBContext")
        {
        }

        public virtual DbSet<Alumno> Alumnos { get; set; }
        public virtual DbSet<Correlativa> Correlativas { get; set; }
        public virtual DbSet<Materia> Materias { get; set; }
        public virtual DbSet<Materia_Descripcion> Materias_Descripciones { get; set; }
        public virtual DbSet<Titulo> Titulos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Alumno>()
                .Property(e => e.Matricula)
                .IsUnicode(false);

            modelBuilder.Entity<Alumno>()
                .Property(e => e.Password)
                .IsUnicode(false);
            
            modelBuilder.Entity<Correlativa>()
                .Property(e => e.PCursar)
                .IsUnicode(false);

            modelBuilder.Entity<Correlativa>()
                .Property(e => e.PAprobar)
                .IsUnicode(false);

            modelBuilder.Entity<Correlativa>()
                .Property(e => e.Plan_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Correlativa>()
                .Property(e => e.Plan_Tit)
                .IsUnicode(false);

            modelBuilder.Entity<Materia>()
                .Property(e => e.Plan_Id)
                .IsUnicode(false);

            modelBuilder.Entity<Materia>()
                .Property(e => e.Plan_Tit)
                .IsUnicode(false);

            modelBuilder.Entity<Materia>()
                .HasMany(e => e.Correlativas)
                .WithRequired(e => e.Materia)
                .HasForeignKey(e => new { e.Codigo_Correlativa, e.Plan_Id, e.Plan_Tit, e.Titulo_Id })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Materia_Descripcion>()
                .Property(e => e.Mat_Des)
                .IsUnicode(false);

            modelBuilder.Entity<Materia_Descripcion>()
                .HasMany(e => e.Materias)
                .WithRequired(e => e.Materia_Descripcion)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Titulo>()
                .Property(e => e.Plan_Tit)
                .IsUnicode(false);

            modelBuilder.Entity<Titulo>()
                .Property(e => e.Tit_Des)
                .IsUnicode(false);

            modelBuilder.Entity<Titulo>()
                .HasMany(e => e.Materias)
                .WithRequired(e => e.Titulo)
                .HasForeignKey(e => new { e.Plan_Tit, e.Titulo_Id })
                .WillCascadeOnDelete(false);
        }
    }
}
