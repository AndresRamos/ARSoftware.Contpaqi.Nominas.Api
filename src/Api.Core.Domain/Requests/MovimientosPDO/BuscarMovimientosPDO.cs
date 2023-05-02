using Api.Core.Domain.Common;
using Api.Core.Domain.Models;
using ARSoftware.Contpaqi.Api.Common.Domain;

// ReSharper disable InconsistentNaming

namespace Api.Core.Domain.Requests;

public sealed class BuscarMovimientosPDORequest : ContpaqiRequest<BuscarMovimientosPDORequestModel, BuscarMovimientosPDORequestOptions,
    BuscarMovimientosPDOResponse>
{
    public BuscarMovimientosPDORequest(BuscarMovimientosPDORequestModel model, BuscarMovimientosPDORequestOptions options) : base(model,
        options)
    {
    }
}

public sealed class BuscarMovimientosPDORequestModel
{
    public LlavePeriodo? Periodo { get; set; }
    public string? EmpleadoCodigo { get; set; }
    public int? ConceptoNumero { get; set; }
    public string? SqlQuery { get; set; }
}

public sealed class BuscarMovimientosPDORequestOptions : ILoadRelatedDataOptions
{
    public bool CargarDatosExtra { get; set; }
}

public sealed class BuscarMovimientosPDOResponse : ContpaqiResponse<BuscarMovimientosPDOResponseModel>
{
    public BuscarMovimientosPDOResponse(BuscarMovimientosPDOResponseModel model) : base(model)
    {
    }

    public static BuscarMovimientosPDOResponse CreateInstance(List<MovimientoPDO> movimientos)
    {
        return new BuscarMovimientosPDOResponse(new BuscarMovimientosPDOResponseModel(movimientos));
    }
}

public sealed class BuscarMovimientosPDOResponseModel
{
    public BuscarMovimientosPDOResponseModel(List<MovimientoPDO> movimientos)
    {
        Movimientos = movimientos;
    }

    public int NumeroRegistros => Movimientos.Count;
    public List<MovimientoPDO> Movimientos { get; set; }
}
