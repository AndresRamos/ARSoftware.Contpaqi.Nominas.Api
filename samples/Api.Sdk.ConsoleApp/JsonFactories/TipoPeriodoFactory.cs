using System.Text.Json;
using Api.Core.Domain.Common;
using Api.Core.Domain.Requests;
using ARSoftware.Contpaqi.Api.Common.Domain;

namespace Api.Sdk.ConsoleApp.JsonFactories;

public static class TipoPeriodoFactory
{
    private static BuscarTiposPeriodoRequest BuscarTodo()
    {
        var request = new BuscarTiposPeriodoRequest(new BuscarTiposPeriodoRequestModel(), new BuscarTiposPeriodoRequestOptions());
        return request;
    }

    public static void CearJson(string directory)
    {
        JsonSerializerOptions options = JsonExtensions.GetJsonSerializerOptions();
        options.WriteIndented = true;

        Directory.CreateDirectory(directory);

        File.WriteAllText(Path.Combine(directory, $"{nameof(BuscarTiposPeriodoRequest)}_Todo.json"),
            JsonSerializer.Serialize<ContpaqiRequest>(BuscarTodo(), options));
    }
}
