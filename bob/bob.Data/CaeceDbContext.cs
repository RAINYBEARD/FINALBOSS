namespace bob.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using bob.Data.Entities.Authentication;
    using bob.Data.Entities.Caece;
    public partial class CaeceDbContext : DbContext
    {
        public CaeceDbContext()
            : base("name=CaeceDbContext")
        {
        }

        public virtual DbSet<alumno> alumnos { get; set; }
        public virtual DbSet<correlativa> correlativas { get; set; }
        public virtual DbSet<materia> materias { get; set; }
        public virtual DbSet<materia_has_plan> materia_has_plan { get; set; }
        public virtual DbSet<titulo> titulos { get; set; }
        public virtual DbSet<titulo_plan> titulo_plan { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<alumno>()
                .Property(e => e.matricula)
                .IsUnicode(false);

            modelBuilder.Entity<alumno>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<alumno>()
                .Property(e => e.plantit)
                .IsUnicode(false);

            modelBuilder.Entity<correlativa>()
                .Property(e => e.plantit)
                .IsUnicode(false);

            modelBuilder.Entity<materia>()
                .Property(e => e.abr)
                .IsUnicode(false);

            modelBuilder.Entity<materia>()
                .HasMany(e => e.materia_has_plan)
                .WithRequired(e => e.materia)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<materia_has_plan>()
                .Property(e => e.plantit)
                .IsUnicode(false);

            modelBuilder.Entity<materia_has_plan>()
                .HasOptional(e => e.correlativa)
                .WithRequired(e => e.materia_has_plan);

            modelBuilder.Entity<titulo>()
                .Property(e => e.plantit)
                .IsUnicode(false);

            modelBuilder.Entity<titulo>()
                .HasMany(e => e.alumnos)
                .WithRequired(e => e.titulo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<titulo>()
                .HasMany(e => e.titulo_plan)
                .WithRequired(e => e.titulo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<titulo_plan>()
                .Property(e => e.plantit)
                .IsUnicode(false);

            modelBuilder.Entity<titulo_plan>()
                .HasMany(e => e.materia_has_plan)
                .WithRequired(e => e.titulo_plan)
                .HasForeignKey(e => new { e.planid, e.plantit })
                .WillCascadeOnDelete(false);
        }
    }
}
