// ReSharper disable InconsistentNaming

namespace Api.Sync.Infrastructure.ContpaqiNominas.Models;

/// <summary>
///     NOM10001
/// </summary>
public sealed class EmpleadoSql
{
    public int idempleado { get; set; }
    public int? iddepartamento { get; set; }
    public int? idpuesto { get; set; }
    public string? codigoempleado { get; set; }
    public string? nombre { get; set; }
    public string? apellidopaterno { get; set; }
    public string? apellidomaterno { get; set; }
    public string? nombrelargo { get; set; }
    public DateTime? fechanacimiento { get; set; }
    public string? rfc { get; set; }
    public string? homoclave { get; set; }
    public string? curpi { get; set; }
    public string? curpf { get; set; }
}
