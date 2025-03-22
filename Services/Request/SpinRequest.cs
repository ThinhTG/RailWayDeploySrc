using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Request
{
    public class SpinRequest
    {
        public string AccountId { get; set; }
        public Guid PackageId { get; set; }
    }
}
