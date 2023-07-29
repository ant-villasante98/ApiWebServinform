using Microsoft.EntityFrameworkCore;
using System.Linq;
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

        var result = await _context.Facturas.Where(f => f.IdEmpresa == idEmpresa).Include(f => f.IdEmpresaNavigation).ToListAsync();

        return result;
    }

    public async Task<List<Factura>> FacturasPorUsuario(string idUsuario, int limit, int page, string orderBy, string sort)
    {
        if (_context.Facturas == null)
        {
            throw new TaskCanceledException();
        }

        IQueryable<Factura> queryFactura = _context.Facturas.Where(f => f.IdEmpresaNavigation.EmailUsuario == idUsuario).Include(f => f.IdEmpresaNavigation);
        if (sort == "desc")
        {
            queryFactura = queryFactura.OrderByDescending(f => f.NroFactura);
        }
        if (sort == "asc")
        {
            queryFactura = queryFactura.OrderBy(f => f.NroFactura);
        }
        List<Factura> ListFacturas = await queryFactura.Skip((page - 1) * limit).Take(limit).ToListAsync();
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