using System;
using System.Collections.Generic;
using System.Text;

namespace Working.XUnitTest
{
    public class ResponseJson
    {
        public int result
        { get; set; }
        public string message
        { get; set; }

        public dynamic data
        { get; set; }
    }
}
