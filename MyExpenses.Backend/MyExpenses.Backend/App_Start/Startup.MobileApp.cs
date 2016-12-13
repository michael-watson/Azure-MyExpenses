using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using MyExpenses.Backend.DataObjects;
using MyExpenses.Backend.Models;
using Owin;
using System.Web.Http.Description;
using System.Data.Entity.Migrations;
using System.Configuration;

namespace MyExpenses.Backend
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            new MobileAppConfiguration()
                .MapApiControllers()
                .AddTables(
                    new Microsoft.Azure.Mobile.Server.Tables.Config.MobileAppTableConfiguration()
                    .MapTableControllers()
                    .AddEntityFramework()
                )
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new MobileServiceInitializer());

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    // This middleware is intended to be used locally for debugging. By default, HostName will
                    // only have a value when running in an App Service application.
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }

            app.UseWebApi(config);

            var migrator = new DbMigrator(new MyExpenses.Backend.Migrations.Configuration());
            migrator.Update();
        }
    }

    public class MobileServiceInitializer : CreateDatabaseIfNotExists<MobileServiceContext>
    {
        protected override void Seed(MobileServiceContext context)
        {
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

            context.SaveChanges();

            base.Seed(context);
        }
    }
}