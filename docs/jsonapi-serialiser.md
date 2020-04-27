Back to [top level readme](../README.md)

# Overview

This page discusses the evaluation of various [{json:api}](https://jsonapi.org) serialisers for .NET. This makes it easier to work with json:api formatted documents, which are used by the Actionstep API.

## Why no package is used for {json:api}
Essentially it looks like the Actionstep API doesn't adhere completely to the {json:api} spec.

You can read more about this on the WorkCloud DevOps Wiki page [Actionstep JSON:API Conformance
](https://dev.azure.com/workcloudapps/WorkCloudApps/_wiki/wikis/WorkCloudApps.wiki?wikiVersion=GBwikiMaster&pagePath=%2F3rd%20Party%20Integrations%2FActionstep%20jsonapi%20Conformance&pageId=31).

## Packages Investigated
### JsonApiSerializer
Url: https://github.com/codecutout/JsonApiSerializer

JsonApiSerializer looked like the best option because it was the quickest to get up and running. All that is required is the creation of appropriate models.

### JsonApiFramework
Url: https://github.com/scott-mcdonald/JsonApiFramework

Investigated and briefly tested. A benefit is that it supports both server and client.

However, we didn't chose it because it seemed too complex having to create a document context with a fluent API in addition to model objects just to be able to deserialise.

### JsonApiDotNetCore
Url: https://github.com/json-api-dotnet/JsonApiDotNetCore

Only appears to support server side implementations.