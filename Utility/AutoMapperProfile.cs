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
    }
}
