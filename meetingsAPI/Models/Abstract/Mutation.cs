using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace meetingsAPI.Models.Abstract
{
    public class Mutation<T>
    {
        public T Entity { get; set; }
        public List<string> Fields { get; set; }
    }
}
