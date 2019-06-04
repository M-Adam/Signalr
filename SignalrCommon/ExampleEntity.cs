using System;
using System.Collections.Generic;
using System.Text;

namespace SignalrCommon
{
    public class ExampleEntity
    {
        public string Data { get; set; }

        public override string ToString()
        {
            return $"ExampleEntity Data: {Data}";
        }
    }
}
