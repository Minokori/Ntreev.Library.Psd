using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Ntreev.Library.Psd.Services;

namespace Ntreev.Library.Psd;
public static class PsdService
    {

    public static ServiceProvider Services { get; } = GetServices();

    public static IDocumentManager Resolver => Services.GetRequiredService<IDocumentManager>();

    private static ServiceProvider GetServices()
        {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IDocumentManager, PsdDocumentManager>()
            .BuildServiceProvider();

        return serviceProvider;

        }
    }
