using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ModelLayer;
using Newtonsoft.Json;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class AwsRL : IAWS
    {

        private readonly IAmazonSQS _sqs;

        private readonly IConfiguration _config;

        public AwsRL(IAmazonSQS sqs, IConfiguration config)
        {
            _sqs = sqs;
            _config = config;
        }

        public async Task<bool> SendMessageAsync(Tuple<string, string, string, long, string> empDetail)
        {
            try 
            {
                string message = JsonConvert.SerializeObject(empDetail);
                var sendRequest = new SendMessageRequest(_config["ServiceConfiguration:QueueUrl"], message);
                // Post message or payload to queue  
                var sendResult = await _sqs.SendMessageAsync(sendRequest);

                return sendResult.HttpStatusCode == System.Net.HttpStatusCode.OK;

            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<Message>> RecieveMessageAsync()
        {
            try
            {
                var request = new ReceiveMessageRequest
                {
                    QueueUrl = _config["ServiceConfiguration:QueueUrl"],
                    MaxNumberOfMessages = 10,           //This parameter specifies the maximum number of messages to retrieve from the queue in a single request.
                    WaitTimeSeconds = 5
                };

                var result = await _sqs.ReceiveMessageAsync(request);

                // return result.Messages.Any() ? result.Messages : new List<Message>();

                if (result.Messages.Any())
                {
                    return result.Messages;
                }
               
                
            }


            catch (Exception ex)
            {
                throw ex;
            }
            return new List<Message>();

        }

        public async Task<bool> DeleteMessageAsync(string messageReceiptHandle)
        {
            try
            {
                //Deletes the specified message from the specified queue  
                var deleteResult = await _sqs.DeleteMessageAsync(_config["ServiceConfiguration:QueueUrl"], messageReceiptHandle);
                return deleteResult.HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
