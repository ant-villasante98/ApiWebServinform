using System;
using System.Collections.Generic;

namespace Servirform.Models.DataModels;

public partial class LineasFactura
{
    public int NroFactura { get; set; }

    public int CodArticulo { get; set; }

    public float PrecioUnidad { get; set; }
}
