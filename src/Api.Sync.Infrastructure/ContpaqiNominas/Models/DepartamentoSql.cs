// ReSharper disable InconsistentNaming

namespace Api.Sync.Infrastructure.ContpaqiNominas.Models;

/// <summary>
///     NOM10003
/// </summary>
public sealed class DepartamentoSql
{
    public int iddepartamento { get; set; }

    public int? numerodepartamento { get; set; }

    public string? descripcion { get; set; }
}
