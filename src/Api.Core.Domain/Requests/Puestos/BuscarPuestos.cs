using Api.Core.Domain.Common;
using Api.Core.Domain.Models;
using ARSoftware.Contpaqi.Api.Common.Domain;

namespace Api.Core.Domain.Requests;

public sealed class BuscarPuestosRequest : ContpaqiRequest<BuscarPuestosRequestModel, BuscarPuestosRequestOptions, BuscarPuestosResponse>
{
    public BuscarPuestosRequest(BuscarPuestosRequestModel model, BuscarPuestosRequestOptions options) : base(model, options)
    {
    }
}

public sealed class BuscarPuestosRequestModel
{
    public string? SqlQuery { get; set; }
}

public sealed class BuscarPuestosRequestOptions : ILoadRelatedDataOptions
{
    public bool CargarDatosExtra { get; set; }
}

public sealed class BuscarPuestosResponse : ContpaqiResponse<BuscarPuestosResponseModel>
{
    public BuscarPuestosResponse(BuscarPuestosResponseModel model) : base(model)
    {
    }

    public static BuscarPuestosResponse CreateInstance(List<Puesto> puestos)
    {
        return new BuscarPuestosResponse(new BuscarPuestosResponseModel(puestos));
    }
}

public sealed class BuscarPuestosResponseModel
{
    public BuscarPuestosResponseModel(List<Puesto> puestos)
    {
        Puestos = puestos;
    }

    public int NumeroRegistros => Puestos.Count;
    public List<Puesto> Puestos { get; set; }
}
