using System;

using Xamarin.UITest;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;
using NUnit.Framework;

namespace MyExpenses.UITests.PageObject.Pages
{
	public class ExpenseActionPage : BasePage
	{
		readonly Query AddReceiptButton, ChooseFromGalleryButton, ChooseFromCameraRollButton, ExpenseDatePicker, NameEntry, PriceEntry, ShortDescriptionEntry, SaveExpenseButton, CancelExpenseButton, DeleteExpenseButton;

		public ExpenseActionPage(IApp app, Platform platform)
			: base(app, platform)
		{

			AddReceiptButton = x => x.Marked("addReceiptButton");
			ChooseFromGalleryButton = x => x.Marked("Choose From Gallery");
			ChooseFromCameraRollButton = x => x.Marked("Camera Roll");
			ExpenseDatePicker = x => x.Marked("expenseDatePicker");
			PriceEntry = x => x.Marked("priceEntry");
			ShortDescriptionEntry = x => x.Marked("");
			SaveExpenseButton = x => x.Marked("saveExpenseButton");
			CancelExpenseButton = x => x.Marked("cancelExpenseButton");
			DeleteExpenseButton = x => x.Marked("expenseDeleteButton");

			if (OniOS)
			{
			}
			else if (OnAndroid)
			{
			}
		}

		public void VerifyOnPage()
		{
			app.WaitForElement(NameEntry, "Timed out waiting for the page to appear", TimeSpan.FromSeconds(10));
		}

		public void PickReceiptForExpense()
		{
			//TODO: Need to figure out solution for Android, currently only works for iOS because we don't have class of Android photos
			app.Tap(AddReceiptButton);
			app.Screenshot("Tapped the 'Add Receipt' Button");
			app.Tap(ChooseFromGalleryButton);
			app.Screenshot("Choose photos from the gallery");
			app.Tap(ChooseFromCameraRollButton);
			app.Screenshot("Choose a photo from my camera roll");

			if (OniOS)
			{
				var pics = app.Query(x => x.Class("PUPhotoView"));
				var rand = new Random().Next(1, pics.Length);

				app.WaitForElement(x => x.Class("PUPhotoView").Index(rand));
				app.Tap(x => x.Class("PUPhotoView").Index(rand));
				app.Screenshot($"Selected image {rand}");
			}
			else if (OnAndroid)
			{
				throw new Exception("Class for Android image viewer hasn't been incorprated into test suite yet");
			}
		}

		public void ChangeExpenseDate(DateTime date)
		{
			app.Tap(ExpenseDatePicker);
			app.Screenshot("Tapped on the expense date picker to set the date");

			if (app is Xamarin.UITest.iOS.iOSApp)
			{
				app.Query(x => x.Class("UIPickerView").Invoke("selectRow", date.Month - 1, "inComponent", 0, "animated", true));
				app.Query(x => x.Class("UIPickerView").Invoke("selectRow", date.Day - 1, "inComponent", 1, "animated", true));
				app.Query(x => x.Class("UIPickerView").Invoke("selectRow", date.Year - 1, "inComponent", 2, "animated", true));
			}
			else {
				app.Query(x => x.Id("datePicker").Invoke("updateDate", date.Year, date.Month, date.Day));
			}

			app.WaitForElement(x => x.Text("Done"), "Timed out waiting for the done button to appear");
			app.Tap(x => x.Text("Done"));
			app.Screenshot("Done entering the expense date");
		}

		public void ChangeExpensePrice(double price)
		{
			var currentPriceText = app.Query(PriceEntry)[0].Text;
			var clearString = "";

			for (var i = 0; i < currentPriceText.Length; i++)
				clearString += "\b";

			if (currentPriceText != "$ 0.00")
				app.EnterText(PriceEntry, clearString);
			app.EnterText(PriceEntry, price.ToString());
			app.DismissKeyboard();
			app.Screenshot("Changed Price to: " + price.ToString("C"));
		}

		public void EnterShortDescription(string shortDescription)
		{
			app.EnterText(ShortDescriptionEntry, shortDescription);
			app.Screenshot("Changed short description to: " + shortDescription);
		}

		public void EnterExpenseName(string name)
		{
			app.EnterText(NameEntry, name);
			app.Screenshot("Changed report name to: " + name);
		}

		public void PressSaveExpenseButton()
		{
			app.Tap(SaveExpenseButton);
			app.Screenshot("Tapped 'Save' Button");
		}

		public void PressCancelExpenseButton()
		{
			app.Tap(CancelExpenseButton);
			app.Screenshot("Tapped 'Cancel' Button");
		}

		public void PressDeleteExpenseButton()
		{
			app.Tap(DeleteExpenseButton);
			app.Screenshot("Tapped 'Delete' Button");
		}
	}
}