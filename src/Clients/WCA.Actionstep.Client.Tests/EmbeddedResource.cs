using System.IO;
using System.Reflection;

namespace WCA.Actionstep.Client.Tests
{
    internal static class EmbeddedResource
    {
        internal static string Read(string file)
        {
            var assembly = typeof(EmbeddedResource).GetTypeInfo().Assembly;
            var resourceName = $"{typeof(EmbeddedResource).Namespace}.{file}";

            var resourceInfo = assembly.GetManifestResourceInfo(resourceName);
            if (resourceInfo == null)
                throw new FileNotFoundException(
                    "Can't find Embedded Test Resource file. Have you set 'Build Action' to 'Embedded resource' in the file's properties?",
                    resourceName);

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
