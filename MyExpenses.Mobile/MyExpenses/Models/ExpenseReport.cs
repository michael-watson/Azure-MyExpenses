using System;
using System.Collections.Generic;
using MyExpenses.Constants;
using Newtonsoft.Json;

namespace MyExpenses.Models
{
	public class ExpenseReport
	{
		public ExpenseReport()
		{
			Total = 0;
			Status = StatusConstants.PendingSubmission;
			Expenses = new HashSet<ExpenseModel>();
		}

		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
		[JsonProperty(PropertyName = "reportOwner")]
		public string ReportOwner { get; set; }
		[JsonProperty(PropertyName = "reportName")]
		public string ReportName { get; set; }
		[JsonProperty(PropertyName = "status")]
		public string Status { get; set; }
		[JsonProperty(PropertyName = "approver")]
		public string Approver { get; set; }
		[JsonProperty(PropertyName = "total")]
		public double Total { get; set; }
		[JsonProperty(PropertyName = "expenses")]
		public ICollection<ExpenseModel> Expenses { get; set; }
	}
}