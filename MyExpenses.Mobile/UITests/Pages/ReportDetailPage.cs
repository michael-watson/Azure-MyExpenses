using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace MyExpenses.UITests.PageObject.Pages
{
	public class ReportDetailPage : BasePage
	{
		readonly Query EditNameButton, ReportNameEntry, SaveReportButton, CancelButton, AddExpenseButton;

		public ReportDetailPage(IApp app, Platform platform)
			: base(app, platform)
		{
			EditNameButton = x => x.Marked("editNameButton");
			ReportNameEntry = x => x.Marked("reportNameEntry");
			SaveReportButton = x => x.Marked("saveReportButton");
			CancelButton = x => x.Marked("cancelButton");

			if (OniOS)
			{
				AddExpenseButton = x => x.Class("UINavigationButton");
			}
			else if (OnAndroid)
			{

			}
		}

		/// <summary>
		/// Enters text in Report Name. 
		/// </summary>
		/// <param name="textToEnter">The text to enter into the report name.</param>
		/// /// <param name="deletePreviousText">A value of true will clear the text in Report Name before entering the next text. Default is false. </param>
		public void EnterReportName(string textToEnter, bool deletePreviousText)
		{
			app.Tap(EditNameButton);
			app.Screenshot("Tapped on edit name button");

			if (deletePreviousText)
			{
				var charsToDelete = app.Query(EditNameButton)[0].Text.Length;
				var stringToEnter = "";

				for (var i = 0; i < charsToDelete; i++)
					stringToEnter += "\b";

				app.EnterText(ReportNameEntry, stringToEnter);
			}

			app.EnterText(ReportNameEntry, textToEnter);
			app.DismissKeyboard();
			app.Screenshot("Changed Report Name to: " + textToEnter);
		}

		public void EnterNewReportName(string textToEnter)
		{
			app.Tap(ReportNameEntry);
			app.EnterText(ReportNameEntry, textToEnter);
			app.DismissKeyboard();
			app.Screenshot("Changed Report Name to: " + textToEnter);
		}

		public void PressSaveReportButton()
		{
			app.Tap(SaveReportButton);
			app.Screenshot("Ta[[ed on 'Save Report'");
		}

		public void PressCancelReportButton()
		{
			app.Tap(CancelButton);
			app.Screenshot("Tapped on cancel button");
		}

		public void AddExpenseToReport()
		{
			app.Tap(AddExpenseButton);
			app.Screenshot("Tapped 'Add Expense'");
		}
	}
}