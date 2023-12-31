using AutoMapper;
using Servirform.Models.DataModels;
using Servirform.Models.DTO;

namespace Servirform.Utility;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        #region Ususario
        CreateMap<Usuario, UsuarioDTO>().ReverseMap();
        #endregion Usuario

        #region Empresa
        CreateMap<Empresa, EmpresaDTO>().ReverseMap();
        #endregion Empresa

        #region LineasFactura
        CreateMap<LineasFactura, LineasFacturaDTO>()
            .ForMember(destino =>
            destino.ArticuloNombre,
            opt => opt.MapFrom(origin => origin.CodArticuloNavigation.Nombre));

        CreateMap<LineasFacturaDTO, LineasFactura>()
            .ForMember(
                destino => destino.CodArticuloNavigation,
                opt => opt.Ignore()
            );

        #endregion LineasFactura

        #region Factura
        CreateMap<Factura, FacturaDTO>()
            .ForMember(
                destino => destino.FechaHora,
                opt => opt.MapFrom(origen => origen.FechaHora.ToUniversalTime())
            );
        CreateMap<FacturaDTO, Factura>()
            .ForMember(
                destino => destino.FechaHora,
                opt => opt.MapFrom(origen => origen.FechaHora.ToLocalTime())
            );

        #endregion Factura
    }
}
