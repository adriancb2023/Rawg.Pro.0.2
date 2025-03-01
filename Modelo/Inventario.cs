using System;
using System.Collections.Generic;

namespace Rawg.Pro._0._2.Modelo;

public partial class Inventario
{
    public int Id { get; set; }

    public int IdJuego { get; set; }

    public int Stock { get; set; }

    public double PrecioVenta { get; set; }

    public virtual Juego IdJuegoNavigation { get; set; } = null!;
}
