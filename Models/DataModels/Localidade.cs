using System;
using System.Collections.Generic;

namespace Servirform.Models.DataModels;

public partial class Localidade
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdDepartamento { get; set; }

    public virtual ICollection<Barrio> Barrios { get; set; } = new List<Barrio>();

    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;
}
