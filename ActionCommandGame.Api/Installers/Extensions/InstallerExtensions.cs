using ActionCommandGame.Api.Installers.Abstractions;

namespace ActionCommandGame.Api.Installers.Extensions
{
    public static class InstallerExtensions
    {
        public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(Program).Assembly.ExportedTypes
                .Where(t => typeof(IInstaller).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();

            installers.ForEach(installer => installer.InstallServices(services, configuration));
        }
    }
}
