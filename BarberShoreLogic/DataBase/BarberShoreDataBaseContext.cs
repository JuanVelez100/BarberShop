using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BarberShoreLogic.DataBase
{
    public partial class BarberShoreDataBaseContext : DbContext
    {
        public BarberShoreDataBaseContext()
        {
        }

        public BarberShoreDataBaseContext(DbContextOptions<BarberShoreDataBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Persona> Personas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=VALEMAS-185; Database=BarberShoreDataBase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.Cedula);

                entity.ToTable("Persona");

                entity.Property(e => e.Cedula).HasMaxLength(50);

                entity.Property(e => e.Celular)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.Contraseña).HasMaxLength(50);

                entity.Property(e => e.Correo).HasMaxLength(50);

                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
