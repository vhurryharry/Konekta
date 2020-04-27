# PEXA Client

This project contains a C# client for the PEXA API. It is mostly auto-generated from the PEXA OpenAPI YAML definition using [NSwag](https://github.com/RicoSuter/NSwag).

## How to Update the Client

1. Grab the latest YAML file from https://developer.pexalabs.com.au/.
   > *NOTE:* This can only be accessed from a white-listed IP Address.
2. Update the YAML file at the root of this solution.
   > *NOTE:* You may need to update the filename if the version number has changed.
3. If the file name has changed, update the YAML file name in `nswag.yaml`.
4. For the time being you manually convert the YAML files to JSON. While NSwag says it should support YAML, it doesn't at this stage.
5.1 You may also need to fix up any other issues within the YAML/JSON schema, such as missing `$ref`'s.
6. Run nswag from the command line:
```shell
cd src/Clients/WCA.PEXA.Client/
nswag run /runtime:netcore20
```
7. Go through the generated / updated files and carefully compare them against previous versions. Pay particular attention to:
   1. Collections. The XML serializer requires concrete collection implementations and doesn't work with interfaces. E.g. change ICollection properties to `System.Collections.ObjectModel.Collection`.
   2. Ensure enums have the `XmlEnum` attribute instead of the Json attribute. E.g.`[System.Xml.Serialization.XmlEnum(Name = @"Incoming Proprietor")]`
   2. Look for invalid characters in enum names, such as `-`, or `&`.

> The following doesn't work because of the manualy changes required to generated output:

~~6. Build the solution. There is an [MSBuild](https://github.com/RicoSuter/NSwag/wiki/MSBuild) task that should re-generate the `PEXAClient.Generated.cs` file from the updated YAML.~~
   > ~~*NOTE:* You may need to delete the `*.Generated.cs` files and run the build twice. The first build will re-generate the files, the second build will compile them.~~