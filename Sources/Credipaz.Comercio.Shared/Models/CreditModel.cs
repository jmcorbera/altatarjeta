using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Credipaz.Comercio.Shared.Models
{

    public class CreditModel
    {
        public int? IdLog  { get; set; }
        public string Date { get; set; } //check
        public string Name { get; set; }
        public string DniType { get; set; }
        public int? DniNumber { get; set; }
        public decimal Amount { get; set; }
        public int? Quotas { get; set; }
        public string Status { get; set; }

        //Credit Increase

        public string Sex { get; set; }
        public string Ocupation { get; set; }
        public string MonthlyIncome{ get; set; }
        public string RequestedAmount { get; set; }
    }
}
