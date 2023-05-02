namespace Api.Core.Domain.Models;

public sealed class Empleado
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombres { get; set; } = string.Empty;
    public string ApellidoPaterno { get; set; } = string.Empty;
    public string ApellidoMaterno { get; set; } = string.Empty;
    public string NombreLargo { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string Rfc { get; set; } = string.Empty;
    public string Curp { get; set; } = string.Empty;
    public Departamento? Departamento { get; set; }
    public Puesto? Puesto { get; set; }
    public Dictionary<string, string> DatosExtra { get; set; } = new();
}
