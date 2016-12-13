using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyExpenses.Backend.DataObjects
{
    public class ExpenseReport : EntityData
    {
        public ExpenseReport()
        {
            Expenses = new HashSet<ExpenseModel>();
        }
        
        public string ReportOwner { get; set; }
        public string ReportName { get; set; }
        public string Status { get; set; }
        public string Approver { get; set; }
        public double Total { get; set; }
        //Navigation Properties
        public virtual ICollection<ExpenseModel> Expenses { get; set; }
    }
}