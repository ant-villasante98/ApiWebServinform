namespace Servirform.Models.DTO;

public class LineasFacturaDTO
{
    public int NroFactura { get; set; }

    public int CodArticulo { get; set; }

    public string? ArticuloNombre { get; set; }

    public float PrecioUnidad { get; set; }
}