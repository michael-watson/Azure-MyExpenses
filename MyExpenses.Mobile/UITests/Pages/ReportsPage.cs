using System;

using Xamarin.UITest;

using Expenses.UITests.Enums;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace MyExpenses.UITests.PageObject.Pages
{
	public class ReportsPage : BasePage
	{
		readonly Query NewReportButton, FilterReportsButton, CancelButton, YesDialogButton, NoDialogButton, AddExpense;

		public ReportsPage(IApp app, Platform platform)
			: base(app, platform)
		{
		}

		public void BeginNewReport()
		{
			app.Tap(NewReportButton);
			app.Screenshot("Tapped on add new expense report");
		}

		public void FilterReports(Status status)
		{
			app.Tap(FilterReportsButton);
			app.Screenshot("Tapped on expense report filter");

			switch (status)
			{
				case Status.Approved:
					app.Tap(x => x.Text("Approved"));
					break;
				case Status.PendingApproval:
					app.Tap(x => x.Text("Pending Approval"));
					break;
				case Status.PendingSubmission:
					app.Tap(x => x.Text("Pending Submission"));
					break;
				case Status.All:
					app.Tap(x => x.Text("All"));
					break;
			}

			app.Screenshot($"Filter {status.ToString()} applied");
		}
	}
}