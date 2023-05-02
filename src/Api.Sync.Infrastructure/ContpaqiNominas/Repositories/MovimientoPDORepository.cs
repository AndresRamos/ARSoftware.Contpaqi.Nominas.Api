using Api.Core.Domain.Common;
using Api.Core.Domain.Models;
using Api.Core.Domain.Requests;
using Api.Sync.Core.Application.Common.Models;
using Api.Sync.Core.Application.ContpaqiNominas.Interfaces;
using Api.Sync.Infrastructure.ContpaqiNominas.Extensions;
using Api.Sync.Infrastructure.ContpaqiNominas.Models;
using ARSoftware.Contpaqi.Nominas.Sql.Contexts;
using ARSoftware.Contpaqi.Nominas.Sql.Models.Empresa;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

// ReSharper disable InconsistentNaming

namespace Api.Sync.Infrastructure.ContpaqiNominas.Repositories;

public sealed class MovimientoPDORepository : IMovimientoPDORepository
{
    private readonly IConceptoRepository _conceptoRepository;
    private readonly ContpaqiNominasEmpresaDbContext _context;
    private readonly IEmpleadoRepository _empleadoRepository;
    private readonly IMapper _mapper;
    private readonly IPeriodoRepository _periodoRepository;

    public MovimientoPDORepository(ContpaqiNominasEmpresaDbContext context, IMapper mapper, IEmpleadoRepository empleadoRepository,
        IConceptoRepository conceptoRepository, IPeriodoRepository periodoRepository)
    {
        _context = context;
        _mapper = mapper;
        _empleadoRepository = empleadoRepository;
        _conceptoRepository = conceptoRepository;
        _periodoRepository = periodoRepository;
    }

    public async Task<IEnumerable<MovimientoPDO>> BuscarPorRequestModelAsync(BuscarMovimientosPDORequestModel requestModel,
        ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken)
    {
        var movimientosList = new List<MovimientoPDO>();

        IQueryable<nom10008> movimientosQuery = string.IsNullOrEmpty(requestModel.SqlQuery)
            ? _context.nom10008.AsQueryable()
            : _context.nom10008.FromSqlRaw($"SELECT * FROM nom10008 WHERE {requestModel.SqlQuery}");

        if (requestModel.Periodo is not null)
        {
            Periodo? periodo = (await _periodoRepository.BuscarPorRequestModelAsync(
                new BuscarPeriodosRequestModel
                {
                    TipoPeriodo = requestModel.Periodo.TipoPeriodo,
                    Numero = requestModel.Periodo.Numero,
                    Ejercicio = requestModel.Periodo.Ejercicio,
                    Mes = requestModel.Periodo.Mes
                }, LoadRelatedDataOptions.Default, cancellationToken)).FirstOrDefault();

            if (periodo is not null)
                movimientosQuery = movimientosQuery.Where(m => m.idperiodo == periodo.Id);
        }

        if (!string.IsNullOrWhiteSpace(requestModel.EmpleadoCodigo))
        {
            Empleado? empleado =
                (await _empleadoRepository.BuscarPorRequestModelAsync(
                    new BuscarEmpleadosRequestModel { Codigo = requestModel.EmpleadoCodigo }, LoadRelatedDataOptions.Default,
                    cancellationToken)).FirstOrDefault();

            movimientosQuery = movimientosQuery.Where(m => m.idempleado == empleado!.Id);
        }

        if (requestModel.ConceptoNumero.HasValue)
        {
            Concepto? concepto =
                (await _conceptoRepository.BuscarPorRequestModelAsync(
                    new BuscarConceptosRequestModel { Numero = requestModel.ConceptoNumero }, LoadRelatedDataOptions.Default,
                    cancellationToken)).FirstOrDefault();

            movimientosQuery = movimientosQuery.Where(m => m.idconcepto == concepto!.Id);
        }

        List<MovimientoPDOSql> movimientosSql =
            await movimientosQuery.ProjectTo<MovimientoPDOSql>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        foreach (MovimientoPDOSql movimientoSql in movimientosSql)
        {
            var movimiento = _mapper.Map<MovimientoPDO>(movimientoSql);

            await CargarDatosRelacionadosAsync(movimiento, movimientoSql, loadRelatedDataOptions, cancellationToken);

            movimientosList.Add(movimiento);
        }

        return movimientosList;
    }

