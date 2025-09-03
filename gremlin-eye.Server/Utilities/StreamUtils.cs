namespace gremlin_eye.Server.Utilities
{
    public static class StreamUtils
    {
        public static ulong ReadUInt64(byte[] buffer, int offset)
        {
            return (((ulong)buffer[offset + 0]) << 56)
                   | (((ulong)buffer[offset + 1]) << 48)
                   | (((ulong)buffer[offset + 2]) << 40)
                   | (((ulong)buffer[offset + 3]) << 32)
                   | (((ulong)buffer[offset + 4]) << 24)
                   | (((ulong)buffer[offset + 5]) << 16)
                   | (((ulong)buffer[offset + 6]) << 8)
                   | (ulong)buffer[offset + 7];
        }

        public static void WriteUInt64(byte[] buffer, int offset, ulong value)
        {
            buffer[offset + 0] = (byte)(value >> 56);
            buffer[offset + 1] = (byte)(value >> 48);
            buffer[offset + 2] = (byte)(value >> 40);
            buffer[offset + 3] = (byte)(value >> 32);
            buffer[offset + 4] = (byte)(value >> 24);
            buffer[offset + 5] = (byte)(value >> 16);
            buffer[offset + 6] = (byte)(value >> 8);
            buffer[offset + 7] = (byte)(value);
        }
    }
}
