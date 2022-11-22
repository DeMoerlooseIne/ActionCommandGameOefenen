using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using ActionCommandGame.Sdk;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Settings;
using ActionCommandGame.Ui.ConsoleApp.Stores;
using ActionCommandGame.Ui.ConsoleApp.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ActionCommandGame.Ui.ConsoleApp
{
    class Program
    {
        private static IServiceProvider ServiceProvider { get; set; }
        private static IConfiguration Configuration { get; set; }

        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            
            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var navigationManager = ServiceProvider.GetRequiredService<NavigationManager>();

            Console.OutputEncoding = Encoding.UTF8;
            
            await navigationManager.NavigateTo<TitleView>();

            //Console.Clear();
            //await playerSelectionView.Show();

            //Console.Clear();
            //await gameView.Show();
        }

        public static void ConfigureServices(IServiceCollection services)

        {
            var appSettings = new AppSettings();
            Configuration.Bind(nameof(AppSettings), appSettings);
            services.AddSingleton(appSettings);

            services.AddSingleton<MemoryStore>();
            services.AddSingleton<ITokenStore, TokenStore>();

            //Register Navigation
            services.AddTransient<NavigationManager>();

            //Register the Views
            services.AddTransient<ExitView>();
            services.AddTransient<GameView>();
            services.AddTransient<HelpView>();
            services.AddTransient<InventoryView>();
            services.AddTransient<LeaderboardView>();
            services.AddTransient<PlayerSelectionView>();
            services.AddTransient<RegisterView>();
            services.AddTransient<ShopView>();
            services.AddTransient<SignInView>();
            services.AddTransient<TitleView>();

            //Register the Sdk api classes
            services.AddApi(appSettings.ApiBaseUrl);
        }
    }
}
