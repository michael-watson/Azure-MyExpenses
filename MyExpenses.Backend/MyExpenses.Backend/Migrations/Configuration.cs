namespace MyExpenses.Backend.Migrations
{
    using DataObjects;
    using Microsoft.Azure.Mobile.Server.Tables;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyExpenses.Backend.Models.MobileServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator());
        }
        
        protected override void Seed(MyExpenses.Backend.Models.MobileServiceContext context)
        {
            var itemId = Guid.NewGuid().ToString();

            var expense = new ExpenseModel
            {
                Name = "Flight",
            };

            var expenseReport = new ExpenseReport
            {
                Approver = "Ian Leathurbyury",
                ReportName = "FY17 LATAM trip",
                ReportOwner = "Michael Watson",
                Status = "InProgress",
            };

            expenseReport.Expenses.Add(expense);

            context.ExpenseReports.Add(expenseReport);

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}
