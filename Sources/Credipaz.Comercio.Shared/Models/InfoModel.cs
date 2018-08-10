using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Credipaz.Comercio.Shared.Models
{
    public class InfoModel
    {
        public virtual int IdCommerce { get; set; }
        public virtual string CUIT { get; set; }
        public virtual string Description { get; set; }
        public virtual string Address { get; set; }
        public virtual string LocationCode { get; set; }
        public virtual string Location { get; set; }
        public virtual string Province { get; set; }
        public virtual string ProvinceCode { get; set; }
        public virtual int IdSubsidiary { get; set; }
        public virtual string Subsidiary { get; set; }
        public virtual string Company { get; set; }
        public virtual string CompanyFullName { get; set; }

    }

}
