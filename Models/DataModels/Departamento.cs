using System;
using System.Collections.Generic;

namespace Servirform.Models.DataModels;

public partial class Departamento
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdProvincia { get; set; }

    public virtual Provincia IdProvinciaNavigation { get; set; } = null!;

    public virtual ICollection<Localidad> Localidades { get; set; } = new List<Localidad>();
}
