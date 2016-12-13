using System;

using Newtonsoft.Json;

namespace MyExpenses.Models
{
	public class ExpenseModel
	{
		public ExpenseModel()
		{
			Date = DateTime.Now;
		}
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
		[JsonProperty(PropertyName = "expenseReportId")]
		public string ExpenseReportId { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }
		[JsonProperty(PropertyName = "price")]
		public double Price { get; set; }
		[JsonProperty(PropertyName = "shortDescription")]
		public string ShortDescription { get; set; }
		[JsonProperty(PropertyName = "date")]
		public DateTime Date { get; set; }
		[JsonProperty(PropertyName = "receiptStreet")]
		public string ReceiptStreet { get; set; }
		[JsonProperty(PropertyName = "receiptCity")]
		public string ReceiptCity { get; set; }
		[JsonProperty(PropertyName = "receiptState")]
		public string ReceiptState { get; set; }
		[JsonProperty(PropertyName = "receiptZip")]
		public string ReceiptZip { get; set; }
	}
}