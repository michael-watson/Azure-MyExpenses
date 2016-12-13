using System;

using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace MyExpenses.UITests.PageObject.Pages
{
	public class NewReportPage : BasePage
	{
		//These variables will be identified separately per platform into the contructor above
		readonly Query ReportName, SaveReport, CancelButton, YesDialogButton, NoDialogButton, AddExpense;

		public NewReportPage(IApp app, Platform platform)
			: base(app, platform)
		{
			ReportName = x => x.Marked("reportNameEntry");
			SaveReport = x => x.Marked("saveReportButton");
			CancelButton = x => x.Marked("cancelButton");
			YesDialogButton = x => x.Marked("Yes");
			NoDialogButton = x => x.Marked("No");
			AddExpense = x => x.Marked("addExpense");

			if (OnAndroid)
			{
			}
			else if (OniOS)
			{
			}
		}

		public void VerifyOnPage()
		{
			app.WaitForElement(ReportName, "Timed out waiting for the page to appear", TimeSpan.FromSeconds(10));
		}

		public void EnterReportName(string reportName)
		{
			app.EnterText(ReportName, reportName);
			app.DismissKeyboard();
			app.Screenshot("Changed Report Name to: " + reportName);
		}

		public void PressSaveReportButton()
		{
			app.Tap(SaveReport);
			app.Screenshot("Tapped on 'Save Report' button");
		}

		public void InitiateReportCancel(bool shouldCancelReport)
		{
			app.Tap(CancelButton);
			app.WaitForElement(YesDialogButton, "Timed out waiting for the dialog to appear", TimeSpan.FromSeconds(3));
			app.Screenshot("Tapped on 'Cancel' button to cancel report creation.");

			if (shouldCancelReport)
			{
				app.Tap(YesDialogButton);
				app.Screenshot("Report was cancelled");
			}
			else
			{
				app.Tap(NoDialogButton);
				app.Screenshot("Report was not cancelled");
			}
		}

		public void AddExpenseToReport()
		{
			app.Tap(AddExpense);
			app.Screenshot("Tapped 'Add Expense' button");
		}
	}
}