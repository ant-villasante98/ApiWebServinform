
using Microsoft.EntityFrameworkCore;
using Servirform.DataAcces;
using Servirform.Models.DataModels;
using Servirform.Services.Contrato;

namespace Servirform.Services;

public class EmpresaService : IEmpresaService
{
    private readonly ServinformContext _context;

    public EmpresaService(ServinformContext context)
    {
        _context = context;
    }

    public async Task<List<Empresa>> EmpresasPorUsuario(string idUsuario, int limit, int page)
    {
        List<Empresa> ListEmpresas = await _context.Empresas.Where(e => e.EmailUsuario == idUsuario).Skip((page - 1) * limit).Take(limit).Include(e => e.IdBarrioNavigation).ToListAsync();

        return ListEmpresas;
    }
}