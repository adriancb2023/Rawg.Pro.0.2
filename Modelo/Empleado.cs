using System;
using System.Collections.Generic;

namespace Rawg.Pro._0._2.Modelo;

public partial class Empleado
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Email { get; set; }

    public string Rol { get; set; } = null!;

    public string Nombreusuario { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
