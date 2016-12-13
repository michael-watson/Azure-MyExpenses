using System.Threading.Tasks;
using System.Collections.ObjectModel;

using MyExpenses.Models;

namespace MyExpenses.ViewModels
{
	public class ReportsPageViewModel : BaseViewModel
	{
		public ReportsPageViewModel()
		{
		}

		ObservableCollection<ExpenseReport> reports;

		public ObservableCollection<ExpenseReport> Reports
		{
			get { return reports; }
			set
			{
				if (reports == value)
					return;
				reports = value;
				OnPropertyChanged("Reports");
			}
		}

		public void FilterData(string filterType)
		{
			//Track a filter
		}

		public async Task RefreshData()
		{
			if (!App.ViewModel.IsLoggedIn)
				return;

			if (App.ViewModel.ReportDatabase == null)
			{
				App.Current.MainPage.DisplayAlert("Syncing", "The table is syncing and will be ready in just a second", "Ok");
				return;
			}

			var source = await App.ViewModel.ReportDatabase.GetAllExpenseReportsForUserAsync(App.ViewModel.UserId);

			Reports = new ObservableCollection<ExpenseReport>(source);
		}
	}
}