using System;
using System.Collections.Generic;
using System.Text;

namespace SampleFunctionApplication
{
    /// <summary>
    /// Defines constants for function names to ensure that all function names are unique.
    /// </summary>
    public static class Names
    {
        /// <summary>
        /// A HTTP triggered function that is used as a generic callback function for webhooks.
        /// </summary>
        public const string GenericWebhookCallbackHttp = nameof(GenericWebhookCallbackHttp);

    }
}
