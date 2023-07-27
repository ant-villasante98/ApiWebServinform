using System.Security.Claims;
using Servirform.Models.DataModels;

namespace Servirform.Services.Contrato;

public interface IEmpresaService
{
    Task<List<Empresa>> EmpresasPorUsuario(string idUsuario, int limit, int page);
}