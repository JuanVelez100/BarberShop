using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BarbershopLogic.DataBase
{
    public partial class BarberShopDataBaseContext : DbContext
    {
        public BarberShopDataBaseContext()
        {
        }

        public BarberShopDataBaseContext(DbContextOptions<BarberShopDataBaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Barber> Barbers { get; set; }
        public virtual DbSet<Barbershop> Barbershops { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Contract> Contracts { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Service> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=VALEMAS-185;Database=BarberShopDataBase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Barber>(entity =>
            {
                entity.ToTable("Barber");

                entity.Property(e => e.Id)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Barber)
                    .HasForeignKey<Barber>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Barber_Person");

                entity.HasOne(d => d.IdBarberShopNavigation)
                    .WithMany(p => p.Barbers)
                    .HasForeignKey(d => d.IdBarberShop)
                    .HasConstraintName("FK_Barber_Barbershop");

                entity.HasOne(d => d.IdServicioNavigation)
                    .WithMany(p => p.Barbers)
                    .HasForeignKey(d => d.IdServicio)
                    .HasConstraintName("FK_Barber_Service");
            });

            modelBuilder.Entity<Barbershop>(entity =>
            {
                entity.ToTable("Barbershop");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdCityNavigation)
                    .WithMany(p => p.Barbershops)
                    .HasForeignKey(d => d.IdCity)
                    .HasConstraintName("FK_Barbershop_Cities");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Client");

                entity.Property(e => e.Id)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Client)
                    .HasForeignKey<Client>(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Client__Id__66603565");

                entity.HasOne(d => d.TypeAffiliationNavigation)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.TypeAffiliation)
                    .HasConstraintName("FK__Client__TypeAffi__656C112C");
            });

            modelBuilder.Entity<Contract>(entity =>
            {
                entity.HasKey(e => e.TypeAffiliation)
                    .HasName("PK__Contract__F89478BE1FD49845");

                entity.ToTable("Contract");

                entity.Property(e => e.TypeAffiliation).ValueGeneratedNever();
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");

                entity.Property(e => e.Id)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mail)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Passwork)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("Reservation");

                entity.HasIndex(e => e.Id, "IX_Reservation")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.IdBarber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("idBarber");

                entity.Property(e => e.IdCity).HasColumnName("idCity");

                entity.Property(e => e.IdClient)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("idClient");

                entity.HasOne(d => d.IdBarberNavigation)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.IdBarber)
                    .HasConstraintName("FK_Reservation_Barber");

                entity.HasOne(d => d.IdBarberShopNavigation)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.IdBarberShop)
                    .HasConstraintName("FK_Reservation_Barbershop");

                entity.HasOne(d => d.IdCityNavigation)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.IdCity)
                    .HasConstraintName("FK_Reservation_City");

                entity.HasOne(d => d.IdClientNavigation)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.IdClient)
                    .HasConstraintName("FK_Reservation_Client");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
