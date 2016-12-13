using System;
using MyExpenses.Pages;
using Xamarin.Forms;

namespace MyExpenses
{
	public partial class App : Application
	{
		public static string _firstName;
		public static string _lastName;
		public static string _token;
		public static DateTimeOffset _expiry;

		public static AppViewModel ViewModel = new AppViewModel();

		public App()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new ReportsPage());
		}

		protected async override void OnStart()
		{
			// Handle when your app starts
			await App.ViewModel.InitiateLoginAsync();
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected async override void OnResume()
		{
			// Handle when your app resumes
			await App.ViewModel.InitiateLoginAsync();
		}
	}
}
