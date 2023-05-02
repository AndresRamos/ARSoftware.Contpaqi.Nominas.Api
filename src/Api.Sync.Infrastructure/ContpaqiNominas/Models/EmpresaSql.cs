// ReSharper disable InconsistentNaming

namespace Api.Sync.Infrastructure.ContpaqiNominas.Models;

/// <summary>
///     NOM10000
/// </summary>
public sealed class EmpresaSql
{
    public string NombreEmpresa { get; set; } = string.Empty;

    public int IDEmpresa { get; set; }
    public string? RutaEmpresa { get; set; }
    public string? RFC { get; set; }

    public string? Homoclave { get; set; }
    public DateTime? FechaConstitucion { get; set; }
    public string GUIDDSL { get; set; } = string.Empty;
}
