using System;
using System.Collections.Generic;
using System.Text;

namespace LOC_DATALib
{
    public class SpecialCharacters
    {
        //special character bytes conversion tables here (0x81 -> <button:square/>)

        //Storing them as hex strings because its easier to substitute albeit less efficient
        public static Dictionary<string, string> CodeToTextTable = new Dictionary<string, string>()
        {
            { "80", "<button=x/>" },
            { "81", "<button=square/>" },
            { "82", "<button=triangle/>" },
            { "83", "<button=circle/>" },
            { "84", "<button=joystick/>" },
            { "85", "<button=d-pad_up/>" },
            { "86", "<button=d-pad_down/>" },
            { "87", "<button=d-pad_left/>" },
            { "88", "<button=d-pad_right/>" },
            { "89", "<symbol=ellipsis/>" },
            { "8a", "<symbol=registered/>" },
            { "8b", "<button=d-pad_all/>" },
            { "8e", "<button=volume_up/>" },
            { "8f", "<button=volume_down/>" },
            { "96", "<button=select/>" },
            { "98", "<button=start/>" },
            { "9a", "<button=home/>" },
            { "9e", "<button=l/>" },
            { "9f", "<button=r/>" },
            //Text color
            { "9001", "</text_color>" },
            { "9002", "<text_color=orange>" },
            { "9003", "<text_color=red>" },
            { "9004", "<text_color=magenta>" },
            { "9005", "<text_color=pink>" },
            { "9006", "<text_color=cyan>" },
            { "9007", "<text_color=orange2>" },
            { "9008", "<text_color=salmon>" },
            { "9009", "<text_color=red2>" },
            { "900a", "<text_color=blue>" },
            { "900b", "<text_color=green>" },
            { "900c", "<text_color=orange3>" },
            { "900d", "<text_color=purple>" },
            { "900e", "<text_color=brown>" },
            { "900f", "<text_color=white>" },
            { "9010", "<text_color=yellow>" },

            // 9109 may be another special code. Search for Delicious in the SAC main loc file or at offset 0x8EAE
            // More possible symbols at 0xB517

            //Flags
            { "9500", "<flag=invalid>" },
            { "9501", "<flag=denmark>" },
            { "9502", "<flag=finland>" },
            { "9503", "<flag=france>" },
            { "9504", "<flag=germany>" },
            { "9505", "<flag=italy>" },
            { "9506", "<flag=netherlands>" },
            { "9507", "<flag=norway>" },
            { "9508", "<flag=portugal>" },
            { "9509", "<flag=spain>" },
            { "950a", "<flag=sweden>" },
            { "950b", "<flag=uk>" },
            { "950c", "<flag=poland>" },
        };



        private static Dictionary<string, string> TextBytesToCodeTable;


        public static Dictionary<string, string> GetTextBytesToCodeTable()
        {
            if (TextBytesToCodeTable == null || TextBytesToCodeTable.Count == 0)
            {
                TextBytesToCodeTable = new Dictionary<string, string>();
                foreach (KeyValuePair<string, string> kvp in CodeToTextTable)
                {
                    TextBytesToCodeTable.Add(Util.TextStringToHexString(kvp.Value), kvp.Key);
                }
            }

            return TextBytesToCodeTable;
        }
    }
}
