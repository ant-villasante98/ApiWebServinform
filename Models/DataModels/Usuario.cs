using System;
using System.Collections.Generic;

namespace Servirform.Models.DataModels;

public partial class Usuario
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public int IdRol { get; set; }

    public virtual ICollection<Empresa> Empresas { get; set; } = new List<Empresa>();

    public virtual Role IdRolNavigation { get; set; } = null!;
}
