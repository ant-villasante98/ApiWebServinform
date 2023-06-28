using System;
using System.Collections.Generic;

namespace Servirform.Models.DataModels;

public partial class Empresa
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Calle { get; set; } = null!;

    public int NroCalle { get; set; }

    public int IdBarrio { get; set; }

    public string EmailUsuario { get; set; } = null!;

    public virtual ICollection<Articulo> Articulos { get; set; } = new List<Articulo>();

    public virtual Usuario EmailUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual Barrio IdBarrioNavigation { get; set; } = null!;
}
