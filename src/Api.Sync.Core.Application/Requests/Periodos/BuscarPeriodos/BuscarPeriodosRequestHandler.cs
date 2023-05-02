using Api.Core.Domain.Models;
using Api.Core.Domain.Requests;
using Api.Sync.Core.Application.ContpaqiNominas.Interfaces;
using MediatR;

namespace Api.Sync.Core.Application.Requests.Periodos.BuscarPeriodos;

public sealed class BuscarPeriodosRequestHandler : IRequestHandler<BuscarPeriodosRequest, BuscarPeriodosResponse>
{
    private readonly IPeriodoRepository _periodoRepository;

    public BuscarPeriodosRequestHandler(IPeriodoRepository periodoRepository)
    {
        _periodoRepository = periodoRepository;
    }

    public async Task<BuscarPeriodosResponse> Handle(BuscarPeriodosRequest request, CancellationToken cancellationToken)
    {
        List<Periodo> periodos = (await _periodoRepository.BuscarPorRequestModelAsync(request.Model, request.Options, cancellationToken))
            .ToList();

        return BuscarPeriodosResponse.CreateInstance(periodos);
    }
}
