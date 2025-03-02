using System;
using System.Collections.Generic;

namespace Rawg.Pro._0._2.Modelo;

public partial class Juego
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Precio { get; set; } = null!;

    public string Desarrollador { get; set; } = null!;

    public string Genero { get; set; } = null!;

    public string Puntuacion { get; set; } = null!;

    public string Lanzamiento { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string Capturas { get; set; } = null!;

    public string Portada { get; set; } = null!;

    public virtual ICollection<DetallesVentum> DetallesVenta { get; set; } = new List<DetallesVentum>();

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();
}
