using System;

using Xamarin.Forms;
using MyExpenses.Pages;
using MyExpenses.Models;

namespace MyExpenses.Views
{
	public class ExpenseViewCell : ViewCell
	{
		Label nameLabel, shortDescriptionLabel, priceLabel, dateLabel;

		public ExpenseViewCell()
		{
			nameLabel = new Label { Style = (Style)App.Current.Resources["whiteTextLabel"] };
			shortDescriptionLabel = new Label { Style = (Style)App.Current.Resources["whiteTextLabel"], Font = Font.SystemFontOfSize(NamedSize.Small), TextColor = Color.FromRgb(200, 200, 200) };
			priceLabel = new Label { Style = (Style)App.Current.Resources["whiteTextLabel"] };
			dateLabel = new Label { Style = (Style)App.Current.Resources["whiteTextLabel"], Font = Font.SystemFontOfSize(NamedSize.Small), TextColor = Color.FromRgb(200, 200, 200) };

			AbsoluteLayout layout = new AbsoluteLayout { Padding = new Thickness(20, 0, 0, 0) };
			layout.Children.Add(nameLabel, new Rectangle(0.05, 0.05, 0.75, 0.5));
			layout.Children.Add(shortDescriptionLabel, new Rectangle(0.05, 0.95, 0.75, 0.5));
			layout.Children.Add(priceLabel, new Rectangle(1, 0.05, 0.25, 0.5));
			layout.Children.Add(dateLabel, new Rectangle(1, 0.95, 0.25, 0.5));

			AbsoluteLayout.SetLayoutFlags(nameLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutFlags(shortDescriptionLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutFlags(priceLabel, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutFlags(dateLabel, AbsoluteLayoutFlags.All);

			nameLabel.SetBinding(Label.TextProperty, "Name");
			shortDescriptionLabel.SetBinding(Label.TextProperty, "ShortDescription");
			priceLabel.SetBinding(Label.TextProperty, "FormattedPrice");
			dateLabel.SetBinding(Label.TextProperty, "FormattedDate");

			var deleteItem = new MenuItem { Text = "Delete", IsDestructive = true };
			deleteItem.Clicked += OnDelete;

			ContextActions.Add(deleteItem);
			View = layout;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			var expense = BindingContext as ExpenseModel;

			nameLabel.Text = expense?.Name ?? "No Name";
			shortDescriptionLabel.Text = expense?.ShortDescription ?? string.Empty;
			priceLabel.Text = expense?.Price.ToString("C") ?? "$0.00";
			dateLabel.Text = expense?.Date.ToString("d");
		}

		void OnDelete(object sender, EventArgs e)
		{
			var page = this.Parent.Parent.Parent as ReportDetailPage;
			var item = (MenuItem)sender;

			App.Current.MainPage.DisplayAlert("Not Implemented", "This feature isn't implemented yet", "Ok");
		}
	}
}