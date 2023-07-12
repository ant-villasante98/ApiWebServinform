namespace Servirform.Models.DTO;

public class BarrioDTO
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int IdLocalidad { get; set; }
}