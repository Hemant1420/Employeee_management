using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
  
        public class AllMessage
        {
           
            public string MessageId { get; set; }
            public string ReceiptHandle { get; set; }

            public Tuple<string,string,string,long,string> Employee { get; set; } 

        }
        public class DeleteMessage
        {
            public string ReceiptHandle { get; set; }
        }
    
}
