using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Linq;
using Android.App;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(MyExpenses.Droid.Authenticator_Droid))]

namespace MyExpenses.Droid
{
	public class Authenticator_Droid : IAuthenticator
	{
		public async Task<AuthenticationResult> Authenticate(string authority, string resource, string clientId, string returnUri)
		{
			var authContext = new AuthenticationContext(authority);
			if (authContext.TokenCache.ReadItems().Any())
				authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);

			var uri = new Uri(returnUri);
			var platformParams = new PlatformParameters((Activity)Xamarin.Forms.Forms.Context);
			try
			{
				var authResult = await authContext.AcquireTokenAsync(resource, clientId, uri, platformParams);
				return authResult;
			}
			catch (AdalException e)
			{
				return null;
			}
		}
	}
}
