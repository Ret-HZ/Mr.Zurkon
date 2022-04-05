using System;
using System.IO;
using Yarhl.IO;

namespace PAKLib.GIM
{

    //GIM Format Documentation: https://www.psdevwiki.com/ps3/Graphic_Image_Map_(GIM)

    public class GimData
    {

        public string filename;
        public string username;
        public string timestamp;
        public string originator;


        private GimData() { }


        private GimData(string filename, string username, string timestamp, string originator)
        {
            this.filename = filename;
            this.username = username;
            this.timestamp = timestamp;
            this.originator = originator;
        }


        public static GimData GetGimFileInfo(byte[] file)
        {
            using (var stream = DataStreamFactory.FromStream(new MemoryStream(file)))
            {
                var reader = new DataReader(stream)
                {
                    Endianness = EndiannessMode.LittleEndian,
                };

                try
                {
                    reader.Stream.Seek(0x14);
                    int ptrFileData = reader.ReadInt32();
                    reader.Stream.Seek(ptrFileData + 0x20);
                    string fn = reader.ReadString();
                    fn = Path.GetFileName(fn); //Keep only the filename
                    string un = reader.ReadString();
                    string ts = reader.ReadString();
                    ts = ts.TrimEnd('\r','\n'); //Strip new lines out
                    string o = reader.ReadString();
                    return new GimData(fn, un, ts, o);
                }
                catch (Exception)
                {
                    Console.WriteLine("GetGimData() failed.");
                    return new GimData("No Data", "No Data", "No Data", "No Data");
                }
            }
        }


    }
}
