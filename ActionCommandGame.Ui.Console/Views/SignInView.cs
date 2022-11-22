
using System;
using System.Threading.Tasks;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Navigation;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class SignInView: IView
    {
        private readonly ITokenStore _tokenStore;
        private readonly IIdentityApi _identityApi;
        private readonly NavigationManager _navigationManager;

        public SignInView(
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
            ConsoleWriter.WriteTitle("Sign In");

            var token = await GetJwtToken();

            await _tokenStore.SaveTokenAsync(token);

            await _navigationManager.NavigateTo<PlayerSelectionView>();
        }

        private async Task<string> GetJwtToken()
        {
            var request = GetSignInRequest();

            var result = await _identityApi.SignInAsync(request);

            if (!result.Success)
            {
                Console.Clear();
                ConsoleWriter.WriteText("Email/Password combination is wrong.");
                return await GetJwtToken();
            }

            return result.Token;
        }

        private UserSignInRequest GetSignInRequest()
        {
            ConsoleWriter.WriteText("Please type your email: ", ConsoleColor.White, false);
            var email = Console.ReadLine();
            ConsoleWriter.WriteText("Please type your password: ", ConsoleColor.White, false);
            string password = null;
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                    break;
                password += key.KeyChar;
                Console.Write("*");
            }

            return new UserSignInRequest { Email = email, Password = password };
        }
    }
}
