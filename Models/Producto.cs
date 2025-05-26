using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Productos.Models;

public partial class Producto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Activo { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

}
public partial class InsertarProducto
{
    
    public string Nombre { get; set; } = null!;

    public string? Activo { get; set; }

    public decimal Precio { get; set; }

    public int Stock { get; set; }

}