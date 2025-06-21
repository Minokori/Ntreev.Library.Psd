using Microsoft.Extensions.DependencyInjection;
using Ntreev.Library.Psd.Services;

namespace Ntreev.Library.Psd;
public static class PsdService
    {

    public static ServiceProvider Services { get; } = GetServices();

    public static PsdUriResolver Resolver => Services.GetRequiredService<PsdUriResolver>();

    private static ServiceProvider GetServices()
        {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<PsdUriResolver, PathResolver>()
            .BuildServiceProvider();

        return serviceProvider;

        }
    }
