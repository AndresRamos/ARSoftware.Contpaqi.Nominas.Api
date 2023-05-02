using Api.Core.Domain.Common;
using Api.Core.Domain.Models;
using Api.Core.Domain.Requests;
using Api.Sync.Core.Application.ContpaqiNominas.Interfaces;
using Api.Sync.Infrastructure.ContpaqiNominas.Extensions;
using Api.Sync.Infrastructure.ContpaqiNominas.Models;
using ARSoftware.Contpaqi.Nominas.Sql.Contexts;
using ARSoftware.Contpaqi.Nominas.Sql.Models.Empresa;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.Sync.Infrastructure.ContpaqiNominas.Repositories;

public sealed class TipoPeriodoRepository : ITipoPeriodoRepository
{
    private readonly ContpaqiNominasEmpresaDbContext _context;
    private readonly IMapper _mapper;

    public TipoPeriodoRepository(ContpaqiNominasEmpresaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TipoPeriodo?> BuscarPorIdAsync(int id, ILoadRelatedDataOptions loadRelatedDataOptions,
        CancellationToken cancellationToken)
    {
        TipoPeriodoSql? tipoPeriodoSql = await _context.nom10023.Where(d => d.idtipoperiodo == id)
            .ProjectTo<TipoPeriodoSql>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (tipoPeriodoSql is null)
            return null;

        var tipoPeriodo = _mapper.Map<TipoPeriodo>(tipoPeriodoSql);

        await CargarDatosRelacionadosAsync(tipoPeriodo, tipoPeriodoSql, loadRelatedDataOptions, cancellationToken);

        return tipoPeriodo;
    }

    public async Task<IEnumerable<TipoPeriodo>> BuscarPorRequestModelAsync(BuscarTiposPeriodoRequestModel requestModel,
        ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken)
    {
        var tiposPeriodoList = new List<TipoPeriodo>();

        IQueryable<nom10023> tiposPeriodoQuery = string.IsNullOrEmpty(requestModel.SqlQuery)
            ? _context.nom10023.AsQueryable()
            : _context.nom10023.FromSqlRaw($"SELECT * FROM nom10023 WHERE {requestModel.SqlQuery}");

        if (!string.IsNullOrEmpty(requestModel.Nombre))
            tiposPeriodoQuery = tiposPeriodoQuery.Where(d => d.nombretipoperiodo == requestModel.Nombre);

        List<TipoPeriodoSql> tiposPeriodoSql =
            await tiposPeriodoQuery.ProjectTo<TipoPeriodoSql>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        foreach (TipoPeriodoSql tipoPeriodoSql in tiposPeriodoSql)
        {
            var tipoPeriodo = _mapper.Map<TipoPeriodo>(tipoPeriodoSql);

            await CargarDatosRelacionadosAsync(tipoPeriodo, tipoPeriodoSql, loadRelatedDataOptions, cancellationToken);

            tiposPeriodoList.Add(tipoPeriodo);
        }

        return tiposPeriodoList;
    }

    private async Task CargarDatosRelacionadosAsync(TipoPeriodo tipoPerido, TipoPeriodoSql tipoPeriodoSql,
        ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken)
    {
        if (loadRelatedDataOptions.CargarDatosExtra)
            tipoPerido.DatosExtra =
                (await _context.nom10023.FirstAsync(m => m.idtipoperiodo == tipoPeriodoSql.idtipoperiodo, cancellationToken))
                .ToDatosDictionary<nom10023>();
    }
}
