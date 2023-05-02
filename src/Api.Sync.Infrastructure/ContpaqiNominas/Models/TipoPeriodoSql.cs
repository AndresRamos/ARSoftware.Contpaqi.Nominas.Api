// ReSharper disable InconsistentNaming

namespace Api.Sync.Infrastructure.ContpaqiNominas.Models;

/// <summary>
///     NOM10023
/// </summary>
public sealed class TipoPeriodoSql
{
    public int idtipoperiodo { get; set; }
    public string? nombretipoperiodo { get; set; }
    public int? diasdelperiodo { get; set; }
    public int? periodotrabajo { get; set; }
}
