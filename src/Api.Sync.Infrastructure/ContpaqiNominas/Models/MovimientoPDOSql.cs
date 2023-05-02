// ReSharper disable InconsistentNaming

namespace Api.Sync.Infrastructure.ContpaqiNominas.Models;

/// <summary>
///     NOM10007 (Historia) y NOM10008
/// </summary>
public sealed class MovimientoPDOSql
{
    public int idmovtopdo { get; set; }

    public int? idperiodo { get; set; }

    public int? idempleado { get; set; }

    public int? idconcepto { get; set; }
    public double? importetotal { get; set; }
}
