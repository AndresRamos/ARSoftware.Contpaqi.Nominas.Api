using Api.Core.Domain.Common;
using Api.Core.Domain.Models;
using ARSoftware.Contpaqi.Api.Common.Domain;

// ReSharper disable InconsistentNaming

namespace Api.Core.Domain.Requests;

public sealed class CrearMovimientoPDORequest : ContpaqiRequest<CrearMovimientoPDORequestModel, CrearMovimientoPDORequestOptions,
    CrearMovimientoPDOResponse>
{
    public CrearMovimientoPDORequest(CrearMovimientoPDORequestModel model, CrearMovimientoPDORequestOptions options) : base(model, options)
    {
    }
}

public sealed class CrearMovimientoPDORequestModel
{
    public LlavePeriodo Periodo { get; set; } = new();
    public string EmpleadoCodigo { get; set; } = string.Empty;
    public int ConceptoNumero { get; set; }
    public decimal Importe { get; set; }
}

public sealed class CrearMovimientoPDORequestOptions : ILoadRelatedDataOptions
{
    public bool Sobreescribir { get; set; }
    public bool CargarDatosExtra { get; set; }
}

public sealed class CrearMovimientoPDOResponse : ContpaqiResponse<CrearMovimientoPDOResponseModel>
{
    public CrearMovimientoPDOResponse(CrearMovimientoPDOResponseModel model) : base(model)
    {
    }

    public static CrearMovimientoPDOResponse CreateInstance(MovimientoPDO movimientoPdo)
    {
        return new CrearMovimientoPDOResponse(new CrearMovimientoPDOResponseModel(movimientoPdo));
    }
}

public sealed class CrearMovimientoPDOResponseModel
{
    public CrearMovimientoPDOResponseModel(MovimientoPDO movimientoPdo)
    {
        MovimientoPDO = movimientoPdo;
    }

    public MovimientoPDO MovimientoPDO { get; set; }
}
