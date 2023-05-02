// ReSharper disable InconsistentNaming

namespace Api.Sync.Infrastructure.ContpaqiNominas.Models;

/// <summary>
///     NOM10002
/// </summary>
public sealed class PeriodoSql
{
    public int idperiodo { get; set; }

    public int? idtipoperiodo { get; set; }

    public int? numeroperiodo { get; set; }

    public int? ejercicio { get; set; }

    public int? mes { get; set; }

    public DateTime? fechainicio { get; set; }

    public DateTime? fechafin { get; set; }
}
