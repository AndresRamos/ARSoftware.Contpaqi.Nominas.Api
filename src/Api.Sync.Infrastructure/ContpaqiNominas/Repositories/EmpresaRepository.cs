using Api.Core.Domain.Common;
using Api.Core.Domain.Models;
using Api.Sync.Core.Application.ContpaqiNominas.Interfaces;
using Api.Sync.Infrastructure.ContpaqiNominas.Extensions;
using Api.Sync.Infrastructure.ContpaqiNominas.Models;
using ARSoftware.Contpaqi.Nominas.Sql.Contexts;
using ARSoftware.Contpaqi.Nominas.Sql.Models.Generales;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Api.Sync.Infrastructure.ContpaqiNominas.Repositories;

public sealed class EmpresaRepository : IEmpresaRepository
{
    private readonly ContpaqiNominasGeneralesDbContext _contpaqiNominasGeneralesDbContext;
    private readonly IMapper _mapper;

    public EmpresaRepository(ContpaqiNominasGeneralesDbContext contpaqiNominasGeneralesDbContext, IMapper mapper)
    {
        _contpaqiNominasGeneralesDbContext = contpaqiNominasGeneralesDbContext;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Empresa>> BuscarTodoAsync(ILoadRelatedDataOptions loadRelatedDataOptions,
        CancellationToken cancellationToken)
    {
        var empresasList = new List<Empresa>();

        List<EmpresaSql> empresasSql = await _contpaqiNominasGeneralesDbContext.NOM10000.Where(e => e.IDEmpresa != 1)
            .OrderBy(m => m.NombreEmpresa)
            .ProjectTo<EmpresaSql>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        foreach (EmpresaSql? empresaSql in empresasSql)
        {
            var empresa = _mapper.Map<Empresa>(empresaSql);

            await CargarDatosRelacionadosAsync(empresa, empresaSql, loadRelatedDataOptions, cancellationToken);

            empresasList.Add(empresa);
        }

        return empresasList;
    }

    private async Task CargarDatosRelacionadosAsync(Empresa empresa, EmpresaSql empresaSql, ILoadRelatedDataOptions loadRelatedDataOptions,
        CancellationToken cancellationToken)
    {
        if (loadRelatedDataOptions.CargarDatosExtra)
            empresa.DatosExtra =
                (await _contpaqiNominasGeneralesDbContext.NOM10000.FirstAsync(e => e.IDEmpresa == empresaSql.IDEmpresa, cancellationToken))
                .ToDatosDictionary<NOM10000>();
    }
}
