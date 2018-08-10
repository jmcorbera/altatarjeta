using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Credipaz.Comercio.Shared.Models
{
    public class Condition
    {
        public virtual string Concept { get; set; }
        public virtual string Amount { get; set; }
        public virtual string Initial { get; set; }

    }
}
