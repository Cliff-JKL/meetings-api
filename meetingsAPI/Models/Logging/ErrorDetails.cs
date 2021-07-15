using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace meetingsAPI.Logging
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; } = 500;
        public string Message { get; set; }
        public string InnerMessage { get; set; }
        public override string ToString()
        {
            return this.Message + " " + this?.InnerMessage;
        }
    }
}
