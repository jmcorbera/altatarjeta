using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Credipaz.Comercio.Shared.Models
{
    public class TaxModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Int16 Quota { get; set; }
        public decimal Coefficient { get; set; }
    }
}
