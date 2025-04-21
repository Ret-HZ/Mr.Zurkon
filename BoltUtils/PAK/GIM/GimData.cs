using System;
using System.IO;
using Yarhl.IO;

namespace BoltUtils.PAK.GIM
{
    //GIM Format Documentation: https://www.psdevwiki.com/ps3/Graphic_Image_Map_(GIM)

    public class GimData
    {
        public string Filename {get; internal set; }
        public string Username { get; internal set; }
        public string Timestamp { get; internal set; }
        public string Originator { get; internal set; }


        private GimData() { }


        private GimData(string filename, string username, string timestamp, string originator)
        {
            this.Filename = filename;
            this.Username = username;
            this.Timestamp = timestamp;
            this.Originator = originator;
        }


        /// <summary>
        /// Extracts creation metadata from the GIM file.
        /// </summary>
        /// <param name="file">The GIM file as a byte array.</param>
        /// <returns>A <see cref="GimData"/> object.</returns>
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
                    fn = Path.GetFileName(fn); //Keep only the Filename
                    string un = reader.ReadString();
                    string ts = reader.ReadString();
                    ts = ts.TrimEnd('\r', '\n'); //Strip new lines out
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
