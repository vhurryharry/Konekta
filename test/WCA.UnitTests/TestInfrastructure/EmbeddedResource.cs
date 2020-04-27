using System.IO;
using System.Reflection;

namespace WCA.UnitTests.TestInfrastructure
{
    public static class EmbeddedResource
    {
        public static Stream GetStream(string file)
        {
            var assembly = typeof(EmbeddedResource).GetTypeInfo().Assembly;
            var resourceName = string.Join(".", nameof(WCA), nameof(UnitTests), file);

            var resourceInfo = assembly.GetManifestResourceInfo(resourceName);
            if (resourceInfo == null)
                throw new FileNotFoundException(
                    "Can't find Embedded Test Resource file. Have you set 'Build Action' to 'Embedded resource' in the file's properties?",
                    resourceName);

            return assembly.GetManifestResourceStream(resourceName);

        }
        public static string Read(string file)
        {
            using (Stream stream = GetStream(file))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
