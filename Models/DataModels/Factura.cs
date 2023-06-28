using System;
using System.Collections.Generic;

namespace Servirform.Models.DataModels;

public partial class Factura
{
    public int NroFactura { get; set; }

    public DateTime FechaHora { get; set; }

    public int IdEmpresa { get; set; }

    public float PrecioTotal { get; set; }

    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}
