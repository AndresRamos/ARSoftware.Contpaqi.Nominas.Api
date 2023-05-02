using System.Globalization;
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

public class EmpleadoRepository : IEmpleadoRepository
{
    private readonly ContpaqiNominasEmpresaDbContext _context;
    private readonly IDepartamentoRepository _departamentoRepository;
    private readonly IMapper _mapper;
    private readonly IPuestoRepository _puestoRepository;

    public EmpleadoRepository(ContpaqiNominasEmpresaDbContext context, IMapper mapper, IPuestoRepository puestoRepository,
        IDepartamentoRepository departamentoRepository)
    {
        _context = context;
        _mapper = mapper;
        _puestoRepository = puestoRepository;
        _departamentoRepository = departamentoRepository;
    }

    public async Task<Empleado?> BuscarPorIdAsync(int id, ILoadRelatedDataOptions loadRelatedDataOptions,
        CancellationToken cancellationToken)
    {
        EmpleadoSql? empleadoSql = await _context.nom10001.Where(d => d.idempleado == id)
            .ProjectTo<EmpleadoSql>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (empleadoSql is null)
            return null;

        var empleado = _mapper.Map<Empleado>(empleadoSql);

        await CargarDatosRelacionadosAsync(empleado, empleadoSql, loadRelatedDataOptions, cancellationToken);

        return empleado;
    }

    public async Task<IEnumerable<Empleado>> BuscarPorRequestModelAsync(BuscarEmpleadosRequestModel requestModel,
        ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken)
    {
        var empleadosList = new List<Empleado>();

        IQueryable<nom10001> empleadosQuery = string.IsNullOrEmpty(requestModel.SqlQuery)
            ? _context.nom10001.AsQueryable()
            : _context.nom10001.FromSqlRaw($"SELECT * FROM nom10001 WHERE {requestModel.SqlQuery}");

        if (requestModel.Id.HasValue)
            empleadosQuery = empleadosQuery.Where(p => p.idempleado == requestModel.Id);

        if (!string.IsNullOrWhiteSpace(requestModel.Codigo))
            empleadosQuery = empleadosQuery.Where(p => p.codigoempleado == requestModel.Codigo);

        if (!string.IsNullOrWhiteSpace(requestModel.Rfc))
        {
            string rfcInicial = requestModel.Rfc.Substring(0, 4);
            string fechaString = requestModel.Rfc.Substring(4, 6);
            DateTime fechaNacimiento = DateTime.ParseExact(fechaString, "yyMMdd", CultureInfo.CurrentCulture);
            string homoClave = requestModel.Rfc.Substring(10, 3);

            empleadosQuery =
                empleadosQuery.Where(e => e.rfc == rfcInicial && e.fechanacimiento == fechaNacimiento && e.homoclave == homoClave);
        }

        if (!string.IsNullOrWhiteSpace(requestModel.Curp))
        {
            string curpInicial = requestModel.Curp.Substring(0, 4);
            string fechaString = requestModel.Curp.Substring(4, 6);
            DateTime fechaNacimiento = DateTime.ParseExact(fechaString, "yyMMdd", CultureInfo.CurrentCulture);
            string curpfinal = requestModel.Curp.Substring(10, 8);

            empleadosQuery = empleadosQuery.Where(e =>
                e.curpi == curpInicial && e.fechanacimiento == fechaNacimiento && e.curpf == curpfinal);
        }

        List<EmpleadoSql> empleadosSql =
            await empleadosQuery.ProjectTo<EmpleadoSql>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

        foreach (EmpleadoSql? empleadoSql in empleadosSql)
        {
            var empleado = _mapper.Map<Empleado>(empleadoSql);

            await CargarDatosRelacionadosAsync(empleado, empleadoSql, loadRelatedDataOptions, cancellationToken);

            empleadosList.Add(empleado);
        }

        return empleadosList;
    }

    private async Task CargarDatosRelacionadosAsync(Empleado empleado, EmpleadoSql empleadoSql,
        ILoadRelatedDataOptions loadRelatedDataOptions, CancellationToken cancellationToken)
    {
        if (empleadoSql.idpuesto.HasValue)
            empleado.Puesto =
                await _puestoRepository.BuscarPorIdAsync(empleadoSql.idpuesto.Value, loadRelatedDataOptions, cancellationToken);

        if (empleadoSql.iddepartamento.HasValue)
            empleado.Departamento =
                await _departamentoRepository.BuscarPorIdAsync(empleadoSql.iddepartamento.Value, loadRelatedDataOptions, cancellationToken);

        if (loadRelatedDataOptions.CargarDatosExtra)
            empleado.DatosExtra = (await _context.nom10001.FirstAsync(m => m.idempleado == empleadoSql.idempleado, cancellationToken))
                .ToDatosDictionary<nom10001>();
    }
}
