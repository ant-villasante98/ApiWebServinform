namespace Servirform.Models.DTO;

public class UsuarioDTO
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public int IdRol { get; set; }
}