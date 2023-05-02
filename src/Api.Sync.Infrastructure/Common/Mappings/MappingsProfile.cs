using Api.Core.Domain.Models;
using Api.Sync.Infrastructure.ContpaqiNominas.Models;
using ARSoftware.Contpaqi.Nominas.Sql.Models.Empresa;
using ARSoftware.Contpaqi.Nominas.Sql.Models.Generales;
using AutoMapper;

namespace Api.Sync.Infrastructure.Common.Mappings;

public sealed class MappingsProfile : Profile
{
    public MappingsProfile()
    {
        CreateMap<NOM10000, EmpresaSql>();
        CreateMap<EmpresaSql, Empresa>()
            .ForMember(des => des.Nombre, opt => opt.MapFrom(src => src.NombreEmpresa))
            .ForMember(des => des.Ruta, opt => opt.MapFrom(src => src.RutaEmpresa))
            .ForMember(des => des.BaseDatos, opt => opt.MapFrom(src => src.RutaEmpresa))
            .ForMember(des => des.GuidAdd, opt => opt.MapFrom(src => src.GUIDDSL))
            .ForMember(des => des.Rfc,
                opt => opt.MapFrom(src => $"{src.RFC!.Trim()}{src.FechaConstitucion!.Value:yyMMdd}{src.Homoclave!.Trim()}"));

        CreateMap<nom10003, DepartamentoSql>();
        CreateMap<DepartamentoSql, Departamento>()
            .ForMember(des => des.Numero, opt => opt.MapFrom(src => src.numerodepartamento))
            .ForMember(des => des.Nombre, opt => opt.MapFrom(src => src.descripcion));

        CreateMap<nom10006, PuestoSql>();
        CreateMap<PuestoSql, Puesto>()
            .ForMember(des => des.Numero, opt => opt.MapFrom(src => src.numeropuesto))
            .ForMember(des => des.Nombre, opt => opt.MapFrom(src => src.descripcion))
            .ForMember(des => des.Descripcion, opt => opt.MapFrom(src => src.Detalle));

        CreateMap<nom10001, EmpleadoSql>();
        CreateMap<EmpleadoSql, Empleado>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => src.idempleado))
            .ForMember(des => des.Codigo, opt => opt.MapFrom(src => src.codigoempleado))
            .ForMember(des => des.Nombres, opt => opt.MapFrom(src => src.nombre))
            .ForMember(des => des.ApellidoPaterno, opt => opt.MapFrom(src => src.apellidopaterno))
            .ForMember(des => des.ApellidoMaterno, opt => opt.MapFrom(src => src.apellidomaterno))
            .ForMember(des => des.NombreLargo, opt => opt.MapFrom(src => src.nombrelargo))
            .ForMember(des => des.Rfc,
                opt => opt.MapFrom(src => $"{src.rfc!.Trim()}{src.fechanacimiento!.Value:yyMMdd}{src.homoclave!.Trim()}"))
            .ForMember(des => des.Curp,
                opt => opt.MapFrom(src => $"{src.curpi!.Trim()}{src.fechanacimiento!.Value:yyMMdd}{src.curpf!.Trim()}"));

        CreateMap<nom10004, ConceptoSql>();
        CreateMap<ConceptoSql, Concepto>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => src.idconcepto))
            .ForMember(des => des.Numero, opt => opt.MapFrom(src => src.numeroconcepto))
            .ForMember(des => des.Tipo, opt => opt.MapFrom(src => src.tipoconcepto))
            .ForMember(des => des.Descripcion, opt => opt.MapFrom(src => src.descripcion))
            .ForMember(des => des.ClaveAgrupadoraSat, opt => opt.MapFrom(src => src.ClaveAgrupadoraSAT))
            .ForMember(des => des.TipoClaveSat, opt => opt.MapFrom(src => src.TipoClaveSAT));

        CreateMap<nom10002, PeriodoSql>();
        CreateMap<PeriodoSql, Periodo>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => src.idperiodo))
            .ForMember(des => des.Numero, opt => opt.MapFrom(src => src.numeroperiodo))
            .ForMember(des => des.FechaInicio, opt => opt.MapFrom(src => src.fechainicio))
            .ForMember(des => des.FechaFin, opt => opt.MapFrom(src => src.fechafin))
            .ForMember(des => des.Ejercicio, opt => opt.MapFrom(src => src.ejercicio))
            .ForMember(des => des.Mes, opt => opt.MapFrom(src => src.mes));

        CreateMap<nom10008, MovimientoPDOSql>();
        CreateMap<MovimientoPDOSql, MovimientoPDO>().ForMember(des => des.ImporteTotal, opt => opt.MapFrom(src => src.importetotal));

        CreateMap<nom10023, TipoPeriodoSql>();
        CreateMap<TipoPeriodoSql, TipoPeriodo>()
            .ForMember(des => des.Id, opt => opt.MapFrom(src => src.idtipoperiodo))
            .ForMember(des => des.Nombre, opt => opt.MapFrom(src => src.nombretipoperiodo))
            .ForMember(des => des.DiasDelPeriodo, opt => opt.MapFrom(src => src.diasdelperiodo))
            .ForMember(des => des.PeriodoTrabajo, opt => opt.MapFrom(src => src.periodotrabajo));
    }
}
