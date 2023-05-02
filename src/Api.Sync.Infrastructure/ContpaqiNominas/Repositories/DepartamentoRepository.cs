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

public sealed class DepartamentoRepository : IDepartamentoRepository
{
    private readonly ContpaqiNominasEmpresaDbContext _context;
    private readonly IMapper _mapper;

    public DepartamentoRepository(ContpaqiNominasEmpresaDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Departamento?> BuscarPorIdAsync(int id, ILoadRelatedDataOptions loadRelatedDataOptions,
        CancellationToken cancellationToken)
    {
        DepartamentoSql? departamentoSql = await _context.nom10003.Where(d => d.iddepartamento == id)
            .ProjectTo<DepartamentoSql>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (departamentoSql is null)
            return null;

        var departamento = _mapper.Map<Departamento>(departamentoSql);

        await CargarDatosRelacionadosAsync(departamento, departamentoSql, loadRelatedDataOptions, cancellationToken);

        return departamento;
    }

    public async Task<IEnumerable<Departamento>> BuscarPorRequestModelAsync(BuscarDepartamentosRequestModel requestModel,
        ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken)
    {
        var departamentosList = new List<Departamento>();

        IQueryable<nom10003> departamentosQuery = string.IsNullOrEmpty(requestModel.SqlQuery)
            ? _context.nom10003.AsQueryable()
            : _context.nom10003.FromSqlRaw($"SELECT * FROM nom10003 WHERE {requestModel.SqlQuery}");

        List<DepartamentoSql> departamentosSql = await departamentosQuery.ProjectTo<DepartamentoSql>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        foreach (DepartamentoSql departamentoSql in departamentosSql)
        {
            var departamento = _mapper.Map<Departamento>(departamentoSql);

            await CargarDatosRelacionadosAsync(departamento, departamentoSql, loadRelatedDataOptions, cancellationToken);

            departamentosList.Add(departamento);
        }

        return departamentosList;
    }

    private async Task CargarDatosRelacionadosAsync(Departamento departamento, DepartamentoSql departamentoSql,
        ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken)
    {
        if (loadRelatedDataOptions.CargarDatosExtra)
            departamento.DatosExtra =
                (await _context.nom10003.FirstAsync(m => m.iddepartamento == departamentoSql.iddepartamento, cancellationToken))
                .ToDatosDictionary<nom10003>();
    }
}
