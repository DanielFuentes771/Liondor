using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Productos.Models;

public partial class GestionProductosContext : DbContext
{
    public GestionProductosContext()
    {
    }

    public GestionProductosContext(DbContextOptions<GestionProductosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Producto> Productos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producto__3214EC077D9DA787");

            entity.Property(e => e.Activo).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
        });

        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
