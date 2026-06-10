using GPrev.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GPrev.Forms;

static class Program
{
    /// <summary>
    /// Ponto de entrada principal da aplicação
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        
        // Configurar container de DI
        var services = new ServiceCollection();
        ConfigurarServicos(services);
        
        using var serviceProvider = services.BuildServiceProvider();
        var mainForm = serviceProvider.GetRequiredService<Form1>();
        
        Application.Run(mainForm);
    }

    private static void ConfigurarServicos(IServiceCollection services)
    {
        // Adicionar serviços do Core
        services.AddGPrevCore();
        
        // Registrar o formulário principal
        services.AddTransient<Form1>();
    }
}