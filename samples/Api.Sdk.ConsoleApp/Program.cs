using Api.Sdk.ConsoleApp.JsonFactories;

Console.WriteLine("Programa inicio.");

const string baseDirectory = @"C:\AR Software\Contpaqi Nominas API\Requests";

if (Directory.Exists(baseDirectory))
    Directory.Delete(baseDirectory, true);

ConceptosFactory.CearJson(Path.Combine(baseDirectory, "Conceptos"));
DepartamentoFactory.CearJson(Path.Combine(baseDirectory, "Departamentos"));
EmpleadoFactory.CearJson(Path.Combine(baseDirectory, "Empleados"));
EmpresaFactory.CearJson(Path.Combine(baseDirectory, "Empresas"));
MovimientoPDOFactory.CearJson(Path.Combine(baseDirectory, "MovimientosPDO"));
PuestoFactory.CearJson(Path.Combine(baseDirectory, "Puestos"));
TipoPeriodoFactory.CearJson(Path.Combine(baseDirectory, "TiposPeriodos"));
PeriodoFactory.CearJson(Path.Combine(baseDirectory, "Periodos"));

Console.WriteLine("Programa fin.");
