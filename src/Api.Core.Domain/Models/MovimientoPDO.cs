// ReSharper disable InconsistentNaming

namespace Api.Core.Domain.Models;

public sealed class MovimientoPDO
{
    public Periodo Periodo { get; set; } = new();
    public Empleado Empleado { get; set; } = new();
    public Concepto Concepto { get; set; } = new();
    public decimal ImporteTotal { get; set; }
    public Dictionary<string, string> DatosExtra { get; set; } = new();
}
