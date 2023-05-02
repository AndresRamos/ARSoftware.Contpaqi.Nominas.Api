using Api.Core.Domain.Models;
using Api.Core.Domain.Requests;
using Api.Sync.Core.Application.ContpaqiNominas.Interfaces;
using MediatR;

namespace Api.Sync.Core.Application.Requests.Puestos.BuscarPuestos;

public sealed class BuscarPuestosRequestHandler : IRequestHandler<BuscarPuestosRequest, BuscarPuestosResponse>
{
    private readonly IPuestoRepository _puestoRepository;

    public BuscarPuestosRequestHandler(IPuestoRepository puestoRepository)
    {
        _puestoRepository = puestoRepository;
    }

    public async Task<BuscarPuestosResponse> Handle(BuscarPuestosRequest request, CancellationToken cancellationToken)
    {
        List<Puesto> puestos = (await _puestoRepository.BuscarPorRequestModelAsync(request.Model, request.Options, cancellationToken))
            .ToList();

        return BuscarPuestosResponse.CreateInstance(puestos);
    }
}
