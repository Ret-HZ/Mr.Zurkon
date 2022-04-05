using System;
using System.Collections.Generic;
using System.Text;

namespace PAKLib
{
    public static class PakContentFormats
    {
        public enum Format
        {
            UNKNOWN,
            GIM, //Texture
            MB_PSP_GDE, //Model
            MB_PS2_GDE, //Model
        }


        public static Dictionary<Format, byte[]> MagicBytes = new Dictionary<Format, byte[]>()
        {
            {Format.UNKNOWN, new byte[]{} },
            {Format.GIM, new byte[]{0x4D, 0x49, 0x47, 0x2E, 0x30, 0x30, 0x2E, 0x31} }, //MIG.00.1
            {Format.MB_PSP_GDE, new byte[]{0x11, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x10} }, //0x11000100 01000010 PSP
            {Format.MB_PS2_GDE, new byte[]{0x01, 0x00, 0x01, 0x00, 0x01, 0x00, 0x00, 0x10} }, //0x11000100 01000010 PS2
        };


        public static Dictionary<Format, string> Extension = new Dictionary<Format, string>()
        {
            {Format.UNKNOWN, "DAT" },
            {Format.GIM, "GIM" },
            {Format.MB_PSP_GDE, "MB.PSP.GDE" }, //0x11000100 01000010
            {Format.MB_PS2_GDE, "MB.PS2.GDE" }, //0x01000100 01000010
        };
    }
}
