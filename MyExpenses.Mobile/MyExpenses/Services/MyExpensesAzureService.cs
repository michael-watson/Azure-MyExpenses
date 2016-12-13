using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Newtonsoft.Json.Linq;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

using MyExpenses.Models;
using MyExpenses.Constants;

namespace MyExpenses.Services
{
	public class MyExpensesAzureService
	{
		MobileServiceClient client = new MobileServiceClient("http://myexpenses-backend.azurewebsites.net/");
		IMobileServiceSyncTable<ExpenseReport> expenseReportTable;
		IMobileServiceSyncTable<ExpenseModel> expensesTable;

		public MyExpensesAzureService()
		{
			var url = new Uri("http://myexpenses-backend.azurewebsites.net/");
			var store = new MobileServiceSQLiteStore($"{url.Host}.db");
			store.DefineTable<ExpenseReport>();
			store.DefineTable<ExpenseModel>();
			client.SyncContext.InitializeAsync(store);

			expenseReportTable = client.GetSyncTable<ExpenseReport>();
			expensesTable = client.GetSyncTable<ExpenseModel>();
		}

		public async Task AuthenticateClientAsync(string accessToken)
		{
			var payload = new JObject();
			payload["access_token"] = accessToken;

			await client.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory, payload);
		}

		public async Task<List<ExpenseReport>> GetAllExpenseReportsForUserAsync(string id)
		{
			await SyncAsync();
			//Assume each person has less than 50 reports to display

			return await expenseReportTable.Where(r => r.ReportOwner == id).Take(50).ToListAsync();
		}

		public async Task PostExpenseReportAsync(ExpenseReport report)
		{
			await expenseReportTable.InsertAsync(report);
			await SyncAsync();
		}

		public async Task<List<ExpenseReport>> GetExpenseReportsByStatusForUserAsync(string filterString, string id)
		{
			switch (filterString)
			{
				case StatusConstants.PendingApproval:
					return await expenseReportTable.Where(r => r.Status == StatusConstants.PendingApproval && r.ReportOwner == id).ToListAsync();
				case StatusConstants.Approved:
					return await expenseReportTable.Where(r => r.Status == StatusConstants.Approved && r.ReportOwner == id).ToListAsync();
				case StatusConstants.PendingSubmission:
					return await expenseReportTable.Where(r => r.Status == StatusConstants.PendingSubmission && r.ReportOwner == id).ToListAsync();
			}
			return null;
		}

		public async Task<ExpenseReport> GetExpenseReportById(string Id)
		{
			return await expenseReportTable.LookupAsync(Id);
		}

		public async Task SyncAsync()
		{
			ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

			try
			{
				await this.client.SyncContext.PushAsync().ConfigureAwait(false);

				await this.expenseReportTable.PullAsync(
					"allExpenseReports",
					this.expenseReportTable.CreateQuery()).ConfigureAwait(false);
			}
			catch (MobileServicePushFailedException exc)
			{
				if (exc.PushResult != null)
				{
					syncErrors = exc.PushResult.Errors;
				}
			}

			// Simple error/conflict handling.
			if (syncErrors != null)
			{
				foreach (var error in syncErrors)
				{
					if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
					{
						//Update failed, reverting to server's copy.
						await error.CancelAndUpdateItemAsync(error.Result).ConfigureAwait(false);
					}
					else
					{
						// Discard local change.
						await error.CancelAndDiscardItemAsync().ConfigureAwait(false);
					}

					Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.",
						error.TableName, error.Item["id"]);
				}
			}
		}
	}
}