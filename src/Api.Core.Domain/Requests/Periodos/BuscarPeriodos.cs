using Api.Core.Domain.Common;
using Api.Core.Domain.Models;
using ARSoftware.Contpaqi.Api.Common.Domain;

namespace Api.Core.Domain.Requests;

public sealed class
    BuscarPeriodosRequest : ContpaqiRequest<BuscarPeriodosRequestModel, BuscarPeriodosRequestOptions, BuscarPeriodosResponse>
{
    public BuscarPeriodosRequest(BuscarPeriodosRequestModel model, BuscarPeriodosRequestOptions options) : base(model, options)
    {
    }
}

public sealed class BuscarPeriodosRequestModel
{
    public string? TipoPeriodo { get; set; }
    public int? Numero { get; set; }
    public int? Ejercicio { get; set; }
    public int? Mes { get; set; }
    public string? SqlQuery { get; set; }
}

public sealed class BuscarPeriodosRequestOptions : ILoadRelatedDataOptions
{
    public bool CargarDatosExtra { get; set; }
}

public sealed class BuscarPeriodosResponse : ContpaqiResponse<BuscarPeriodosResponseModel>
{
    public BuscarPeriodosResponse(BuscarPeriodosResponseModel model) : base(model)
    {
    }

    public static BuscarPeriodosResponse CreateInstance(List<Periodo> periodos)
    {
        return new BuscarPeriodosResponse(new BuscarPeriodosResponseModel(periodos));
    }
}

public sealed class BuscarPeriodosResponseModel
{
    public BuscarPeriodosResponseModel(List<Periodo> periodos)
    {
        Periodos = periodos;
    }

    public int NumeroRegistros => Periodos.Count;
    public List<Periodo> Periodos { get; set; }
}
