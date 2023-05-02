using System.Text.Json;
using Api.Core.Domain.Common;
using Api.Core.Domain.Models;
using Api.Core.Domain.Requests;
using ARSoftware.Contpaqi.Api.Common.Domain;

// ReSharper disable InconsistentNaming

namespace Api.Sdk.ConsoleApp.JsonFactories;

public sealed class MovimientoPDOFactory
{
    private static CrearMovimientoPDORequest Crear()
    {
        var request = new CrearMovimientoPDORequest(new CrearMovimientoPDORequestModel(), new CrearMovimientoPDORequestOptions());

        request.Model.EmpleadoCodigo = "001";
        request.Model.ConceptoNumero = 70; // Deduccion general
        request.Model.Periodo = new LlavePeriodo { TipoPeriodo = "Semanal", Numero = 1, Ejercicio = 2023, Mes = 1 };
        request.Model.Importe = 100;
        request.Options.Sobreescribir = true;
        return request;
    }

    public static void CearJson(string directory)
    {
        JsonSerializerOptions options = JsonExtensions.GetJsonSerializerOptions();
        options.WriteIndented = true;

        Directory.CreateDirectory(directory);

        File.WriteAllText(Path.Combine(directory, $"{nameof(CrearMovimientoPDORequest)}.json"),
            JsonSerializer.Serialize<ContpaqiRequest>(Crear(), options));
    }
}
