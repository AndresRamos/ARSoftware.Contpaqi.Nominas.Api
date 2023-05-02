namespace Api.Core.Domain.Models;

public sealed class Puesto
{
    public int Numero { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public Dictionary<string, string> DatosExtra { get; set; } = new();
}
