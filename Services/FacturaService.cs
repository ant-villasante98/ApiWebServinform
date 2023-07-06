using Microsoft.EntityFrameworkCore;
using Servirform.DataAcces;
using Servirform.Models.DataModels;
using Servirform.Services.Contrato;

namespace Servirform.Services;

public class FacturaService : IFacturaService
{
    private readonly ServinformContext _context;

    public FacturaService(ServinformContext context)
    {
        _context = context;
    }

    public async Task<List<Factura>> FacturasPorEmpresas(int idEmpresa)
    {
        if (_context.Facturas == null)
        {
            throw new TaskCanceledException();
        }

        var result = await _context.Facturas.Where(f => f.IdEmpresa == idEmpresa).ToListAsync();

        return result;
    }

    public async Task<List<Factura>> FacturasPorUsuario(string idUsuario)
    {
        if (_context.Facturas == null)
        {
            throw new TaskCanceledException();
        }

        List<Factura> ListFacturas = await _context.Facturas.Where(f => f.IdEmpresaNavigation.EmailUsuario == idUsuario).ToListAsync();
        return ListFacturas;
    }

    public async Task<Factura> RegitrarFactura(Factura factura)
    {
        if (_context.Facturas == null)
        {
            throw new TaskCanceledException("La entidad Facturas es null");
        }

        using (var transaccion = _context.Database.BeginTransaction())
        {


            try
            {
                await _context.AddAsync(factura);
                await _context.SaveChangesAsync();

                // foreach (LineasFactura lf in factura.LineasFacturas)
                // {
                //     Console.WriteLine($"#####El nroFactura en la LineaFactura: {lf.NroFactura} y codArticulo:{lf.CodArticulo}");
                //     await _context.AddAsync(lf);
                //     await _context.SaveChangesAsync();
                // }

                transaccion.Commit();
            }
            catch (Exception error)
            {
                transaccion.Rollback();
                throw new TaskCanceledException($"No se pudo completar el Registro de la Factura: {error}");
            }
        }

        return factura;
    }

}