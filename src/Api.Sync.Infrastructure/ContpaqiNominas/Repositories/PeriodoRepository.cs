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

namespace Api.Sync.Infrastructure.ContpaqiNominas.Repositories;

public sealed class PeriodoRepository : IPeriodoRepository
{
    private readonly ContpaqiNominasEmpresaDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITipoPeriodoRepository _tipoPeriodoRepository;

    public PeriodoRepository(ContpaqiNominasEmpresaDbContext context, IMapper mapper, ITipoPeriodoRepository tipoPeriodoRepository)
    {
        _context = context;
        _mapper = mapper;
        _tipoPeriodoRepository = tipoPeriodoRepository;
    }

    public async Task<Periodo?> BuscarPorIdAsync(int id, ILoadRelatedDataOptions loadRelatedDataOptions,
        CancellationToken cancellationToken)
    {
        PeriodoSql? periodoSql = await _context.nom10002.Where(d => d.idperiodo == id)
            .ProjectTo<PeriodoSql>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (periodoSql is null)
            return null;

        var periodo = _mapper.Map<Periodo>(periodoSql);

        await CargarDatosRelacionadosAsync(periodo, periodoSql, loadRelatedDataOptions, cancellationToken);

        return periodo;
    }

    public async Task<IEnumerable<Periodo>> BuscarPorRequestModelAsync(BuscarPeriodosRequestModel requestModel,
        ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken)
    {
        var periodosList = new List<Periodo>();

        IQueryable<nom10002> periodosQuery = string.IsNullOrEmpty(requestModel.SqlQuery)
            ? _context.nom10002.AsQueryable()
            : _context.nom10002.FromSqlRaw($"SELECT * FROM nom10002 WHERE {requestModel.SqlQuery}");

        if (!string.IsNullOrWhiteSpace(requestModel.TipoPeriodo))
        {
            TipoPeriodo? tipoPerido = (await _tipoPeriodoRepository.BuscarPorRequestModelAsync(
                new BuscarTiposPeriodoRequestModel { Nombre = requestModel.TipoPeriodo }, LoadRelatedDataOptions.Default,
                cancellationToken)).FirstOrDefault();

            periodosQuery = periodosQuery.Where(m => m.idtipoperiodo == tipoPerido!.Id);
        }

        if (requestModel.Numero.HasValue)
            periodosQuery = periodosQuery.Where(m => m.numeroperiodo == requestModel.Numero.Value);

        if (requestModel.Ejercicio.HasValue)
            periodosQuery = periodosQuery.Where(m => m.ejercicio == requestModel.Ejercicio.Value);

        if (requestModel.Mes.HasValue)
            periodosQuery = periodosQuery.Where(m => m.mes == requestModel.Mes.Value);

        List<PeriodoSql> periodosSql =
            await periodosQuery.ProjectTo<PeriodoSql>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        foreach (PeriodoSql periodoSql in periodosSql)
        {
            var periodo = _mapper.Map<Periodo>(periodoSql);

            await CargarDatosRelacionadosAsync(periodo, periodoSql, loadRelatedDataOptions, cancellationToken);

            periodosList.Add(periodo);
        }

        return periodosList;
    }

    private async Task CargarDatosRelacionadosAsync(Periodo periodo, PeriodoSql periodoSql, ILoadRelatedDataOptions loadRelatedDataOptions,
        CancellationToken cancellationToken)
    {
        if (periodoSql.idtipoperiodo.HasValue)
            periodo.TipoPeriodo =
                await _tipoPeriodoRepository.BuscarPorIdAsync(periodoSql.idtipoperiodo.Value, loadRelatedDataOptions, cancellationToken) ??
                new TipoPeriodo();

        if (loadRelatedDataOptions.CargarDatosExtra)
            periodo.DatosExtra = (await _context.nom10002.FirstAsync(m => m.idperiodo == periodoSql.idperiodo, cancellationToken))
                .ToDatosDictionary<nom10002>();
    }
}
