using System;
using System.Collections.Generic;

namespace Rawg.Pro._0._2.Modelo;

public partial class DetallesVentum
{
    public int Id { get; set; }

    public int IdVenta { get; set; }

    public int IdJuego { get; set; }

    public int Copias { get; set; }

    public double PrecioInventario { get; set; }

    public virtual Juego IdJuegoNavigation { get; set; } = null!;

    public virtual Venta IdVentaNavigation { get; set; } = null!;
}
