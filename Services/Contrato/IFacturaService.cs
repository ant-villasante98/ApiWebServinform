using Servirform.Models.DataModels;

namespace Servirform.Services.Contrato;

public interface IFacturaService
{
    Task<Factura> RegitrarFactura(Factura factura);

    Task<List<Factura>> FacturasPorEmpresas(int idEmpresa);

    Task<List<Factura>> FacturasPorUsuario(string idUsuario, int limit, int page, string orderBy, string sort);
}