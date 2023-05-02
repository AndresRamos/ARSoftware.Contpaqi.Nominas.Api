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

public sealed class PuestoRepository : IPuestoRepository
{
    private readonly ContpaqiNominasEmpresaDbContext _context;
    private readonly IMapper _mapper;

    public PuestoRepository(ContpaqiNominasEmpresaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Puesto?> BuscarPorIdAsync(int id, ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken)
    {
        PuestoSql? puestoSql = await _context.nom10006.Where(d => d.idpuesto == id)
            .ProjectTo<PuestoSql>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (puestoSql is null)
            return null;

        var puesto = _mapper.Map<Puesto>(puestoSql);

        await CargarDatosRelacionadosAsync(puesto, puestoSql, loadRelatedDataOptions, cancellationToken);

        return puesto;
    }

    public async Task<IEnumerable<Puesto>> BuscarPorRequestModelAsync(BuscarPuestosRequestModel requestModel,
        ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken)
    {
        var puestosList = new List<Puesto>();

        IQueryable<nom10006> puestosQuery = string.IsNullOrEmpty(requestModel.SqlQuery)
            ? _context.nom10006.AsQueryable()
            : _context.nom10006.FromSqlRaw($"SELECT * FROM nom10006 WHERE {requestModel.SqlQuery}");

        List<PuestoSql> puestosSql = await puestosQuery.ProjectTo<PuestoSql>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        foreach (PuestoSql puestoSql in puestosSql)
        {
            var puesto = _mapper.Map<Puesto>(puestoSql);

            await CargarDatosRelacionadosAsync(puesto, puestoSql, loadRelatedDataOptions, cancellationToken);

            puestosList.Add(puesto);
        }

        return puestosList;
    }

    private async Task CargarDatosRelacionadosAsync(Puesto puesto, PuestoSql puestoSql, ILoadRelatedDataOptions loadRelatedDataOptions,
        CancellationToken cancellationToken)
    {
        if (loadRelatedDataOptions.CargarDatosExtra)
            puesto.DatosExtra = (await _context.nom10006.FirstAsync(m => m.idpuesto == puestoSql.idpuesto, cancellationToken))
                .ToDatosDictionary<nom10006>();
    }
}
