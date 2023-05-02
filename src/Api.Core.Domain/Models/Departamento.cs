namespace Api.Core.Domain.Models;

public sealed class Departamento
{
    public int Numero { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public Dictionary<string, string> DatosExtra { get; set; } = new();
}
