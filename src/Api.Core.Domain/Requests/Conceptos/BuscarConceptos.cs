using Api.Core.Domain.Common;
using Api.Core.Domain.Models;
using ARSoftware.Contpaqi.Api.Common.Domain;

namespace Api.Core.Domain.Requests;

public sealed class
    BuscarConceptosRequest : ContpaqiRequest<BuscarConceptosRequestModel, BuscarConceptosRequestOptions, BuscarConceptosResponse>
{
    public BuscarConceptosRequest(BuscarConceptosRequestModel model, BuscarConceptosRequestOptions options) : base(model, options)
    {
    }
}

public sealed class BuscarConceptosRequestModel
{
    public int? Id { get; set; }
    public int? Numero { get; set; }
    public string? SqlQuery { get; set; }
}

public sealed class BuscarConceptosRequestOptions : ILoadRelatedDataOptions
{
    public bool CargarDatosExtra { get; set; }
}

public sealed class BuscarConceptosResponse : ContpaqiResponse<BuscarConceptosResponseModel>
{
    public BuscarConceptosResponse(BuscarConceptosResponseModel model) : base(model)
    {
    }

    public static BuscarConceptosResponse CreateInstance(List<Concepto> conceptos)
    {
        return new BuscarConceptosResponse(new BuscarConceptosResponseModel(conceptos));
    }
}

public sealed class BuscarConceptosResponseModel
{
    public BuscarConceptosResponseModel(List<Concepto> conceptos)
    {
        Conceptos = conceptos;
    }

    public int NumeroRegistros => Conceptos.Count;
    public List<Concepto> Conceptos { get; set; }
}
