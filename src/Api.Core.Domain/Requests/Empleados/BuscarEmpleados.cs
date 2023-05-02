using Api.Core.Domain.Common;
using Api.Core.Domain.Models;
using ARSoftware.Contpaqi.Api.Common.Domain;

namespace Api.Core.Domain.Requests;

public sealed class
    BuscarEmpleadosRequest : ContpaqiRequest<BuscarEmpleadosRequestModel, BuscarEmpleadosRequestOptions, BuscarEmpleadosResponse>
{
    public BuscarEmpleadosRequest(BuscarEmpleadosRequestModel model, BuscarEmpleadosRequestOptions options) : base(model, options)
    {
    }
}

public sealed class BuscarEmpleadosRequestModel
{
    public int? Id { get; set; }
    public string? Codigo { get; set; }
    public string? Rfc { get; set; }
    public string? Curp { get; set; }
    public string? SqlQuery { get; set; }
}

public sealed class BuscarEmpleadosRequestOptions : ILoadRelatedDataOptions
{
    public bool CargarDatosExtra { get; set; }
}

public sealed class BuscarEmpleadosResponse : ContpaqiResponse<BuscarEmpleadosResponseModel>
{
    public BuscarEmpleadosResponse(BuscarEmpleadosResponseModel model) : base(model)
    {
    }

    public static BuscarEmpleadosResponse CreateInstance(List<Empleado> empleados)
    {
        return new BuscarEmpleadosResponse(new BuscarEmpleadosResponseModel(empleados));
    }
}

public sealed class BuscarEmpleadosResponseModel
{
    public BuscarEmpleadosResponseModel(List<Empleado> empleados)
    {
        Empleados = empleados;
    }

    public int NumeroRegistros => Empleados.Count;
    public List<Empleado> Empleados { get; set; }
}
