using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreechrDemo.Contracts.Model
{
    public class PageModel
    {
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        public string SortDir { get; set; }
        public ulong? UserId { get; set; }
    }
}
