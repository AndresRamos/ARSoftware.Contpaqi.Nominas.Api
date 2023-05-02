using Api.Core.Domain.Common;
using Api.Core.Domain.Models;
using ARSoftware.Contpaqi.Api.Common.Domain;

namespace Api.Core.Domain.Requests;

public sealed class BuscarDepartamentosRequest : ContpaqiRequest<BuscarDepartamentosRequestModel, BuscarDepartamentosRequestOptions,
    BuscarDepartamentosResponse>
{
    public BuscarDepartamentosRequest(BuscarDepartamentosRequestModel model, BuscarDepartamentosRequestOptions options) : base(model,
        options)
    {
    }
}

public sealed class BuscarDepartamentosRequestModel
{
    public string? SqlQuery { get; set; }
}

public sealed class BuscarDepartamentosRequestOptions : ILoadRelatedDataOptions
{
    public bool CargarDatosExtra { get; set; }
}

public sealed class BuscarDepartamentosResponse : ContpaqiResponse<BuscarDepartamentosResponseModel>
{
    public BuscarDepartamentosResponse(BuscarDepartamentosResponseModel model) : base(model)
    {
    }

    public static BuscarDepartamentosResponse CreateInstance(List<Departamento> departamentos)
    {
        return new BuscarDepartamentosResponse(new BuscarDepartamentosResponseModel(departamentos));
    }
}

public sealed class BuscarDepartamentosResponseModel
{
    public BuscarDepartamentosResponseModel(List<Departamento> departamentos)
    {
        Departamentos = departamentos;
    }

    public int NumeroRegistros => Departamentos.Count;
    public List<Departamento> Departamentos { get; set; }
}
