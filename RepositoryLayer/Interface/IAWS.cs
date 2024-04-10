using Amazon.SQS.Model;
using ModelLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface IAWS
    {
        public Task<bool> SendMessageAsync(Tuple<string, string, string, long, string> empDetail);

        public Task<List<Message>> RecieveMessageAsync();

        public Task<bool> DeleteMessageAsync(string messageRecieptHandle);

    }
}
