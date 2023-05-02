using Api.Core.Domain.Models;
using Api.Core.Domain.Requests;
using Api.Sync.Core.Application.ContpaqiNominas.Interfaces;
using MediatR;

namespace Api.Sync.Core.Application.Requests.Departamentos.BuscarDepartamentos;

public sealed class BuscarDepartamentosRequestHandler : IRequestHandler<BuscarDepartamentosRequest, BuscarDepartamentosResponse>
{
    private readonly IDepartamentoRepository _departamentoRepository;

    public BuscarDepartamentosRequestHandler(IDepartamentoRepository departamentoRepository)
    {
        _departamentoRepository = departamentoRepository;
    }

    public async Task<BuscarDepartamentosResponse> Handle(BuscarDepartamentosRequest request, CancellationToken cancellationToken)
    {
        List<Departamento> departamentos =
            (await _departamentoRepository.BuscarPorRequestModelAsync(request.Model, request.Options, cancellationToken)).ToList();

        return BuscarDepartamentosResponse.CreateInstance(departamentos);
    }
}
