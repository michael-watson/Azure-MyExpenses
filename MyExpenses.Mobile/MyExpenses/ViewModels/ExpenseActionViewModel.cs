﻿using System;

using MyExpenses.Models;

namespace MyExpenses.ViewModels
{
	public class ExpenseActionViewModel : BaseViewModel
	{
		public ExpenseModel Expense { get; set; }

		ReportDetailViewModel parentViewModel;

		public ExpenseActionViewModel(ReportDetailViewModel viewModel)
		{
			parentViewModel = viewModel;
			Expense = new ExpenseModel();
		}

		public ExpenseActionViewModel(bool editable)
		{
			Expense = new ExpenseModel();
			IsEditable = editable;
		}

		public bool IsEditable;

		public string Name
		{
			get { return Expense.Name; }
			set
			{
				if (Expense.Name == value)
					return;
				Expense.Name = value;
				OnPropertyChanged("Name");
			}
		}

		public string ShortDescription
		{
			get { return Expense.ShortDescription; }
			set
			{
				if (Expense.ShortDescription == value)
					return;
				Expense.ShortDescription = value;
				OnPropertyChanged("ShortDescription");
			}
		}

		public string Price
		{
			get
			{
				var dollar = Expense.Price.ToString("C");
				return dollar.Substring(1, dollar.Length - 1);
			}
			set
			{
				//Get Raw number without $ sign and front space
				var rawNumber = value.Remove(0, 2);
				//Get count of the raw number
				var rawCount = rawNumber.Length;

				//Check to ensure a . wasn't the last button touched and not to add additional 0's
				//This must be checked before indefinite loop to make sure it actually is a number
				if (rawNumber.Substring(rawCount - 1, 1) == "." || rawNumber == "0.000")
				{
					OnPropertyChanged("Price");
					return;
				}

				//Get the raw number as a double to check against the price
				var rawDouble = Convert.ToDouble(rawNumber);
				var split = rawNumber.Split(new char[] { '.' }, 2);
				var decimals = split[1];
				var integers = split[0];

				//Check if price equals the raw double to stop indefinite loop
				//Also check if decimals length is 2 because this would be a value curated below
				if (Expense.Price == rawDouble && decimals.Length == 2)
					return;

				//If decimals length is 1, then the backspace button was pressed last
				if (decimals.Length == 1)
				{
					decimals = decimals.Insert(0, integers.Substring(integers.Length - 1, 1));
					integers = integers.Remove(integers.Length - 1, 1);
				}
				else
				{
					integers += decimals.Substring(0, 1);
					decimals = decimals.Remove(0, 1);
				}

				var price = integers + "." + decimals;
				Expense.Price = Convert.ToDouble(price);
				OnPropertyChanged("Price");
			}
		}

		public DateTime Date
		{
			get { return Expense.Date; }
			set
			{
				if (Expense.Date == value)
					return;
				Expense.Date = value;
				OnPropertyChanged("Date");
			}
		}

		double numericPrice
		{
			get
			{
				return Convert.ToDouble(Price);
			}
		}

		public void Save()
		{
			parentViewModel.Report.Expenses.Add(Expense);
		}
	}
}