using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Servirform.Models.DataModels;

namespace Servirform.DataAcces;

public partial class ServinformContext : DbContext
{
    public ServinformContext()
    {
    }

    public ServinformContext(DbContextOptions<ServinformContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Articulo> Articulos { get; set; }

    public virtual DbSet<Barrio> Barrios { get; set; }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<Empresa> Empresas { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<LineasFactura> LineasFacturas { get; set; }

    public virtual DbSet<Localidade> Localidades { get; set; }

    public virtual DbSet<Paise> Paises { get; set; }

    public virtual DbSet<Provincia> Provincias { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseNpgsql("Host=localhost;Username=postgres;Password=noChavon;Database=Servinform");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Articulo>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("articulos_pkey");

            entity.ToTable("articulos");

            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            entity.Property(e => e.Nombre)
                .HasColumnType("character varying")
                .HasColumnName("nombre");
            entity.Property(e => e.PrecioUnidad).HasColumnName("precio_unidad");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Articulos)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_articulos_empresas");
        });

        modelBuilder.Entity<Barrio>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("barrios_pkey");

            entity.ToTable("barrios");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdLocalidad).HasColumnName("id_localidad");
            entity.Property(e => e.Nombre)
                .HasColumnType("character varying")
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdLocalidadNavigation).WithMany(p => p.Barrios)
                .HasForeignKey(d => d.IdLocalidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_barrios_localidades");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("departamentos_pkey");

            entity.ToTable("departamentos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdProvincia).HasColumnName("id_provincia");
            entity.Property(e => e.Nombre)
                .HasColumnType("character varying")
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdProvinciaNavigation).WithMany(p => p.Departamentos)
                .HasForeignKey(d => d.IdProvincia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_departamentos_provincia");
        });

        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("empresas_pkey");

            entity.ToTable("empresas");

            entity.HasIndex(e => e.Nombre, "empresas_nombre_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Calle)
                .HasColumnType("character varying")
                .HasColumnName("calle");
            entity.Property(e => e.EmailUsuario)
                .HasColumnType("character varying")
                .HasColumnName("email_usuario");
            entity.Property(e => e.IdBarrio).HasColumnName("id_barrio");
            entity.Property(e => e.Nombre)
                .HasColumnType("character varying")
                .HasColumnName("nombre");
            entity.Property(e => e.NroCalle).HasColumnName("nro_calle");

            entity.HasOne(d => d.EmailUsuarioNavigation).WithMany(p => p.Empresas)
                .HasForeignKey(d => d.EmailUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_empresas_usuarios");

            entity.HasOne(d => d.IdBarrioNavigation).WithMany(p => p.Empresas)
                .HasForeignKey(d => d.IdBarrio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_empresas_departamentos");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.NroFactura).HasName("facturas_pkey");

            entity.ToTable("facturas");

            entity.Property(e => e.NroFactura).HasColumnName("nro_factura");
            entity.Property(e => e.FechaHora)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("fecha_hora");
            entity.Property(e => e.IdEmpresa).HasColumnName("id_empresa");
            entity.Property(e => e.PrecioTotal).HasColumnName("precio_total");

            entity.HasOne(d => d.IdEmpresaNavigation).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.IdEmpresa)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_facturas_empresas");
        });

        modelBuilder.Entity<LineasFactura>(entity =>
        {
            entity.HasKey(e => new { e.NroFactura, e.CodArticulo }).HasName("lineas_facturas_pkey");

            entity.ToTable("lineas_facturas");

            entity.Property(e => e.NroFactura).HasColumnName("nro_factura");
            entity.Property(e => e.CodArticulo).HasColumnName("cod_articulo");
            entity.Property(e => e.PrecioUnidad).HasColumnName("precio_unidad");

            entity.HasOne(d => d.CodArticuloNavigation).WithMany(p => p.LineasFacturas)
                .HasForeignKey(d => d.CodArticulo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_lineas_facturas_articulos");

            entity.HasOne(d => d.NroFacturaNavigation).WithMany(p => p.LineasFacturas)
                .HasForeignKey(d => d.NroFactura)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_lineas_facturas_facturas");
        });

        modelBuilder.Entity<Localidade>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("localidades_pkey");

            entity.ToTable("localidades");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdDepartamento).HasColumnName("id_departamento");
            entity.Property(e => e.Nombre)
                .HasColumnType("character varying")
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Localidades)
                .HasForeignKey(d => d.IdDepartamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_localidades_departamentos");
        });

        modelBuilder.Entity<Paise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("paises_pkey");

            entity.ToTable("paises");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasColumnType("character varying")
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Provincia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("provincias_pkey");

            entity.ToTable("provincias");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdPais).HasColumnName("id_pais");
            entity.Property(e => e.Nombre)
                .HasColumnType("character varying")
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Provincia)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_provincias_paises");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Email).HasName("usuarios_pkey");

            entity.ToTable("usuarios");

            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Apellido)
                .HasColumnType("character varying")
                .HasColumnName("apellido");
            entity.Property(e => e.IdRol).HasColumnName("id_rol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(150)
                .HasColumnName("nombre");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usuarios_roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
