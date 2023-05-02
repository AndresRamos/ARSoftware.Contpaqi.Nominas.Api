// ReSharper disable InconsistentNaming

namespace Api.Sync.Infrastructure.ContpaqiNominas.Models;

/// <summary>
///     NOM10004
/// </summary>
public sealed class ConceptoSql
{
    public int idconcepto { get; set; }

    public int? numeroconcepto { get; set; }

    public string? tipoconcepto { get; set; }

    public string? descripcion { get; set; }
    public string? ClaveAgrupadoraSAT { get; set; }
    public string TipoClaveSAT { get; set; } = string.Empty;
}
