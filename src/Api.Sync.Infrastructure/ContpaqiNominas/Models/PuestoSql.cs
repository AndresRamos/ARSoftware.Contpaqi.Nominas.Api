// ReSharper disable InconsistentNaming

namespace Api.Sync.Infrastructure.ContpaqiNominas.Models;

/// <summary>
///     NOM10006
/// </summary>
public sealed class PuestoSql
{
    public int idpuesto { get; set; }

    public int? numeropuesto { get; set; }

    public string? descripcion { get; set; }

    public string? Detalle { get; set; }
}
