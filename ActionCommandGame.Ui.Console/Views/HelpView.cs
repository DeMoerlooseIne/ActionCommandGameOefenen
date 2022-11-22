using System;
using System.Threading.Tasks;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Settings;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class HelpView: IView
    {
        private readonly AppSettings _appSettings;

        public HelpView(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public Task Show()
        {
            ConsoleBlockWriter.Write("Help");

            ConsoleWriter.WriteText($"\t{_appSettings.ActionCommand}: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Do something");

            ConsoleWriter.WriteText("\tshop: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("See the shop items");

            ConsoleWriter.WriteText("\tbuy 1: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Buy item number 1 from the shop");

            ConsoleWriter.WriteText("\tinventory: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Shows your inventory");

            ConsoleWriter.WriteText("\tstats: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("See your statistics");

            ConsoleWriter.WriteText("\tleaderboard: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("See the leaderboard");

            ConsoleWriter.WriteText("\tchange: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Change player");

            ConsoleWriter.WriteText("\tquit: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Quit the game");

            ConsoleWriter.WriteText("\thelp: ", ConsoleColor.White, false);
            ConsoleWriter.WriteText("Well, this one is self explanatory, isn't it? Because you just used it?");

            return Task.CompletedTask;
        }
    }
}
