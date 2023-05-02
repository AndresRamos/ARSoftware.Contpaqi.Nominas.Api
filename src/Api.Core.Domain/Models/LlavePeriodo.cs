namespace Api.Core.Domain.Models;

public sealed class LlavePeriodo
{
    public string TipoPeriodo { get; set; } = string.Empty;
    public int Numero { get; set; }
    public int Ejercicio { get; set; }
    public int Mes { get; set; }
}
