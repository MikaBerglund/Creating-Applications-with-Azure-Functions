using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SampleFunctionApplication.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SampleFunctionApplication
{
    public class WebhookFunctions
    {
        public WebhookFunctions(RootConfigSection rootConfig, CloudBlobClient blobClient)
        {
            this.RootConfig = rootConfig;
            this.BlobClient = blobClient;
        }

        private readonly RootConfigSection RootConfig;
        private readonly CloudBlobClient BlobClient;


        [FunctionName(Names.GenericWebhookCallbackHttp)]
        public async Task<HttpResponseMessage> GenericWebhookCallbackHttpAsync([HttpTrigger(authLevel: AuthorizationLevel.Function, "GET", "POST", "PUT")]HttpRequestMessage request, ILogger logger)
        {
            HttpStatusCode status = HttpStatusCode.OK;
            if(request.Method == HttpMethod.Post || request.Method == HttpMethod.Put)
            {
                // If the method is post or put, then we store the payload body in the configured storage account.
                var body = await request.Content.ReadAsStringAsync();
                await this.StoreRequestBodyAsync(body);
                status = HttpStatusCode.Accepted;
            }

            var response = new HttpResponseMessage(status)
            {
                Content = new StringContent($"{{\"status\": \"{status}\"}}", Encoding.UTF8, "application/json")
            };
            return await Task.FromResult(response);
        }



        /// <summary>
        /// Stores the given payload body in a blob container, if one has been configured in the application settings.
        /// </summary>
        /// <param name="body">The body to store in the blob container.</param>
        /// <returns>Returns <c>true</c> if the body was stored.</returns>
        private async Task<bool> StoreRequestBodyAsync(string body)
        {
            if (string.IsNullOrWhiteSpace(this.RootConfig.WebHookTempContainer)) return false;

            try
            {
                var container = this.BlobClient.GetContainerReference(this.RootConfig.WebHookTempContainer);
                await container.CreateIfNotExistsAsync();

                var blob = container.GetBlockBlobReference($"{DateTime.UtcNow.ToString("yyyyMMdd-HHmmssfffff")}.txt");
                await blob.UploadTextAsync(body);
            }
            catch (Exception ex)
            {
                throw new Exception("Could not store payload body in blog storage.", ex);
            }

            return true;
        }

    }
}
