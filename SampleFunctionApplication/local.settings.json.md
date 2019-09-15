local.settings.json
===================

To successfully run this sample application in your local development environment, you need to have a `local.settins.json` file stored in the same folder with this documentation. For security reasons, configuration files should ***never*** be stored in your source code repository.

The contents of the `local.settins.json` configuration file is described below.

``` JSON
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "WebHookTempContainer": "webhook-payloads-temp",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet"
  }
}
```

If you use the configuration above, you should have the [Azure Storage emulator](https://docs.microsoft.com/en-gb/azure/storage/common/storage-use-emulator) installed on your computer.

Configuration Values
--------------------

The configuration settings are described in the table below.

|Setting|Description|
|-------|-----------|
|AzureWebJobsStorage|The connection string to the storage account to use in the application.|
|WebHookTempContainer|The name of the blob container to use to store the webhook payload in (Only for demonstration purposes).|