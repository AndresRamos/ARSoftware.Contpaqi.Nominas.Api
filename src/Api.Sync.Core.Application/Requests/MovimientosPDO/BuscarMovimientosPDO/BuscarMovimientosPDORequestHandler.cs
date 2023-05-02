using Api.Core.Domain.Models;
using Api.Core.Domain.Requests;
using Api.Sync.Core.Application.ContpaqiNominas.Interfaces;
using MediatR;

// ReSharper disable InconsistentNaming

namespace Api.Sync.Core.Application.Requests.MovimientosPDO.BuscarMovimientosPDO;

public sealed class BuscarMovimientosPDORequestHandler : IRequestHandler<BuscarMovimientosPDORequest, BuscarMovimientosPDOResponse>
{
    private readonly IMovimientoPDORepository _movimientoPdoRepository;

    public BuscarMovimientosPDORequestHandler(IMovimientoPDORepository movimientoPdoRepository)
    {
        _movimientoPdoRepository = movimientoPdoRepository;
    }

    public async Task<BuscarMovimientosPDOResponse> Handle(BuscarMovimientosPDORequest request, CancellationToken cancellationToken)
    {
        List<MovimientoPDO> movimientos =
            (await _movimientoPdoRepository.BuscarPorRequestModelAsync(request.Model, request.Options, cancellationToken)).ToList();

        return BuscarMovimientosPDOResponse.CreateInstance(movimientos);
    }
}
