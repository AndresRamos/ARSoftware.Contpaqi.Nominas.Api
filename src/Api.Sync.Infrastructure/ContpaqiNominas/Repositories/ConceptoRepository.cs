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

public sealed class ConceptoRepository : IConceptoRepository
{
    private readonly ContpaqiNominasEmpresaDbContext _context;
    private readonly IMapper _mapper;

    public ConceptoRepository(ContpaqiNominasEmpresaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Concepto?> BuscarPorIdAsync(int id, ILoadRelatedDataOptions loadRelatedDataOptions,
        CancellationToken cancellationToken)
    {
        ConceptoSql? conceptoSql = await _context.nom10004.Where(d => d.idconcepto == id)
            .ProjectTo<ConceptoSql>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (conceptoSql is null)
            return null;

        var concepto = _mapper.Map<Concepto>(conceptoSql);

        await CargarDatosRelacionadosAsync(concepto, conceptoSql, loadRelatedDataOptions, cancellationToken);

        return concepto;
    }

    public async Task<IEnumerable<Concepto>> BuscarPorRequestModelAsync(BuscarConceptosRequestModel requestModel,
        ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken)
    {
        var conceptosList = new List<Concepto>();

        IQueryable<nom10004> conceptosQuery = string.IsNullOrEmpty(requestModel.SqlQuery)
            ? _context.nom10004.AsQueryable()
            : _context.nom10004.FromSqlRaw($"SELECT * FROM nom10004 WHERE {requestModel.SqlQuery}");

        if (requestModel.Numero.HasValue)
            conceptosQuery = conceptosQuery.Where(d => d.numeroconcepto == requestModel.Numero.Value);

        List<ConceptoSql> conceptosSql =
            await conceptosQuery.ProjectTo<ConceptoSql>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        foreach (ConceptoSql conceptoSql in conceptosSql)
        {
            var concepto = _mapper.Map<Concepto>(conceptoSql);

            await CargarDatosRelacionadosAsync(concepto, conceptoSql, loadRelatedDataOptions, cancellationToken);

            conceptosList.Add(concepto);
        }

        return conceptosList;
    }

    private async Task CargarDatosRelacionadosAsync(Concepto concepto, ConceptoSql conceptoSql,
        ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken)
    {
        if (loadRelatedDataOptions.CargarDatosExtra)
            concepto.DatosExtra = (await _context.nom10004.FirstAsync(m => m.idconcepto == conceptoSql.idconcepto, cancellationToken))
                .ToDatosDictionary<nom10004>();
    }
}
