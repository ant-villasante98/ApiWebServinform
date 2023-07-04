
using Servirform;
using Servirform.Services;
using Servirform.Services.Contrato;

namespace Servirform.IOC;

public static class Dependencia
{
    public static void InyectarDependencias(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddAutoMapper(typeof(Utility.AutoMapperProfile));

        service.AddScoped<IFacturaService, FacturaService>();

        service.AddScoped<IUsuarioService, UsuarioService>();
    }
}