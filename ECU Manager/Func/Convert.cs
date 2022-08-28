using System;

namespace ECU_Manager
{
    public static class Convert
    {

        public static string ToHexString(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data.Length == 0)
                return string.Empty;
            if (data.Length > int.MaxValue / 2)
                throw new ArgumentOutOfRangeException(nameof(data), "SR.ArgumentOutOfRange_InputTooLarge");
            return HexConverter.ToString(data, HexConverter.Casing.Upper);
        }

        public static byte[] FromHexString(string str)
        {
            if (str == null)
                throw new ArgumentNullException(nameof(str));
            if (str.Length == 0)
                return Array.Empty<byte>();
            if ((uint)str.Length % 2 != 0)
                throw new FormatException("SR.Format_BadHexLength");

            byte[] result = new byte[str.Length >> 1];

            if (!HexConverter.TryDecodeFromUtf16(str, result))
                throw new FormatException("SR.Format_BadHexChar");

            return result;
        }
    }
}
