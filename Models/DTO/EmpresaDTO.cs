namespace Servirform.Models.DTO;

public class EmpresaDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;

    public string Calle { get; set; } = null!;

    public int NroCalle { get; set; }

    public int IdBarrio { get; set; }
    public string? NombreBarrio { get; set; }

    public string EmailUsuario { get; set; } = null!;
}