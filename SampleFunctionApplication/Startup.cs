using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage.Blob;
using SampleFunctionApplication;
using SampleFunctionApplication.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: WebJobsStartup(typeof(Startup))]

namespace SampleFunctionApplication
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();
            var config = serviceProvider.GetService<IConfiguration>();
            var rootConfig = config.Get<RootConfigSection>();
            builder.Services.AddSingleton(rootConfig);

            var blobService = new BlobServiceClient(rootConfig.AzureWebJobsStorage);
            var blobClient = blobService.GetBlobContainerClient(rootConfig.WebHookTempContainer);
            builder.Services.AddSingleton(blobClient);
        }

    }
}
