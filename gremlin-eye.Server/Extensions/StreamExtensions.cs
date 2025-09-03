using System.Text;

namespace gremlin_eye.Server.Extensions
{
    public static class StreamExtensions
    {
        internal static readonly Encoding DefaultEncoding = new UTF8Encoding(false, true);
        public static BinaryReader CreateReader(this Stream stream)
            => new BinaryReader(stream, DefaultEncoding, true);
        public static DateTimeOffset ReadDateTimeOffset(this BinaryReader reader)
            => new DateTimeOffset(reader.ReadInt64(), TimeSpan.Zero);
        public static void Write(this BinaryWriter writer, DateTimeOffset value)
        {
            writer.Write(value.UtcTicks);
        }
    }
}
