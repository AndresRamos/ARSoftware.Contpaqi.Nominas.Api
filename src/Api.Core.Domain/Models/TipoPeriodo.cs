namespace Api.Core.Domain.Models;

public sealed class TipoPeriodo
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int DiasDelPeriodo { get; set; }
    public int PeriodoTrabajo { get; set; }
    public Dictionary<string, string> DatosExtra { get; set; } = new();
}
