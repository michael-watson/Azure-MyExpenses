using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using MyExpenses.Models;
using MyExpenses.Constants;

namespace MyExpenses.ViewModels
{
	public class ReportDetailViewModel : BaseViewModel
	{
		public ReportDetailViewModel(ExpenseReport report)
		{
			nameEditImageSource = "ic_mode_edit_white_48pt.png";

			Report = report;

			if (Report.Expenses == null)
				Report.Expenses = new List<ExpenseModel>();
		}

		public ExpenseReport Report { get; set; }

		public string ReportStatus
		{
			get
			{
				switch (Report.Status)
				{
					case StatusConstants.Approved:
						return "Approved";
					case StatusConstants.PendingApproval:
						return "Pending Approval";
					case StatusConstants.PendingSubmission:
						return "Draft";
				}
				return "Draft";
			}
		}

		public string ReportTotal
		{
			get
			{
				return Report.Total.ToString("C");
			}
		}

		string nameEditImageSource;

		public string NameEditImageSource
		{
			get { return nameEditImageSource; }
			set
			{
				if (nameEditImageSource == value)
					return;
				nameEditImageSource = value;
				OnPropertyChanged("NameEditImageSource");
			}
		}

		bool isEditing;

		public bool IsEditing
		{
			get { return isEditing; }
			set
			{
				if (isEditing == value)
					return;
				isEditing = value;
				OnPropertyChanged("IsEditing");
				OnPropertyChanged("NameLabelTextColor");
			}
		}

		public void ToggleEdit(object sender, EventArgs e)
		{
			if (IsEditing)
			{
				IsEditing = false;
				NameEditImageSource = "ic_mode_edit_white_48pt.png";
			}
			else
			{
				IsEditing = true;
				NameEditImageSource = "ic_check_white_48pt.png";
			}
		}

		public void Refresh()
		{
			OnPropertyChanged("Report");
		}

		public async Task SaveAsync()
		{
			Report.ReportOwner = App.ViewModel.UserId;
			Report.Status = StatusConstants.PendingSubmission;

			await App.ViewModel.ReportDatabase.PostExpenseReportAsync(Report);
		}
	}
}