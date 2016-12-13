using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Auth;

using MyExpenses.ViewModels;
using MyExpenses.Services;

namespace MyExpenses
{
	public class AppViewModel : BaseViewModel
	{
		public static string authority = "https://login.windows.net/common";
		public static string ResourceID = "Backend ClientId";//MyExpenses-Backend
		public static string clientId = "Native App ClientId";//MyExpenses native app
		public static string returnUri = "https://{My Azure Site}.azurewebsites.net/.auth/login/done";

		public bool IsLoggedIn;
		public string access_token;

		public MyExpensesAzureService ReportDatabase = new MyExpensesAzureService();
		//public UserInfo User;
		public string UserId;

		IAuthenticator authenticator
		{
			get
			{
				return Xamarin.Forms.DependencyService.Get<IAuthenticator>();
			}
		}

		public AppViewModel()
		{
		}

		public async Task InitiateLoginAsync()
		{
			var account = AccountStore.Create().FindAccountsForService("MyExpenses").FirstOrDefault();
			string accessToken = account?.Properties["access_token"] ?? string.Empty;

			if (string.IsNullOrEmpty(accessToken))
				await LoginAsync();
			else
			{
				string expirationString = account?.Properties["expiration"] ?? DateTime.UtcNow.ToString();
				DateTime expiration = DateTime.Parse(expirationString);

				if (expiration > DateTime.Now)
				{
					//All good and continue
					UserId = account.Properties["userId"];
					IsLoggedIn = true;

					if (ReportDatabase == null)
						ReportDatabase = new MyExpensesAzureService();

					ReportDatabase.AuthenticateClientAsync(accessToken);
				}
				else
					await LoginAsync();
			}
		}

		async Task LoginAsync()
		{
			IsLoggedIn = false;

			var auth = Xamarin.Forms.DependencyService.Get<IAuthenticator>();
			var data = await auth.Authenticate(authority, ResourceID, clientId, returnUri);

			if (data != null)
			{
				var account = new Account
				{
					Username = $"{data.UserInfo.GivenName} {data.UserInfo.FamilyName}"
				};
				account.Properties.Add("access_token", data.AccessToken);
				account.Properties.Add("expiration", data.ExpiresOn.UtcDateTime.ToString());
				account.Properties.Add("userId", data.UserInfo.UniqueId);

				AccountStore.Create().Save(account, "MyExpenses");

				App._token = data.AccessToken;
				//User = data.UserInfo;
				IsLoggedIn = true;

				if (ReportDatabase == null)
					ReportDatabase = new MyExpensesAzureService();

				ReportDatabase.AuthenticateClientAsync(data.AccessToken);
			}
			else
				IsLoggedIn = false;
		}
	}
}