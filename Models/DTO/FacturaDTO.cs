namespace Servirform.Models.DTO;

public class FacturaDTO
{
    public int? NroFactura { get; set; }

    public DateTime FechaHora { get; set; }

    public int IdEmpresa { get; set; }

    public float PrecioTotal { get; set; }
    public virtual ICollection<LineasFacturaDTO> LineasFacturas { get; set; }
}