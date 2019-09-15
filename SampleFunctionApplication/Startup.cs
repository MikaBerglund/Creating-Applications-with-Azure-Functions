using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            var account = CloudStorageAccount.Parse(rootConfig.AzureWebJobsStorage);
            builder.Services.AddSingleton(account);

            var blobClient = account.CreateCloudBlobClient();
            builder.Services.AddSingleton(blobClient);
        }
    }
}
