using Amazon.Runtime.SharedInterfaces;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class S3RL : IS3

     {
        private readonly IAmazonS3 _awsS3;

        public S3RL(IAmazonS3 awsS3)
        {
            _awsS3 = awsS3; 
        }

        public async Task<bool> UploadFileAsync(IFormFile file, string bucketName, string objectKey)
        {
            try
            {
                var bucketExists = await _awsS3.DoesS3BucketExistAsync(bucketName);
                if (!bucketExists)
                {
                    return false;
                }

                var request = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    InputStream = file.OpenReadStream(),
                    ContentType = file.ContentType // Optional: Set content type if known

                };
                var sendResult = await _awsS3.PutObjectAsync(request);

                return sendResult.HttpStatusCode == System.Net.HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., logging, returning false)
                Console.WriteLine($"Error uploading file to S3: {ex.Message}");
                return false;
            }
        }

    }
}
