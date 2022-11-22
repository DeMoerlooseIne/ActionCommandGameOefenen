namespace ActionCommandGame.Api.Installers.Abstractions
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection services, IConfiguration configuration);
    }
}
