namespace Api.Core.Domain.Models;

public sealed class Periodo
{
    public int Id { get; set; }
    public TipoPeriodo TipoPeriodo { get; set; } = new();
    public int Numero { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public int Ejercicio { get; set; }
    public int Mes { get; set; }
    public Dictionary<string, string> DatosExtra { get; set; } = new();
}
