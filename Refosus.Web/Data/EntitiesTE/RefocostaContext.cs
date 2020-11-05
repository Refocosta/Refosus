using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace Refosus.Web.Data.EntitiesTE
{
    public partial class RefocostaContext : DbContext
    {
        public RefocostaContext()
        {
        }

        public RefocostaContext(DbContextOptions<RefocostaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EstadoHitos> EstadoHitos { get; set; }
        public virtual DbSet<Hitos> Hitos { get; set; }
        public virtual DbSet<IniciativasEntity> Iniciativas { get; set; }
        public virtual DbSet<Responsables> Responsables { get; set; }
        public virtual DbSet<TipoIniciativas> TipoIniciativas { get; set; }
        public virtual DbSet<WorkStreamEntity> WorkStream { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EstadoHitos>(entity =>
            {
                entity.HasKey(e => e.IdEstadoHito);

                entity.ToTable("EstadoHitos", "wave");

                entity.Property(e => e.IdEstadoHito).HasColumnName("idEstadoHito");

                entity.Property(e => e.Estado)
                    .HasMaxLength(15)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Hitos>(entity =>
            {
                entity.HasKey(e => e.IdHito);

                entity.ToTable("Hitos", "wave");

                entity.Property(e => e.IdHito).HasColumnName("idHito");

                entity.Property(e => e.DescripcionHito)
                    .IsRequired()
                    .HasMaxLength(900);

                entity.Property(e => e.FechaFin).HasColumnType("date");

                entity.Property(e => e.FechaInicio).HasColumnType("date");

                entity.Property(e => e.IdEstado).HasColumnName("idEstado");

                entity.Property(e => e.IdIniciativa).HasColumnName("idIniciativa");

                entity.Property(e => e.IdResponsable).HasColumnName("idResponsable");

                entity.Property(e => e.NombreHito)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Tipo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.Hitos)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Hitos_EstadoHitos");

                entity.HasOne(d => d.IdIniciativaNavigation)
                    .WithMany(p => p.Hitos)
                    .HasForeignKey(d => d.IdIniciativa)
                    .HasConstraintName("FK_Hitos_Iniciativas");
            });

            modelBuilder.Entity<IniciativasEntity>(entity =>
            {
                entity.HasKey(e => e.IdIniciativa)
                    .HasName("PK__Iniciati__7119858A49FF37CD");

                entity.ToTable("Iniciativas", "wave");

                entity.Property(e => e.IdIniciativa).HasColumnName("idIniciativa");

                entity.Property(e => e.BeneficiosNoRecurrentes).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.BeneficiosRecurrente).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.CostoImplementación).HasColumnType("numeric(18, 0)");

                entity.Property(e => e.DescripIniciativa).HasColumnType("text");

                entity.Property(e => e.Etapa)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.FechaL2).HasColumnType("date");

                entity.Property(e => e.FechaL3).HasColumnType("date");

                entity.Property(e => e.FechaL4).HasColumnType("date");

                entity.Property(e => e.FechaL5).HasColumnType("date");

                entity.Property(e => e.IdResponsable).HasColumnName("idResponsable");

                entity.Property(e => e.IdWorkStream).HasColumnName("idWorkStream");

                entity.Property(e => e.Medicion).HasColumnType("text");

                entity.Property(e => e.NombreIniciativa)
                    .HasMaxLength(200)
                    .IsFixedLength();

                entity.Property(e => e.Observacion).HasColumnType("text");

                entity.Property(e => e.RangoBenefRecurrentes)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.SupuestosImpacto).HasColumnType("text");

                entity.Property(e => e.TipoIniciativa)
                    .HasMaxLength(15)
                    .IsFixedLength();

                entity.Property(e => e.Unidad)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.HasOne(d => d.IdResponsableNavigation)
                    .WithMany(p => p.Iniciativas)
                    .HasForeignKey(d => d.IdResponsable)
                    .HasConstraintName("FK_Iniciativas_Responsables");

                entity.HasOne(d => d.IdWorkStreamNavigation)
                    .WithMany(p => p.Iniciativas)
                    .HasForeignKey(d => d.IdWorkStream)
                    .HasConstraintName("FK_Iniciativas_WorkStream");
            });

            modelBuilder.Entity<Responsables>(entity =>
            {
                entity.HasKey(e => e.IdResponsable)
                    .HasName("PK__Responsa__6D0A525186AB0AEF");

                entity.ToTable("Responsables", "wave");

                entity.Property(e => e.IdResponsable).HasColumnName("idResponsable");

                entity.Property(e => e.IdRol)
                    .HasColumnName("idRol")
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .IsFixedLength();
            });

            modelBuilder.Entity<TipoIniciativas>(entity =>
            {
                entity.HasKey(e => e.IdTipoIniciativa);

                entity.ToTable("TipoIniciativas", "wave");

                entity.Property(e => e.IdTipoIniciativa).HasColumnName("idTipoIniciativa");

                entity.Property(e => e.TipoIniciativa)
                    .HasMaxLength(15)
                    .IsFixedLength();
            });

            modelBuilder.Entity<WorkStreamEntity>(entity =>
            {
                entity.HasKey(e => e.IdWorkStream);

                entity.ToTable("WorkStream", "wave");

                entity.Property(e => e.IdWorkStream).HasColumnName("idWorkStream");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.IdResponsable).HasColumnName("idResponsable");

                entity.Property(e => e.WorkStream1)
                    .HasColumnName("WorkStream")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.IdResponsableNavigation)
                    .WithMany(p => p.WorkStream)
                    .HasForeignKey(d => d.IdResponsable)
                    .HasConstraintName("FK_WorkStream_Responsables");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
