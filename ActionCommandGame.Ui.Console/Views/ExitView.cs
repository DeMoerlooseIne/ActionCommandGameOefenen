using System;
using System.Threading.Tasks;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class ExitView: IView
    {
        public Task Show()
        {
            ConsoleBlockWriter.Write("Thank you for playing.", 4, ConsoleColor.Yellow);
            Console.ReadLine();

            return Task.CompletedTask;
        }
    }
}
