using Servirform.Models.DataModels;

namespace Servirform.Services.Contrato;

public interface IFacturaService
{
    Task<Factura> RegitrarFactura(Factura factura);
}