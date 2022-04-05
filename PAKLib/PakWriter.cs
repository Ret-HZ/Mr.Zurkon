using System.Collections.Generic;
using Yarhl.IO;

namespace PAKLib
{
    public class PakWriter
    {
        public static void WritePAK(string path, PAK pak)
        {
            using (var stream = DataStreamFactory.FromFile(path, FileOpenMode.Write))
            {
                var writer = new DataWriter(stream);

                List<byte[]> files = pak.GetPAKContents();

                int fileAmount = files.Count;

                writer.Write("PAKK", false);
                writer.Write(285605888); //0x00000611. Always the same
                writer.Write(0); //Pointer to the first file (?). Possibly irrelevant
                writer.Write(fileAmount);

                List<int> mainTableOffsets = new List<int>(); //Offsets to populate with pointers and sizes after writing the files.

                for (int i=0; i<fileAmount; i++)
                {
                    mainTableOffsets.Add((int)writer.Stream.Position);
                    writer.Write(0); //Placeholder pointer
                    writer.Write(files[i].Length); //File size
                }

                writer.WritePadding(0x00, 0x10);

                for (int j=0; j<fileAmount; j++)
                {
                    int fileOffset = (int)writer.Stream.Position;
                    writer.Write(files[j]);
                    writer.WritePadding(0x00, 0x10);
                    writer.Stream.PushToPosition(mainTableOffsets[j]);
                    writer.Write(fileOffset);
                    writer.Stream.PopPosition();
                }
            }
        }
    }
}
