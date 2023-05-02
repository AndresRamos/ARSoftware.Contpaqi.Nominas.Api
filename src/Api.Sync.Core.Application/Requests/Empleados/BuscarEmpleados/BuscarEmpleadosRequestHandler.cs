using Api.Core.Domain.Models;
using Api.Core.Domain.Requests;
using Api.Sync.Core.Application.ContpaqiNominas.Interfaces;
using MediatR;

namespace Api.Sync.Core.Application.Requests.Empleados.BuscarEmpleados;

public sealed class BuscarEmpleadosRequestHandler : IRequestHandler<BuscarEmpleadosRequest, BuscarEmpleadosResponse>
{
    private readonly IEmpleadoRepository _empleadoRepository;

    public BuscarEmpleadosRequestHandler(IEmpleadoRepository empleadoRepository)
    {
        _empleadoRepository = empleadoRepository;
    }

    public async Task<BuscarEmpleadosResponse> Handle(BuscarEmpleadosRequest request, CancellationToken cancellationToken)
    {
        List<Empleado> empleados = (await _empleadoRepository.BuscarPorRequestModelAsync(request.Model, request.Options, cancellationToken))
            .ToList();

        return BuscarEmpleadosResponse.CreateInstance(empleados);
    }
}
