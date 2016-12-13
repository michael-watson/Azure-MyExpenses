using System;

using Xamarin.Forms;

using MyExpenses.Views;
using MyExpenses.Models;
using MyExpenses.ViewModels;
using MyExpenses.Constants;

namespace MyExpenses.Pages
{
	public class ReportDetailPage : BasePage
	{
		public ReportDetailViewModel ViewModel;
		RelativeLayout dashboard;
		Image editNameImage;
		ExpenseListView expenseList;
		ToolbarItem addExpense;
		StackLayout layout;

		Label reportTotal, status, listHeader;
		Entry reportName;
		Button submitButton, deleteButton;

		public ReportDetailPage(ExpenseReport report)
		{
			NavigationPage.SetTitleIcon(this, "icon.png");
			ViewModel = new ReportDetailViewModel(report);
			BindingContext = ViewModel;

			AddConditionalUI();

			Content = layout;

			if (report.Status == "PendingApproval")
				submitButton.Text = "Unsubmit Report";

			#region Set Event Handlers and Bindings
			if (submitButton != null)
				submitButton.Clicked += HandleSubmitReport;
			if (deleteButton != null)
				deleteButton.Clicked += HandleDeleteReport;
			if (addExpense != null)
				addExpense.Clicked += HandleAddExpense;

			TapGestureRecognizer tap = new TapGestureRecognizer();
			editNameImage.GestureRecognizers.Add(tap);
			tap.Tapped += ViewModel.ToggleEdit;

			expenseList.ItemSelected += HandleExpenseSelected;

			expenseList.SetBinding<ReportDetailViewModel>(ListView.ItemsSourceProperty, vm => vm.Report.Expenses);
			reportName.SetBinding<ReportDetailViewModel>(Entry.TextProperty, vm => vm.Report.ReportName);
			reportName.SetBinding<ReportDetailViewModel>(Entry.IsEnabledProperty, vm => vm.IsEditing);
			reportTotal.SetBinding<ReportDetailViewModel>(Label.TextProperty, vm => vm.Report.Total.ToString("C"));
			status.SetBinding<ReportDetailViewModel>(Label.TextProperty, vm => vm.ReportStatus);
			editNameImage.SetBinding<ReportDetailViewModel>(Image.SourceProperty, vm => vm.NameEditImageSource);
			#endregion
		}

		#region Create UI

		public override void ConstructUI()
		{
			base.ConstructUI();

			dashboard = new RelativeLayout();
			reportName = new Entry
			{
				AutomationId = "reportNameEntry",
				Style = (Style)App.Current.Resources["underlinedEntry"],
				FontSize = Device.OnPlatform(20, 14, 14),
			};

			reportTotal = new Label { Style = (Style)App.Current.Resources["whiteTextLabel"] };
			status = new Label { Style = (Style)App.Current.Resources["whiteTextLabel"] };

			submitButton = new Button
			{
				Style = (Style)App.Current.Resources["borderedButton"],
				AutomationId = "submitReportButton",
				Text = "Submit Report",
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			deleteButton = new Button
			{
				Style = (Style)App.Current.Resources["borderedButton"],
				AutomationId = "deleteReportButton",
				Text = "Delete Report",
				HorizontalOptions = LayoutOptions.CenterAndExpand
			};
			editNameImage = new Image { AutomationId = "editNameButton", Aspect = Aspect.AspectFill };

			expenseList = new ExpenseListView { AutomationId = "expenseListView" };

			listHeader = new Label { Style = (Style)App.Current.Resources["whiteTextLabel"] };
			expenseList.Header = new ContentView
			{
				Content = listHeader,
				Padding = new Thickness(10, 0, 0, 0)
			};

			addExpense = new ToolbarItem { AutomationId = "addExpenseButton", Text = "Add Expense" };

			layout = new StackLayout
			{
				Padding = new Thickness(0, 0, 0, 10),
			};
		}

		public override void AddChildrenToParentLayout()
		{
			base.AddChildrenToParentLayout();

			Func<RelativeLayout, double> getReportTotalWidth = (p) => reportTotal.GetSizeRequest(dashboard.Width, dashboard.Height).Request.Width;
			Func<RelativeLayout, double> getStatusWidth = (p) => status.GetSizeRequest(dashboard.Width, dashboard.Height).Request.Width;

			dashboard.Children.Add(
				reportName,
				xConstraint: Constraint.Constant(10),
				yConstraint: Constraint.Constant(10),
				widthConstraint: Constraint.RelativeToParent(p => p.Width * 0.6 - 10)
			);
			dashboard.Children.Add(
				editNameImage,
				xConstraint: Constraint.RelativeToView(reportName, (p, v) => v.Width + 10 - v.Height * 0.6),
				yConstraint: Constraint.RelativeToView(reportName, (p, v) => v.Height * 0.2 + 10),
				widthConstraint: Constraint.RelativeToView(reportName, (p, v) => v.Height * 0.6),
				heightConstraint: Constraint.RelativeToView(reportName, (p, v) => v.Height * 0.6)
			);
			dashboard.Children.Add(
				reportTotal,
				xConstraint: Constraint.RelativeToParent(p => p.Width - getReportTotalWidth(p) - 10),
				yConstraint: Constraint.Constant(10)
			);
			dashboard.Children.Add(
				status,
				xConstraint: Constraint.RelativeToParent(p => p.Width - getStatusWidth(p) - 10),
				yConstraint: Constraint.RelativeToView(reportTotal, (p, v) => v.Y + v.Height + 5)
			);

			layout.Children.Add(dashboard);
			layout.Children.Add(expenseList);
		}

		public override void AddConditionalUI()
		{
			base.AddConditionalUI();

			if (ViewModel.Report.Status == StatusConstants.PendingSubmission)
				ToolbarItems.Add(addExpense);

			if (ViewModel.Report.Status != StatusConstants.Approved)
			{
				layout.Children.Add(submitButton);
				layout.Children.Add(deleteButton);
			}
			else
			{
				submitButton = null;
				deleteButton = null;
			}
		}

		#endregion

		#region Event Handlers

		void HandleExpenseSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var expense = e.SelectedItem as ExpenseModel;
			if (expense == null)
				return;

			var editable = ViewModel.Report.Status == StatusConstants.PendingSubmission;
			Navigation.PushModalAsync(new ExpenseActionPage(expense, editable));
			expenseList.SelectedItem = null;
		}

		void HandleSubmitReport(object sender, EventArgs e)
		{
			App.Current.MainPage.DisplayAlert("Not Implemented", "This feature isn't implemented yet", "Ok");
		}

		async void HandleDeleteReport(object sender, EventArgs e)
		{
			var delete = await DisplayAlert("Confirm", "Are you sure you want to delete this report? This can't be undone.", "Yes", "No");
			if (delete)
			{
				Navigation.PopAsync();
			}
		}

		void HandleAddExpense(object sender, EventArgs e)
		{
			Navigation.PushModalAsync(new ExpenseActionPage(ViewModel));
		}

		#endregion

		protected override void OnAppearing()
		{
			base.OnAppearing();

			//ViewModel.GetReportExpenses();
		}

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			//TODO: INSIGHTS FIX
			if (submitButton != null)
				submitButton.WidthRequest = width - 20;
			if (deleteButton != null)
				deleteButton.WidthRequest = width - 20;

			base.LayoutChildren(x, y, width, height);
		}
	}
}