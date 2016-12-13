using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using MyExpenses.Backend.DataObjects;
using Microsoft.Azure.Mobile.Server;
using System.Web.Http.Controllers;
using MyExpenses.Backend.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.OData;
using System;
using MyExpenses.Backend.Helpers;

namespace MyExpenses.Backend.Controllers
{
    [MobileAppController]
    public class ExpenseReportController : TableController<ExpenseReport>
    {
        MobileServiceContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            context = new MobileServiceContext();
            DomainManager = new EntityDomainManager<ExpenseReport>(context, Request, enableSoftDelete: true);
        }

        // GET tables/ExpenseReport
        [QueryableExpand("Expenses")]
        public IQueryable<ExpenseReport> GetAllExpenseReport()
        {
            return Query();
        }

        // GET tables/ExpenseReport/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [QueryableExpand("Expenses")]
        public SingleResult<ExpenseReport> GetExpenseReport(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/ExpenseReport/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<ExpenseReport> PatchExpenseReport(string id, Delta<ExpenseReport> patch)
        {
            return UpdateAsync(id, patch);
        }
        // POST tables/ExpenseReport
        public async Task<IHttpActionResult> PostExpenseReport(ExpenseReport item)
        {
            ExpenseReport current = await InsertAsync(item);

            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/ExpenseReport/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteExpenseReport(string id)
        {
            return DeleteAsync(id);
        }
    }
}