using System.Data.Common;
using System.Reflection;
using Api.Sync.Core.Application.Api.Interfaces;
using Api.Sync.Core.Application.Common.Models;
using Api.Sync.Core.Application.ContpaqiNominas.Interfaces;
using Api.Sync.Infrastructure.Api;
using Api.Sync.Infrastructure.ContpaqiNominas.Repositories;
using ARSoftware.Contpaqi.Nominas.Sql.Contexts;
using ARSoftware.Contpaqi.Nominas.Sql.Helpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Api.Sync.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());

        serviceCollection.AddContpaqiNominasApiServices();
        serviceCollection.AddContpaqiNominasServices(configuration);

        return serviceCollection;
    }

    private static IServiceCollection AddContpaqiNominasApiServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient<IContpaqiComercialApiService, ContpaqiComercialApiService>((serviceProvider, httpClient) =>
        {
            ApiSyncConfig apiSyncConfig = serviceProvider.GetRequiredService<IOptions<ApiSyncConfig>>().Value;
            ContpaqiNominasConfig contpaqiNominasConfig = serviceProvider.GetRequiredService<IOptions<ContpaqiNominasConfig>>().Value;
            httpClient.BaseAddress = new Uri(apiSyncConfig.BaseAddress);
            httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiSyncConfig.SubscriptionKey);
            httpClient.DefaultRequestHeaders.Add("x-Empresa-Rfc", contpaqiNominasConfig.Empresa.Rfc);
        });

        return serviceCollection;
    }

    private static void AddContpaqiNominasServices(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ContpaqiNominasGeneralesDbContext>(builder =>
        {
            string connectionString =
                ContpaqiNominasSqlConnectionStringFactory.CreateContpaqiNominasGeneralesConnectionString(
                    configuration.GetConnectionString("Contpaqi"));
            DbConnectionStringBuilder cn = new SqlConnectionStringBuilder(connectionString);
            builder.UseSqlServer(connectionString);
        }, ServiceLifetime.Transient, ServiceLifetime.Transient);

        serviceCollection.AddDbContext<ContpaqiNominasEmpresaDbContext>((provider, builder) =>
        {
            ContpaqiNominasConfig config = provider.GetRequiredService<IOptions<ContpaqiNominasConfig>>().Value;

            builder.UseSqlServer(
                ContpaqiNominasSqlConnectionStringFactory.CreateContpaqiNominasEmpresaConnectionString(
                    configuration.GetConnectionString("Contpaqi"), config.Empresa.BaseDatos));
        }, ServiceLifetime.Transient, ServiceLifetime.Transient);

        serviceCollection.AddTransient<IConceptoRepository, ConceptoRepository>();
        serviceCollection.AddTransient<IDepartamentoRepository, DepartamentoRepository>();
        serviceCollection.AddTransient<IEmpleadoRepository, EmpleadoRepository>();
        serviceCollection.AddTransient<IEmpresaRepository, EmpresaRepository>();
        serviceCollection.AddTransient<IMovimientoPDORepository, MovimientoPDORepository>();
        serviceCollection.AddTransient<IPeriodoRepository, PeriodoRepository>();
        serviceCollection.AddTransient<IPuestoRepository, PuestoRepository>();
        serviceCollection.AddTransient<ITipoPeriodoRepository, TipoPeriodoRepository>();
    }
}
