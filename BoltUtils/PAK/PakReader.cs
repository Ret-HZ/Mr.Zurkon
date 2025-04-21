using System;
using System.IO;
using Yarhl.IO;

namespace BoltUtils.PAK
{
    public static class PakReader
    {
        public static PAK ReadPAK(string path)
        {
            using (var stream = DataStreamFactory.FromFile(path, FileOpenMode.Read))
            {
                var reader = new DataReader(stream) 
                {
                    Endianness = EndiannessMode.LittleEndian,
                };


                string magic = reader.ReadString(4);
                if (magic != "PAKK")
                {
                    throw new Exception("Invalid magic. Expected PAKK");
                }
                int unk0x04 = reader.ReadInt32(); //Version?, Format Index?. No clue
                int unk0x08 = reader.ReadInt32(); //Seems to always match with the pointer to the first element
                int fileAmount = reader.ReadInt32();

                PAK pak = new PAK(Path.GetFileName(path));

                for (int i=0; i<fileAmount; i++)
                {
                    int ptrFile = reader.ReadInt32();
                    int size = reader.ReadInt32();
                    reader.Stream.PushToPosition(ptrFile);
                    byte[] file = reader.ReadBytes(size);
                    pak.AddFile(file);
                    reader.Stream.PopPosition();
                }

                return pak;
            }
        }
    }
}
