using Microsoft.Azure.Mobile.Server;
using System;
using System.ComponentModel.DataAnnotations;

namespace MyExpenses.Backend.DataObjects
{
    public class ExpenseModel : EntityData
    {
        public ExpenseModel()
        {
            Date = DateTime.Now;
        }
        //Primary Key
        //public string ExpenseId { get; set; }
        //Foreign Key
        public string ExpenseReportId { get; set; }


        public string Name { get; set; }
        public double Price { get; set; }
        public string ShortDescription { get; set; }
        public DateTime Date { get; set; }
        public string ReceiptStreet { get; set; }
        public string ReceiptCity { get; set; }
        public string ReceiptState { get; set; }
        public string ReceiptZip { get; set; }

        //Navigation Properties
        //public virtual ExpenseReceipt Receipt { get; set; }
    }
}