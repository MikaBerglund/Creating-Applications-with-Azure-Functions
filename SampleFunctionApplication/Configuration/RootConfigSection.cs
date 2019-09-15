using System;
using System.Collections.Generic;
using System.Text;

namespace SampleFunctionApplication.Configuration
{
    public class RootConfigSection
    {

        public string AzureWebJobsStorage { get; set; }

        public string WebHookTempContainer { get; set; }

    }
}
