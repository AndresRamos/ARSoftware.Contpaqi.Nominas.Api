using Api.Core.Domain.Common;
using Api.Core.Domain.Models;
using ARSoftware.Contpaqi.Api.Common.Domain;

namespace Api.Core.Domain.Requests;

public sealed class BuscarTiposPeriodoRequest : ContpaqiRequest<BuscarTiposPeriodoRequestModel, BuscarTiposPeriodoRequestOptions,
    BuscarTiposPeriodoResponse>
{
    public BuscarTiposPeriodoRequest(BuscarTiposPeriodoRequestModel model, BuscarTiposPeriodoRequestOptions options) : base(model, options)
    {
    }
}

public sealed class BuscarTiposPeriodoRequestModel
{
    public string? Nombre { get; set; }
    public string? SqlQuery { get; set; }
}

public sealed class BuscarTiposPeriodoRequestOptions : ILoadRelatedDataOptions
{
    public bool CargarDatosExtra { get; set; }
}

public sealed class BuscarTiposPeriodoResponse : ContpaqiResponse<BuscarTiposPeriodoResponseModel>
{
    public BuscarTiposPeriodoResponse(BuscarTiposPeriodoResponseModel model) : base(model)
    {
    }

    public static BuscarTiposPeriodoResponse CreateInstance(List<TipoPeriodo> tiposPeriodo)
    {
        return new BuscarTiposPeriodoResponse(new BuscarTiposPeriodoResponseModel(tiposPeriodo));
    }
}

public sealed class BuscarTiposPeriodoResponseModel
{
    public BuscarTiposPeriodoResponseModel(List<TipoPeriodo> tiposPeriodo)
    {
        TiposPeriodo = tiposPeriodo;
    }

    public int NumeroRegistros => TiposPeriodo.Count;
    public List<TipoPeriodo> TiposPeriodo { get; set; }
}
