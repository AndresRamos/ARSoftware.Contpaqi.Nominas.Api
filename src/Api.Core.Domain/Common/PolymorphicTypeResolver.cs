using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Api.Core.Domain.Requests;
using ARSoftware.Contpaqi.Api.Common.Domain;
using ARSoftware.Contpaqi.Api.Common.Responses;

namespace Api.Core.Domain.Common;

public sealed class PolymorphicTypeResolver : DefaultJsonTypeInfoResolver
{
    public override JsonTypeInfo GetTypeInfo(Type type, JsonSerializerOptions options)
    {
        JsonTypeInfo jsonTypeInfo = base.GetTypeInfo(type, options);
        Type apiRequestBaseType = typeof(ContpaqiRequest);
        if (jsonTypeInfo.Type == apiRequestBaseType)
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(BuscarConceptosRequest), nameof(BuscarConceptosRequest)),
                    new JsonDerivedType(typeof(BuscarDepartamentosRequest), nameof(BuscarDepartamentosRequest)),
                    new JsonDerivedType(typeof(BuscarEmpleadosRequest), nameof(BuscarEmpleadosRequest)),
                    new JsonDerivedType(typeof(BuscarEmpresasRequest), nameof(BuscarEmpresasRequest)),
                    new JsonDerivedType(typeof(BuscarMovimientosPDORequest), nameof(BuscarMovimientosPDORequest)),
                    new JsonDerivedType(typeof(BuscarPeriodosRequest), nameof(BuscarPeriodosRequest)),
                    new JsonDerivedType(typeof(BuscarTiposPeriodoRequest), nameof(BuscarTiposPeriodoRequest)),
                    new JsonDerivedType(typeof(CrearMovimientoPDORequest), nameof(CrearMovimientoPDORequest)),
                    new JsonDerivedType(typeof(BuscarPuestosRequest), nameof(BuscarPuestosRequest))
                }
            };

        Type apiResponseBaseType = typeof(ContpaqiResponse);
        if (jsonTypeInfo.Type == apiResponseBaseType)
            jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
            {
                TypeDiscriminatorPropertyName = "$type",
                IgnoreUnrecognizedTypeDiscriminators = true,
                UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FailSerialization,
                DerivedTypes =
                {
                    new JsonDerivedType(typeof(BuscarConceptosResponse), nameof(BuscarConceptosResponse)),
                    new JsonDerivedType(typeof(BuscarDepartamentosResponse), nameof(BuscarDepartamentosResponse)),
                    new JsonDerivedType(typeof(BuscarEmpleadosResponse), nameof(BuscarEmpleadosResponse)),
                    new JsonDerivedType(typeof(BuscarEmpresasResponse), nameof(BuscarEmpresasResponse)),
                    new JsonDerivedType(typeof(BuscarMovimientosPDOResponse), nameof(BuscarMovimientosPDOResponse)),
                    new JsonDerivedType(typeof(BuscarPeriodosResponse), nameof(BuscarPeriodosResponse)),
                    new JsonDerivedType(typeof(BuscarTiposPeriodoResponse), nameof(BuscarTiposPeriodoResponse)),
                    new JsonDerivedType(typeof(CrearMovimientoPDOResponse), nameof(CrearMovimientoPDOResponse)),
                    new JsonDerivedType(typeof(BuscarPuestosResponse), nameof(BuscarPuestosResponse)),
                    new JsonDerivedType(typeof(EmptyContpaqiResponse), nameof(EmptyContpaqiResponse)),
                    new JsonDerivedType(typeof(ErrorContpaqiResponse), nameof(ErrorContpaqiResponse))
                }
            };

        return jsonTypeInfo;
    }
}
