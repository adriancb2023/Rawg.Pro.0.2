using System;
using System.Collections.Generic;

namespace Rawg.Pro._0._2.Modelo;

public partial class Venta
{
    public int Id { get; set; }

    public DateTime Fecha { get; set; }

    public double Importe { get; set; }

    public int IdEmpleado { get; set; }

    public virtual ICollection<DetallesVentum> DetallesVenta { get; set; } = new List<DetallesVentum>();

    public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;
}
