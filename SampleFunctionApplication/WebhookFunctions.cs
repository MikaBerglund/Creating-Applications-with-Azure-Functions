using Azure.Storage.Blobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using SampleFunctionApplication.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SampleFunctionApplication
{
    public class WebhookFunctions
    {
        public WebhookFunctions(RootConfigSection rootConfig, BlobContainerClient blobClient)
        {
            this.RootConfig = rootConfig;
            this.BlobClient = blobClient;
        }

        private readonly RootConfigSection RootConfig;
        private readonly BlobContainerClient BlobClient;


        [FunctionName(nameof(GenericWebhookCallbackHttp))]
        public async Task<HttpResponseMessage> GenericWebhookCallbackHttp([HttpTrigger(authLevel: AuthorizationLevel.Function, "GET", "POST", "PUT")]HttpRequestMessage request, ILogger logger)
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
                await this.BlobClient.CreateIfNotExistsAsync();

                using (var strm = new MemoryStream())
                {
                    using (var writer = new StreamWriter(strm, leaveOpen: true))
                    {
                        await writer.WriteAsync(body);
                    }

                    strm.Position = 0;
                    await this.BlobClient.UploadBlobAsync($"{DateTime.UtcNow.ToString("yyyyMMdd-HHmmssfffff")}.txt", strm);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Could not store payload body in blog storage.", ex);
            }

            return true;
        }

    }
}
