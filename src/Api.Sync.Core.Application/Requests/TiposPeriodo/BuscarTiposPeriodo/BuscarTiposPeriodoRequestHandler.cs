using Api.Core.Domain.Models;
using Api.Core.Domain.Requests;
using Api.Sync.Core.Application.ContpaqiNominas.Interfaces;
using MediatR;

namespace Api.Sync.Core.Application.Requests.TiposPeriodo.BuscarTiposPeriodo;

public sealed class BuscarTiposPeriodoRequestHandler : IRequestHandler<BuscarTiposPeriodoRequest, BuscarTiposPeriodoResponse>
{
    private readonly ITipoPeriodoRepository _tipoPeriodoRepository;

    public BuscarTiposPeriodoRequestHandler(ITipoPeriodoRepository tipoPeriodoRepository)
    {
        _tipoPeriodoRepository = tipoPeriodoRepository;
    }

    public async Task<BuscarTiposPeriodoResponse> Handle(BuscarTiposPeriodoRequest request, CancellationToken cancellationToken)
    {
        List<TipoPeriodo> tiposPeriodo =
            (await _tipoPeriodoRepository.BuscarPorRequestModelAsync(request.Model, request.Options, cancellationToken)).ToList();

        return BuscarTiposPeriodoResponse.CreateInstance(tiposPeriodo);
    }
}
