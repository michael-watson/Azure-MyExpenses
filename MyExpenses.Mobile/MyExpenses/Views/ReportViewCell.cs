using System;

using Xamarin.Forms;

using MyExpenses.Pages;
using MyExpenses.Models;

namespace MyExpenses.Views
{
	public class ReportViewCell : ViewCell
	{
		Label nameLabel, statusLabel, totalPriceLabel;

		public ReportViewCell()
		{
			nameLabel = new Label { Style = (Style)Application.Current.Resources["whiteTextLabel"] };
			statusLabel = new Label { Style = (Style)Application.Current.Resources["whiteTextLabel"], Font = Font.SystemFontOfSize(NamedSize.Small), TextColor = Color.FromRgb(200, 200, 200) };
			totalPriceLabel = new Label { Style = (Style)Application.Current.Resources["whiteTextLabel"] };

			AbsoluteLayout layout = new AbsoluteLayout { Padding = new Thickness(20, 5, 0, 0), HeightRequest = 50 };
			layout.Children.Add(nameLabel, new Rectangle(0.05, 0.05, 0.8, 0.5));
			layout.Children.Add(statusLabel, new Rectangle(0.05, 0.95, 0.55, 0.5));
			layout.Children.Add(totalPriceLabel, new Rectangle(1, 0.05, 0.25, 0.6));

			AbsoluteLayout.SetLayoutFlags(nameLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutFlags(statusLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutFlags(totalPriceLabel, AbsoluteLayoutFlags.All);

			var deleteItem = new MenuItem { Text = "Delete", IsDestructive = true };
			deleteItem.Clicked += OnDelete;

			ContextActions.Add(deleteItem);
			View = layout;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			var report = BindingContext as ExpenseReport;

			nameLabel.Text = report?.ReportName ?? "Name not set";
			statusLabel.Text = report?.Status ?? "Draft";
			totalPriceLabel.Text = report.Total.ToString("C");
		}

		void OnDelete(object sender, EventArgs e)
		{
			var page = this.Parent.Parent as ReportsPage;
			var item = (MenuItem)sender;

			App.Current.MainPage.DisplayAlert("Not Implemented", "This feature isn't implemented yet", "Ok");
		}
	}
}