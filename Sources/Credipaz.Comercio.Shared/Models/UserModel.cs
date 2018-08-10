using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Credipaz.Comercio.Shared.Models
{
    public class UserModel
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }

    }
}
