using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScreechrDemo.Databases.Dtos.Screechr;
using ScreechrDemo.Databases.Utils;

namespace ScreechrDemo.Business.Core.Utils
{
    public class ScreechWrapperResult
    {
        public Result Result { get; set; }
        public Screech Screech { get; set; }
    }
}
