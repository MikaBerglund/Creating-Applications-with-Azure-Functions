Creating Applications with Azure Functions
==========================================

This repository contains the sample code that goes along with a [blog post](https://mikaberglund.com/2019/09/15/building-applications-with-azure-functions/) I wrote on my blog.

To run this sample code locally, you need to check also the documentation for the [local.settins.json](SampleFunctionApplication/local.settings.json.md) configuration file.


Code Overview
-------------

The main things to note in the sample application are outlined below.

- [**The Startup class**](SampleFunctionApplication/Startup.cs) - Takes care of parsing the configuration information into a strongly typed class instance and registers instances that we need to work with blob containers as services that then get injected into our function class.
- [**WebHookFunctions class**](SampleFunctionApplication/WebhookFunctions.cs) - A non-static class that defines a constructor that takes the injected services as constructor parameters. Defines also an HTTP triggered function method that demonstrates how you can use injected dependencies in such a method.
