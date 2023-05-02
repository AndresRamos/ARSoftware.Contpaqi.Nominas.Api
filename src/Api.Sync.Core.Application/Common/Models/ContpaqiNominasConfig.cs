using Api.Core.Domain.Models;

namespace Api.Sync.Core.Application.Common.Models;

public sealed class ContpaqiNominasConfig
{
    public Empresa Empresa { get; set; } = new();
}