    public async Task ActualizarAsync(CrearMovimientoPDORequestModel requestModel, CancellationToken cancellationToken)
    {
        Empleado empleado = (await _empleadoRepository.BuscarPorRequestModelAsync(
                new BuscarEmpleadosRequestModel { Codigo = requestModel.EmpleadoCodigo }, LoadRelatedDataOptions.Default,
                cancellationToken))
            .First();

        Periodo periodo = (await _periodoRepository.BuscarPorRequestModelAsync(
            new BuscarPeriodosRequestModel
            {
                TipoPeriodo = requestModel.Periodo.TipoPeriodo,
                Numero = requestModel.Periodo.Numero,
                Ejercicio = requestModel.Periodo.Ejercicio,
                Mes = requestModel.Periodo.Mes
            }, LoadRelatedDataOptions.Default, cancellationToken)).First();

        Concepto concepto = (await _conceptoRepository.BuscarPorRequestModelAsync(
                new BuscarConceptosRequestModel { Numero = requestModel.ConceptoNumero }, LoadRelatedDataOptions.Default,
                cancellationToken))
            .First();

        nom10008 movimiento = await _context.nom10008
            .Where(p => p.idempleado == empleado.Id && p.idperiodo == periodo.Id && p.idconcepto == concepto.Id)
            .FirstAsync(cancellationToken);

        movimiento.importetotal = (double?)requestModel.Importe;
        movimiento.importetotalreportado = true;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> CrearAsync(CrearMovimientoPDORequestModel requestModel, CancellationToken cancellationToken)
    {
        Empleado empleado = (await _empleadoRepository.BuscarPorRequestModelAsync(
                new BuscarEmpleadosRequestModel { Codigo = requestModel.EmpleadoCodigo }, LoadRelatedDataOptions.Default,
                cancellationToken))
            .First();

        Periodo periodo = (await _periodoRepository.BuscarPorRequestModelAsync(
            new BuscarPeriodosRequestModel
            {
                TipoPeriodo = requestModel.Periodo.TipoPeriodo,
                Numero = requestModel.Periodo.Numero,
                Ejercicio = requestModel.Periodo.Ejercicio,
                Mes = requestModel.Periodo.Mes
            }, LoadRelatedDataOptions.Default, cancellationToken)).First();

        Concepto concepto = (await _conceptoRepository.BuscarPorRequestModelAsync(
                new BuscarConceptosRequestModel { Numero = requestModel.ConceptoNumero }, LoadRelatedDataOptions.Default,
                cancellationToken))
            .First();

        var movimiento = new nom10008
        {
            idempleado = empleado.Id,
            idperiodo = periodo.Id,
            idconcepto = concepto.Id,
            importetotal = (double?)requestModel.Importe,
            importetotalreportado = true
        };

        _context.nom10008.Add(movimiento);
        await _context.SaveChangesAsync(cancellationToken);
        return movimiento.idmovtopdo;
    }

    private async Task CargarDatosRelacionadosAsync(MovimientoPDO movimiento, MovimientoPDOSql movimientoSql,
        ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken)
    {
        if (movimientoSql.idperiodo.HasValue)
            movimiento.Periodo =
                await _periodoRepository.BuscarPorIdAsync(movimientoSql.idperiodo.Value, loadRelatedDataOptions, cancellationToken) ??
                new Periodo();

        if (movimientoSql.idempleado.HasValue)
            movimiento.Empleado =
                await _empleadoRepository.BuscarPorIdAsync(movimientoSql.idempleado.Value, loadRelatedDataOptions, cancellationToken) ??
                new Empleado();

        if (movimientoSql.idconcepto.HasValue)
            movimiento.Concepto =
                await _conceptoRepository.BuscarPorIdAsync(movimientoSql.idconcepto.Value, loadRelatedDataOptions, cancellationToken) ??
                new Concepto();

        if (loadRelatedDataOptions.CargarDatosExtra)
            movimiento.DatosExtra = (await _context.nom10008.FirstAsync(m => m.idperiodo == movimientoSql.idperiodo, cancellationToken))
                .ToDatosDictionary<nom10008>();
    }
}
