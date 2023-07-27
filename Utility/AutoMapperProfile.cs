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
        CreateMap<Empresa, EmpresaDTO>()
            .ForMember(destino =>
            destino.NombreBarrio,
            opt => opt.MapFrom(origen => origen.IdBarrioNavigation.Nombre));
        CreateMap<EmpresaDTO, Empresa>()
        .ForMember(destino =>
        destino.IdBarrioNavigation,
        opt => opt.Ignore());
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
            ).ForMember(
                destino => destino.NombreEmpresa,
                opt => opt.MapFrom(origin => origin.IdEmpresaNavigation.Nombre)
            );
        CreateMap<FacturaDTO, Factura>()
            .ForMember(
                destino => destino.FechaHora,
                opt => opt.MapFrom(origen => origen.FechaHora.ToLocalTime())
            ).ForMember(
                destino => destino.IdEmpresaNavigation,
                opt => opt.Ignore()
            );

        #endregion Factura

        #region Pais
        CreateMap<Pais, PaisDTO>().ReverseMap();
        #endregion Pais

        #region Provincia
        CreateMap<Provincia, ProvinciaDTO>().ReverseMap();
        #endregion Provincia

        #region Departamento
        CreateMap<Departamento, DepartamentoDTO>().ReverseMap();
        #endregion Departamento

        #region Localidad
        CreateMap<Localidad, LocalidadDTO>().ReverseMap();
        #endregion Localidad

        #region Barrio
        CreateMap<Barrio, BarrioDTO>().ReverseMap();
        #endregion Barrio
    }
}
