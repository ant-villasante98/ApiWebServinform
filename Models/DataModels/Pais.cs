using System;
using System.Collections.Generic;

namespace Servirform.Models.DataModels;

public partial class Pais
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Provincia> Provincia { get; set; } = new List<Provincia>();
}
