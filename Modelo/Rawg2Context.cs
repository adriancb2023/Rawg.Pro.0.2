using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Rawg.Pro._0._2.Modelo;

public partial class Rawg2Context : DbContext
{
    public Rawg2Context()
    {
    }

    public Rawg2Context(DbContextOptions<Rawg2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<DetallesVentum> DetallesVenta { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Juego> Juegos { get; set; }

    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=rawg2;user=root;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("11.5.2-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<DetallesVentum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("detalles_venta");

            entity.HasIndex(e => e.IdJuego, "FK_detalles_venta_juegos");

            entity.HasIndex(e => e.IdVenta, "FK_detalles_venta_ventas");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Copias)
                .HasColumnType("int(11)")
                .HasColumnName("copias");
            entity.Property(e => e.IdJuego)
                .HasColumnType("int(11)")
                .HasColumnName("id_juego");
            entity.Property(e => e.IdVenta)
                .HasColumnType("int(11)")
                .HasColumnName("id_venta");
            entity.Property(e => e.PrecioInventario).HasColumnName("precio_inventario");

            entity.HasOne(d => d.IdJuegoNavigation).WithMany(p => p.DetallesVenta)
                .HasForeignKey(d => d.IdJuego)
                .HasConstraintName("FK_detalles_venta_juegos");

            entity.HasOne(d => d.IdVentaNavigation).WithMany(p => p.DetallesVenta)
                .HasForeignKey(d => d.IdVenta)
                .HasConstraintName("FK_detalles_venta_ventas");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("empleados");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Apellidos)
                .HasMaxLength(50)
                .HasColumnName("apellidos");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(50)
                .HasColumnName("contraseña");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Nombreusuario)
                .HasMaxLength(50)
                .HasColumnName("nombreusuario");
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .HasDefaultValueSql("'vendedor'")
                .HasColumnName("rol");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("inventario");

            entity.HasIndex(e => e.IdJuego, "FK__juegos");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IdJuego)
                .HasColumnType("int(11)")
                .HasColumnName("id_juego");
            entity.Property(e => e.PrecioVenta)
                .HasDefaultValueSql("'79.99'")
                .HasColumnName("precioVenta");
            entity.Property(e => e.Stock)
                .HasColumnType("int(11)")
                .HasColumnName("stock");

            entity.HasOne(d => d.IdJuegoNavigation).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.IdJuego)
                .HasConstraintName("FK__juegos");
        });

        modelBuilder.Entity<Juego>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("juegos");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Capturas)
                .HasMaxLength(10000)
                .HasColumnName("capturas");
            entity.Property(e => e.Desarrollador)
                .HasMaxLength(250)
                .HasColumnName("desarrollador");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(250)
                .HasColumnName("descripcion");
            entity.Property(e => e.Genero)
                .HasMaxLength(250)
                .HasColumnName("genero");
            entity.Property(e => e.Lanzamiento)
                .HasMaxLength(50)
                .HasColumnName("lanzamiento");
            entity.Property(e => e.Nombre)
                .HasMaxLength(250)
                .HasDefaultValueSql("'0'")
                .HasColumnName("nombre");
            entity.Property(e => e.Portada)
                .HasMaxLength(1000)
                .HasColumnName("portada");
            entity.Property(e => e.Precio)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Sin estipular'")
                .HasColumnName("precio");
            entity.Property(e => e.Puntuacion)
                .HasMaxLength(50)
                .HasColumnName("puntuacion");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("ventas");

            entity.HasIndex(e => e.IdEmpleado, "FK_ventas_empleados");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("fecha");
            entity.Property(e => e.IdEmpleado)
                .HasColumnType("int(11)")
                .HasColumnName("id_empleado");
            entity.Property(e => e.Importe).HasColumnName("importe");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.Venta)
                .HasForeignKey(d => d.IdEmpleado)
                .HasConstraintName("FK_ventas_empleados");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
