using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoltUtils.LOC_DATA
{
    /// <summary>
    /// Encoding used by High Impact Games in "Ratchet & Clank: Size Matters" and "Secret Agent Clank".
    /// </summary>
    public class HighImpactEncoding : Encoding
    {
        /// <summary>
        /// ISO 8859-1 Latin 1; Western European (ISO)
        /// </summary>
        private readonly Encoding FallbackEncoding = Encoding.GetEncoding(28591);

        /// <summary>
        /// Symbol → byte[] table.
        /// </summary>
        private readonly Dictionary<string, byte[]> SymbolToBytesTable = new Dictionary<string, byte[]>
        {
            // Buttons and special icons
            ["<button=x/>"]             = [0x80],
            ["<button=square/>"]        = [0x81],
            ["<button=triangle/>"]      = [0x82],
            ["<button=circle/>"]        = [0x83],
            ["<button=joystick/>"]      = [0x84],
            ["<button=d-pad_up/>"]      = [0x85],
            ["<button=d-pad_down/>"]    = [0x86],
            ["<button=d-pad_left/>"]    = [0x87],
            ["<button=d-pad_right/>"]   = [0x88],
            ["<symbol=ellipsis/>"]      = [0x89],
            ["<symbol=registered/>"]    = [0x8A],
            ["<button=d-pad_all/>"]     = [0x8B],
            ["<button=volume_up/>"]     = [0x8E],
            ["<button=volume_down/>"]   = [0x8F],
            ["<button=select/>"]        = [0x96],
            ["<button=start/>"]         = [0x98],
            ["<button=home/>"]          = [0x9A],
            ["<button=l/>"]             = [0x9E],
            ["<button=r/>"]             = [0x9F],

            // Text color
            ["</text_color>"]           = [0x90, 0x01],
            ["<text_color=orange>"]     = [0x90, 0x02],
            ["<text_color=red>"]        = [0x90, 0x03],
            ["<text_color=magenta>"]    = [0x90, 0x04],
            ["<text_color=pink>"]       = [0x90, 0x05],
            ["<text_color=cyan>"]       = [0x90, 0x06],
            ["<text_color=orange2>"]    = [0x90, 0x07],
            ["<text_color=salmon>"]     = [0x90, 0x08],
            ["<text_color=red2>"]       = [0x90, 0x09],
            ["<text_color=blue>"]       = [0x90, 0x0A],
            ["<text_color=green>"]      = [0x90, 0x0B],
            ["<text_color=orange3>"]    = [0x90, 0x0C],
            ["<text_color=purple>"]     = [0x90, 0x0D],
            ["<text_color=brown>"]      = [0x90, 0x0E],
            ["<text_color=yellow>"]     = [0x90, 0x10],

            // Japanese
            ["、"] = [0x93, 0x01],
            ["。"] = [0x93, 0x02],
            ["「"] = [0x93, 0x0C],
            ["」"] = [0x93, 0x0D],
            ["『"] = [0x93, 0x0E],
            ["』"] = [0x93, 0x0F],
            ["〜"] = [0x93, 0x27],
            ["！"] = [0x93, 0x28],
            ["？"] = [0x93, 0x29],
            ["♪"] = [0x93, 0x2A],
            ["ぁ"] = [0x93, 0x41],
            ["あ"] = [0x93, 0x42],
            ["ぃ"] = [0x93, 0x43],
            ["い"] = [0x93, 0x44],
            ["ぅ"] = [0x93, 0x45],
            ["う"] = [0x93, 0x46],
            ["ぇ"] = [0x93, 0x47],
            ["え"] = [0x93, 0x48],
            ["ぉ"] = [0x93, 0x49],
            ["お"] = [0x93, 0x4A],
            ["か"] = [0x93, 0x4B],
            ["が"] = [0x93, 0x4C],
            ["き"] = [0x93, 0x4D],
            ["ぎ"] = [0x93, 0x4E],
            ["く"] = [0x93, 0x4F],
            ["ぐ"] = [0x93, 0x50],
            ["け"] = [0x93, 0x51],
            ["げ"] = [0x93, 0x52],
            ["こ"] = [0x93, 0x53],
            ["ご"] = [0x93, 0x54],
            ["さ"] = [0x93, 0x55],
            ["ざ"] = [0x93, 0x56],
            ["し"] = [0x93, 0x57],
            ["じ"] = [0x93, 0x58],
            ["す"] = [0x93, 0x59],
            ["ず"] = [0x93, 0x5A],
            ["せ"] = [0x93, 0x5B],
            ["ぜ"] = [0x93, 0x5C],
            ["そ"] = [0x93, 0x5D],
            ["ぞ"] = [0x93, 0x5E],
            ["た"] = [0x93, 0x5F],
            ["だ"] = [0x93, 0x60],
            ["ち"] = [0x93, 0x61],
            ["ぢ"] = [0x93, 0x62],
            ["っ"] = [0x93, 0x63],
            ["つ"] = [0x93, 0x64],
            ["づ"] = [0x93, 0x65],
            ["て"] = [0x93, 0x66],
            ["で"] = [0x93, 0x67],
            ["と"] = [0x93, 0x68],
            ["ど"] = [0x93, 0x69],
            ["な"] = [0x93, 0x6A],
            ["に"] = [0x93, 0x6B],
            ["ぬ"] = [0x93, 0x6C],
            ["ね"] = [0x93, 0x6D],
            ["の"] = [0x93, 0x6E],
            ["は"] = [0x93, 0x6F],
            ["ば"] = [0x93, 0x70],
            ["ぱ"] = [0x93, 0x71],
            ["ひ"] = [0x93, 0x72],
            ["び"] = [0x93, 0x73],
            ["ぴ"] = [0x93, 0x74],
            ["ふ"] = [0x93, 0x75],
            ["ぶ"] = [0x93, 0x76],
            ["ぷ"] = [0x93, 0x77],
            ["へ"] = [0x93, 0x78],
            ["べ"] = [0x93, 0x79],
            ["ぺ"] = [0x93, 0x7A],
            ["ほ"] = [0x93, 0x7B],
            ["ぼ"] = [0x93, 0x7C],
            ["ぽ"] = [0x93, 0x7D],
            ["ま"] = [0x93, 0x7E],
            ["み"] = [0x93, 0x7F],
            ["む"] = [0x93, 0x80],
            ["め"] = [0x93, 0x81],
            ["も"] = [0x93, 0x82],
            ["ゃ"] = [0x93, 0x83],
            ["や"] = [0x93, 0x84],
            ["ゅ"] = [0x93, 0x85],
            ["ゆ"] = [0x93, 0x86],
            ["ょ"] = [0x93, 0x87],
            ["よ"] = [0x93, 0x88],
            ["ら"] = [0x93, 0x89],
            ["り"] = [0x93, 0x8A],
            ["る"] = [0x93, 0x8B],
            ["れ"] = [0x93, 0x8C],
            ["ろ"] = [0x93, 0x8D],
            ["ゎ"] = [0x93, 0x8E],
            ["わ"] = [0x93, 0x8F],
            ["ゐ"] = [0x93, 0x90],
            ["ゑ"] = [0x93, 0x91],
            ["を"] = [0x93, 0x92],
            ["ん"] = [0x93, 0x93],
            ["ゔ"] = [0x93, 0x94],
            ["゛"] = [0x93, 0x9B],
            ["゜"] = [0x93, 0x9C],
            ["ァ"] = [0x93, 0xA1],
            ["ア"] = [0x93, 0xA2],
            ["ィ"] = [0x93, 0xA3],
            ["イ"] = [0x93, 0xA4],
            ["ゥ"] = [0x93, 0xA5],
            ["ウ"] = [0x93, 0xA6],
            ["ェ"] = [0x93, 0xA7],
            ["エ"] = [0x93, 0xA8],
            ["ォ"] = [0x93, 0xA9],
            ["オ"] = [0x93, 0xAA],
            ["カ"] = [0x93, 0xAB],
            ["ガ"] = [0x93, 0xAC],
            ["キ"] = [0x93, 0xAD],
            ["ギ"] = [0x93, 0xAE],
            ["ク"] = [0x93, 0xAF],
            ["グ"] = [0x93, 0xB0],
            ["ケ"] = [0x93, 0xB1],
            ["ゲ"] = [0x93, 0xB2],
            ["コ"] = [0x93, 0xB3],
            ["ゴ"] = [0x93, 0xB4],
            ["サ"] = [0x93, 0xB5],
            ["ザ"] = [0x93, 0xB6],
            ["シ"] = [0x93, 0xB7],
            ["ジ"] = [0x93, 0xB8],
            ["ス"] = [0x93, 0xB9],
            ["ズ"] = [0x93, 0xBA],
            ["セ"] = [0x93, 0xBB],
            ["ゼ"] = [0x93, 0xBC],
            ["ソ"] = [0x93, 0xBD],
            ["ゾ"] = [0x93, 0xBE],
            ["タ"] = [0x93, 0xBF],
            ["ダ"] = [0x93, 0xC0],
            ["チ"] = [0x93, 0xC1],
            ["ヂ"] = [0x93, 0xC2],
            ["ッ"] = [0x93, 0xC3],
            ["ツ"] = [0x93, 0xC4],
            ["ヅ"] = [0x93, 0xC5],
            ["テ"] = [0x93, 0xC6],
            ["デ"] = [0x93, 0xC7],
            ["ト"] = [0x93, 0xC8],
            ["ド"] = [0x93, 0xC9],
            ["ナ"] = [0x93, 0xCA],
            ["ニ"] = [0x93, 0xCB],
            ["ヌ"] = [0x93, 0xCC],
            ["ネ"] = [0x93, 0xCD],
            ["ノ"] = [0x93, 0xCE],
            ["ハ"] = [0x93, 0xCF],
            ["バ"] = [0x93, 0xD0],
            ["パ"] = [0x93, 0xD1],
            ["ヒ"] = [0x93, 0xD2],
            ["ビ"] = [0x93, 0xD3],
            ["ピ"] = [0x93, 0xD4],
            ["フ"] = [0x93, 0xD5],
            ["ブ"] = [0x93, 0xD6],
            ["プ"] = [0x93, 0xD7],
            ["ヘ"] = [0x93, 0xD8],
            ["ベ"] = [0x93, 0xD9],
            ["ペ"] = [0x93, 0xDA],
            ["木"] = [0x93, 0xDB],
            ["ボ"] = [0x93, 0xDC],
            ["ポ"] = [0x93, 0xDD],
            ["マ"] = [0x93, 0xDE],
            ["ミ"] = [0x93, 0xDF],
            ["ム"] = [0x93, 0xE0],
            ["メ"] = [0x93, 0xE1],
            ["モ"] = [0x93, 0xE2],
            ["ャ"] = [0x93, 0xE3],
            ["ヤ"] = [0x93, 0xE4],
            ["ュ"] = [0x93, 0xE5],
            ["ユ"] = [0x93, 0xE6],
            ["ョ"] = [0x93, 0xE7],
            ["ヨ"] = [0x93, 0xE8],
            ["ラ"] = [0x93, 0xE9],
            ["リ"] = [0x93, 0xEA],
            ["ル"] = [0x93, 0xEB],
            ["レ"] = [0x93, 0xEC],
            ["ロ"] = [0x93, 0xED],
            ["ヮ"] = [0x93, 0xEE],
            ["ワ"] = [0x93, 0xEF],
            ["ヰ"] = [0x93, 0xF0],
            ["ヱ"] = [0x93, 0xF1],
            ["ヲ"] = [0x93, 0xF2],
            ["ン"] = [0x93, 0xF3],
            ["ヴ"] = [0x93, 0xF4],
            ["ヵ"] = [0x93, 0xF5],
            ["ヶ"] = [0x93, 0xF6],
            ["・"] = [0x93, 0xFB],
            ["ー"] = [0x93, 0xFC],

            // Flags
            ["<flag=invalid>"]      = [0x95, 0x00],
            ["<flag=denmark>"]      = [0x95, 0x01],
            ["<flag=finland>"]      = [0x95, 0x02],
            ["<flag=france>"]       = [0x95, 0x03],
            ["<flag=germany>"]      = [0x95, 0x04],
            ["<flag=italy>"]        = [0x95, 0x05],
            ["<flag=netherlands>"]  = [0x95, 0x06],
            ["<flag=norway>"]       = [0x95, 0x07],
            ["<flag=portugal>"]     = [0x95, 0x08],
            ["<flag=spain>"]        = [0x95, 0x09],
            ["<flag=sweden>"]       = [0x95, 0x0A],
            ["<flag=uk>"]           = [0x95, 0x0B],
            ["<flag=poland>"]       = [0x95, 0x0C],
        };

        /// <summary>
        /// Auto-generated byte[] → symbol using hex string keys
        /// </summary>
        private readonly Dictionary<string, string> BytesToSymbolTable;


        /// <summary>
        /// Initializes a new instance of the <see cref="HighImpactEncoding"/> class.
        /// </summary>
        public HighImpactEncoding()
        {
            BytesToSymbolTable = SymbolToBytesTable.ToDictionary(
                kv => ToHexString(kv.Value),
                kv => kv.Key
            );
        }


        public override int GetByteCount(char[] chars, int index, int count)
        {
            return GetBytes(new string(chars, index, count), 0, count, null, 0);
        }


        public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            int originalByteIndex = byteIndex;
            string input = s.Substring(charIndex, charCount);

            while (input.Length > 0)
            {
                bool matched = false;

                foreach (var kv in SymbolToBytesTable)
                {
                    string symbol = kv.Key;
                    if (input.StartsWith(symbol, StringComparison.Ordinal))
                    {
                        byte[] symbolBytes = kv.Value;
                        if (bytes != null)
                            Array.Copy(symbolBytes, 0, bytes, byteIndex, symbolBytes.Length);

                        byteIndex += symbolBytes.Length;
                        input = input.Substring(symbol.Length);
                        matched = true;
                        break;
                    }
                }

                if (!matched)
                {
                    string ch = input.Substring(0, 1);
                    byte[] fallbackBytes = FallbackEncoding.GetBytes(ch);
                    if (bytes != null)
                        Array.Copy(fallbackBytes, 0, bytes, byteIndex, fallbackBytes.Length);

                    byteIndex += fallbackBytes.Length;
                    input = input.Substring(1);
                }
            }

            return byteIndex - originalByteIndex;
        }


        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return GetChars(bytes, index, count, null, 0);
        }


        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            int originalCharIndex = charIndex;
            int i = byteIndex;
            int end = byteIndex + byteCount;

            while (i < end)
            {
                string symbol = null;
                int consumed = 0;

                // Try match longest possible byte pattern
                for (int len = Math.Min(4, end - i); len >= 1; len--)
                {
                    byte[] slice = new byte[len];
                    Array.Copy(bytes, i, slice, 0, len);
                    string hex = ToHexString(slice);

                    if (BytesToSymbolTable.TryGetValue(hex, out symbol))
                    {
                        consumed = len;
                        break;
                    }
                }

                if (symbol != null)
                {
                    if (chars != null)
                    {
                        foreach (char c in symbol)
                            chars[charIndex++] = c;
                    }
                    else
                    {
                        charIndex += symbol.Length;
                    }

                    i += consumed;
                }
                else
                {
                    // Fallback decode
                    string fallbackChar = FallbackEncoding.GetString(new byte[] { bytes[i++] });
                    if (chars != null)
                    {
                        foreach (char c in fallbackChar)
                            chars[charIndex++] = c;
                    }
                    else
                    {
                        charIndex += fallbackChar.Length;
                    }
                }
            }

            return charIndex - originalCharIndex;
        }


        public override int GetMaxByteCount(int charCount) => charCount * 4;
        

        public override int GetMaxCharCount(int byteCount) => byteCount * 4;


        private static string ToHexString(byte[] data)
        {
            return BitConverter.ToString(data);
        }


        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            string s = new string(chars, charIndex, charCount);
            return GetBytes(s, 0, s.Length, bytes, byteIndex);
        }
    }
}
