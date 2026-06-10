using GPrev.Core.Dominio.Infra;
using GPrev.Core.Dominio.Services;
using GPrev.Core.Infra;
using GPrev.InssData;
using GPrev.SelicData;
using Microsoft.Extensions.DependencyInjection;

namespace GPrev.Core.Extensions;

public static class InjecaoDependencias
{
    public static IServiceCollection AddGPrevCore(this IServiceCollection services)
    {
        services.AddSingleton<InssDataService>();
        services.AddSingleton<SelicDataService>();
        services.AddSingleton<LeitorCnisAbstrato, LeitorCnisPdfPig>();
        services.AddSingleton<InssServico>();
        services.AddSingleton<CnisServico>();
        return services;
    }
}