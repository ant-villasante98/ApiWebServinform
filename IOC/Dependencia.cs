
using Servirform;

namespace Servirform.IOC;

public static class Dependencia
{
    public static void InyectarDependencias(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddAutoMapper(typeof(Utility.AutoMapperProfile));
    }
}