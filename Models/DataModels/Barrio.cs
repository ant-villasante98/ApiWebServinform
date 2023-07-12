using System;
using System.Collections.Generic;

namespace Servirform.Models.DataModels;

public partial class Barrio
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdLocalidad { get; set; }

    public virtual ICollection<Empresa> Empresas { get; set; } = new List<Empresa>();

    public virtual Localidad IdLocalidadNavigation { get; set; } = null!;
}
