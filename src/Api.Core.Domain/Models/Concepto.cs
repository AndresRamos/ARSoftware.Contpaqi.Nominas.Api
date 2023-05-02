namespace Api.Core.Domain.Models;

public sealed class Concepto
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string ClaveAgrupadoraSat { get; set; } = string.Empty;
    public string TipoClaveSat { get; set; } = string.Empty;
    public Dictionary<string, string> DatosExtra { get; set; } = new();
}
