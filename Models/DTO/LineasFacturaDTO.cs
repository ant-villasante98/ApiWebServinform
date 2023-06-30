namespace Servirform.Models.DTO;

public class LineasFacturaDTO
{
    public int NroFactura { get; set; }

    public int CodArticulo { get; set; }

    public string? ArticuloNombre { get; set; } = string.Empty;

    public float PrecioUnidad { get; set; }
}