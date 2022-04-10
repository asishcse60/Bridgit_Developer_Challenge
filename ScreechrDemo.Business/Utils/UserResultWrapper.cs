using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScreechrDemo.Databases.Dtos.User;
using ScreechrDemo.Databases.Utils;

namespace ScreechrDemo.Business.Core.Utils
{
    public class UserResultWrapper
    {
        public Result ResultStatus { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
