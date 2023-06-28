using System;
using System.Collections.Generic;

namespace Servirform.Models.DataModels;

public partial class Provincia
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdPais { get; set; }

    public virtual ICollection<Departamento> Departamentos { get; set; } = new List<Departamento>();

    public virtual Paise IdPaisNavigation { get; set; } = null!;
}
