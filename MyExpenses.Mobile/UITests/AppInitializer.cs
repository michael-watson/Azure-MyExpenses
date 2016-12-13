using System;
using System.IO;
using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace MyExpenses.UITests
{
	public class AppInitializer
	{
		public static IApp StartApp(Platform platform)
		{
			if (platform == Platform.Android)
			{
				return ConfigureApp
					.Android
					.ApkFile("../../../Droid/bin/Release/com.michaelwatson.myexpenses-Signed.apk")
					.StartApp(Xamarin.UITest.Configuration.AppDataMode.Clear);
			}

			return ConfigureApp
				.iOS
				.AppBundle("../../../iOS/bin/iPhoneSimulator/Debug/MyExpenses.iOS.app")
				//.InstalledApp ("com.michaelwatson.myexpenses")
				.EnableLocalScreenshots()
				.StartApp();
		}
	}
}