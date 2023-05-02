using System.Text.Json;
using Api.Core.Domain.Common;
using Api.Core.Domain.Requests;
using ARSoftware.Contpaqi.Api.Common.Domain;

namespace Api.Sdk.ConsoleApp.JsonFactories;

public sealed class PeriodoFactory
{
    private static BuscarPeriodosRequest BuscarTodo()
    {
        var request = new BuscarPeriodosRequest(new BuscarPeriodosRequestModel(), new BuscarPeriodosRequestOptions());
        request.Model.TipoPeriodo = "Semanal";
        request.Model.Ejercicio = 2023;
        request.Model.Mes = 1;
        return request;
    }

    public static void CearJson(string directory)
    {
        JsonSerializerOptions options = JsonExtensions.GetJsonSerializerOptions();
        options.WriteIndented = true;

        Directory.CreateDirectory(directory);

        File.WriteAllText(Path.Combine(directory, $"{nameof(BuscarPeriodosRequest)}_Todo.json"),
            JsonSerializer.Serialize<ContpaqiRequest>(BuscarTodo(), options));
    }
}
