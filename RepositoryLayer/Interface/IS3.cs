using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public  interface IS3
    {
        public Task<bool> UploadFileAsync(IFormFile file, string bucketName, string objectKey);

    }
}
