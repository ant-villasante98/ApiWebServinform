using System;
using System.Collections.Generic;

namespace Servirform.Models.DataModels;

public partial class Articulo
{
    public int Codigo { get; set; }

    public string Nombre { get; set; } = null!;

    public float PrecioUnidad { get; set; }

    public int IdEmpresa { get; set; }

    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
