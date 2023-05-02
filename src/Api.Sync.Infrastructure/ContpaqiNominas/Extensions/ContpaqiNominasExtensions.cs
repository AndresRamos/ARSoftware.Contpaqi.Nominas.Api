using System.ComponentModel;

namespace Api.Sync.Infrastructure.ContpaqiNominas.Extensions;

public static class ContpaqiNominasExtensions
{
    public static Dictionary<string, string> ToDatosDictionary<TModel>(this object model)
    {
        var datosDictionary = new Dictionary<string, string>();

        Type sqlModelType = typeof(TModel);

        foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(model))
        {
            if (!sqlModelType.HasProperty(propertyDescriptor.Name) || propertyDescriptor.Name == "CTIMESTAMP")
                continue;

            if (propertyDescriptor.GetValue(model)?.ToString() is null)
                continue;

            datosDictionary.Add(propertyDescriptor.Name, propertyDescriptor.GetValue(model)?.ToString() ?? string.Empty);
        }

        return datosDictionary;
    }

    public static bool HasProperty(this Type obj, string propertyName)
    {
        return obj.GetProperty(propertyName) != null;
    }
}
