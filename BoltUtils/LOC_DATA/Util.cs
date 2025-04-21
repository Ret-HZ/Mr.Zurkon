using System;
using System.Text;

namespace BoltUtils.LOC_DATA
{
    public static class Util
    {
        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }


        public static string TextStringToHexString(string text)
        {
            byte[] textbytes = Encoding.GetEncoding(28591).GetBytes(text); // iso-8859-1  Western European (ISO)
            return ByteArrayToString(textbytes);
        }


        public static string TextStringToHexString(string text, Encoding encoding)
        {
            byte[] textbytes = encoding.GetBytes(text);
            return ByteArrayToString(textbytes);
        }


        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
