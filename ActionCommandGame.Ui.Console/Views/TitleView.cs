using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Settings;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class TitleView: IView
    {
        private readonly AppSettings _appSettings;
        private readonly NavigationManager _navigationManager;

        public TitleView(
            AppSettings appSettings,
            NavigationManager navigationManager)
        {
            _appSettings = appSettings;
            _navigationManager = navigationManager;
        }

        public async Task Show()
        {
            var titleLines = new List<string>
            {
                "Vives Development Studios",
                "presents",
                "",
                _appSettings.GameName,
                "",
                "\"An amazing adventure - 87%\" - any gaming magazine"
            };

            ConsoleBlockWriter.Write(titleLines, 4, ConsoleColor.Blue);

            ConsoleWriter.WriteText("Type \"signin\" to sign in with an existing account or \"register\" to register a new account.");
            var command = Console.ReadLine();

            switch (command)
            {
                case "signin":
                    await _navigationManager.NavigateTo<SignInView>();
                    break;
                case "register":
                    await _navigationManager.NavigateTo<RegisterView>();
                    break;
                default:
                    await _navigationManager.NavigateTo<ExitView>();
                    break;
            }
        }
    }
}
