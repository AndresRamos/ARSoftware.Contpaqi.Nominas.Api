using Api.Core.Domain.Common;
using Api.Core.Domain.Models;
using Api.Core.Domain.Requests;

// ReSharper disable InconsistentNaming

namespace Api.Sync.Core.Application.ContpaqiNominas.Interfaces;

public interface IMovimientoPDORepository
{
    Task<IEnumerable<MovimientoPDO>> BuscarPorRequestModelAsync(BuscarMovimientosPDORequestModel requestModel,
        ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken);

    Task ActualizarAsync(CrearMovimientoPDORequestModel requestModel, CancellationToken cancellationToken);
    Task<int> CrearAsync(CrearMovimientoPDORequestModel requestModel, CancellationToken cancellationToken);
}
