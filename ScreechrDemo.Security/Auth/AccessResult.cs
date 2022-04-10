using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreechrDemo.Security.Auth
{
    public class AccessResult
    {
        public bool IsAuthenticated { get; init; }
        public string UserName { get; init; }   
        public ulong Id { get; init; }
    }
}
