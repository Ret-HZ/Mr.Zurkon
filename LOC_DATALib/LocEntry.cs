using System;
using System.Collections.Generic;
using System.Text;

namespace LOC_DATALib
{
    public class LocEntry
    {
        public int ID { get; set; }
        public string Text { get; set; }


        public LocEntry(int id, string text)
        {
            this.ID = id;
            this.Text = text;
        }



        /// <summary>
        /// Converts the text to a byte array. Special characters will be converted to their respective codes
        /// </summary>
        /// <returns></returns>
        public byte[] TextToBytes()
        {
            Dictionary<string, string> TextBytesToCodeTable = SpecialCharacters.GetTextBytesToCodeTable();

            string textBytesString = Util.TextStringToHexString(this.Text);

            foreach (KeyValuePair<string, string> kvp in TextBytesToCodeTable)
            {
                int index = -1;
                do
                {
                    index = textBytesString.IndexOf(kvp.Key, index + 1);
                    if (index != -1 && index % 2 == 0)
                    {
                        if (this.ID == 235) Console.WriteLine(textBytesString + " | " + kvp.Key);
                        textBytesString = textBytesString.Remove(index, kvp.Key.Length).Insert(index, kvp.Value);

                    }
                } while (index != -1);
            }

            return Util.StringToByteArray(textBytesString+"00");
        }
    }
}
