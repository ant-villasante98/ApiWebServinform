using System;
using System.Collections.Generic;

namespace Servirform.Models.DataModels;

public partial class Role
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
