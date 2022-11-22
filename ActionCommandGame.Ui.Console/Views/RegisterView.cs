using System;
using System.Threading.Tasks;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Stores;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class RegisterView: IView
    {
        private readonly ITokenStore _tokenStore;
        private readonly IIdentityApi _identityApi;
        private readonly NavigationManager _navigationManager;

        public RegisterView(
            ITokenStore tokenStore,
            IIdentityApi identityApi,
            NavigationManager navigationManager)
        {
            _tokenStore = tokenStore;
            _identityApi = identityApi;
            _navigationManager = navigationManager;
        }
        public async Task Show()
        {
            ConsoleBlockWriter.Write("Register");

            var token = await GetJwtToken();

            await _tokenStore.SaveTokenAsync(token);

            await _navigationManager.NavigateTo<PlayerSelectionView>();
        }

        private async Task<string> GetJwtToken()
        {
            var request = GetRegisterRequest();

            var result = await _identityApi.RegisterAsync(request);

            if (!result.Success)
            {
                ConsoleWriter.WriteText("Unable to register.");
                return await GetJwtToken();
            }

            return result.Token;
        }

        private UserRegistrationRequest GetRegisterRequest()
        {
            ConsoleWriter.WriteText("Please type your email: ", ConsoleColor.White, false);
            var email = Console.ReadLine();
            ConsoleWriter.WriteText("Please type your password: ", ConsoleColor.White, false);
            string password = null;
            while (true)
            {
                var key = System.Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                password += key.KeyChar;
                Console.Write("*");
            }

            return new UserRegistrationRequest { Email = email, Password = password };
        }
    }
}
