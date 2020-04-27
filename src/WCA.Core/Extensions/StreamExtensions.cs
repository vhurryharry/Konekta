using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace WCA.Core.Extensions
{
    public static class StreamExtensions
    {
        public static Task<string> ReadAsStringAsync(this Stream stream) =>
            ReadAsStringAsync(stream, Encoding.UTF8);

        public static async Task<string> ReadAsStringAsync(this Stream stream, Encoding encoding)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            using (StreamReader reader = new StreamReader(stream, encoding))
                return await reader.ReadToEndAsync();
        }
    }
}
