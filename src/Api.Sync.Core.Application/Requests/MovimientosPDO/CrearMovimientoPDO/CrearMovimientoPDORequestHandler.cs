using Api.Core.Domain.Models;
using Api.Core.Domain.Requests;
using Api.Sync.Core.Application.Common.Models;
using Api.Sync.Core.Application.ContpaqiNominas.Interfaces;
using MediatR;

// ReSharper disable InconsistentNaming

namespace Api.Sync.Core.Application.Requests.MovimientosPDO.CrearMovimientoPDO;

public sealed class CrearMovimientoPDORequestHandler : IRequestHandler<CrearMovimientoPDORequest, CrearMovimientoPDOResponse>
{
    private readonly IMovimientoPDORepository _movimientoPdoRepository;

    public CrearMovimientoPDORequestHandler(IMovimientoPDORepository movimientoPdoRepository)
    {
        _movimientoPdoRepository = movimientoPdoRepository;
    }

    public async Task<CrearMovimientoPDOResponse> Handle(CrearMovimientoPDORequest request, CancellationToken cancellationToken)
    {
        MovimientoPDO? movimiento = (await _movimientoPdoRepository.BuscarPorRequestModelAsync(
            new BuscarMovimientosPDORequestModel
            {
                Periodo = request.Model.Periodo,
                EmpleadoCodigo = request.Model.EmpleadoCodigo,
                ConceptoNumero = request.Model.ConceptoNumero
            }, LoadRelatedDataOptions.Default, cancellationToken)).FirstOrDefault();

        if (movimiento is null)
            await _movimientoPdoRepository.CrearAsync(request.Model, cancellationToken);
        else
            await _movimientoPdoRepository.ActualizarAsync(request.Model, cancellationToken);

        movimiento = (await _movimientoPdoRepository.BuscarPorRequestModelAsync(
            new BuscarMovimientosPDORequestModel
            {
                Periodo = request.Model.Periodo,
                EmpleadoCodigo = request.Model.EmpleadoCodigo,
                ConceptoNumero = request.Model.ConceptoNumero
            }, LoadRelatedDataOptions.Default, cancellationToken)).First();

        return CrearMovimientoPDOResponse.CreateInstance(movimiento);
    }
}
