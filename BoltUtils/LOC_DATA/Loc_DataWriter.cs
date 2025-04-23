using System.Collections.Generic;
using System.IO;
using Yarhl.IO;

namespace BoltUtils.LOC_DATA
{
    public class Loc_DataWriter
    {
        /// <summary>
        /// Text encoding used by SM and SAC.
        /// </summary>
        private static HighImpactEncoding HIEncoding = new HighImpactEncoding();


        /// <summary>
        /// Writes a LOC_DATA file.
        /// </summary>
        /// <param name="writer">The <see cref="DataWriter"/>.</param>
        private static void WriteLOC_DATA(DataWriter writer, LOC_DATA locdata)
        {
            writer.Write("LOC_DATA", false);
            writer.Write(locdata.unk0x08);

            writer.Write((byte)0x01); //Always 0x010612
            writer.Write((byte)0x06);
            writer.Write((byte)0x12);

            writer.Write(locdata.GetLocalisationAmount());
            writer.Write(locdata.GetLocalisationEntryAmount());
            writer.Write((byte)0);

            //SCOL
            writer.Write("SCOL", false);
            writer.Write(locdata.unkSCOL);

            //LTOC
            List<int> PointersLTOC = new List<int>();
            writer.Write("LTOC", false);
            for (int ltocplaceholderindex = 0; ltocplaceholderindex < locdata.GetLocalisationAmount(); ltocplaceholderindex++)
            {
                PointersLTOC.Add((int)writer.Stream.Position);
                writer.Write(0); //Placeholder TDEF start pointer
                writer.Write(0); //Placeholder TDEF size
                writer.Write(0); //Placeholder Metadata offset
            }

            //TDEF and ENTR Sections
            for (int tdefindex = 0; tdefindex<locdata.GetLocalisationAmount(); tdefindex++)
            {
                Localisation localisation = locdata.GetLocalisation(tdefindex);

                List<int> PointersTDEF = new List<int>(); //TDEF Pointers to be updated after the strings are written
                int TDEFStartPosition = (int)writer.Stream.Position;
                for (int locentryindex = 0; locentryindex < localisation.GetEntryAmount(); locentryindex++)
                {
                    LocEntry entry = localisation.GetEntry(locentryindex);
                    writer.Write("TDEF", false);
                    writer.Write(entry.ID);
                    PointersTDEF.Add((int)writer.Stream.Position);
                    writer.Write(0);
                }

                //Write ENTR and update TDEF pointers
                for (int locentryindex = 0; locentryindex < localisation.GetEntryAmount(); locentryindex++)
                {
                    writer.Write("ENTR", false);
                    int textOffset = (int)(writer.Stream.Position - TDEFStartPosition);
                    writer.Write(HIEncoding.GetBytes(localisation.GetEntry(locentryindex).Text));
                    writer.Write((byte)0x00); // Null terminator
                    writer.Stream.PushToPosition(PointersTDEF[locentryindex]);
                    writer.Write(textOffset);
                    writer.Stream.PopPosition();
                }

                writer.WritePadding(0x00, 0x10);
                //TODO: Metadata

                //Update LTOC section data
                int sectionSize = (int)(writer.Stream.Position - TDEFStartPosition);
                writer.Stream.PushToPosition(PointersLTOC[tdefindex]);
                writer.Write(TDEFStartPosition);
                writer.Write(sectionSize);
                writer.Stream.PopPosition();
            }
        }


        /// <summary>
        /// Writes a <see cref="LOC_DATA"/> to a file.
        /// </summary>
        /// <param name="locdata">The <see cref="LOC_DATA"/> to write.</param>
        /// <param name="path">The destination file path.</param>
        public static void WriteLOC_DATAToFile(LOC_DATA locdata, string path)
        {
            using (Stream stream = new MemoryStream())
            using (DataStream dataStream = DataStreamFactory.FromStream(stream))
            {
                var writer = new DataWriter(dataStream)
                {
                    Endianness = EndiannessMode.LittleEndian,
                };

                WriteLOC_DATA(writer, locdata);
                dataStream.WriteTo(path);
            }
        }


        /// <summary>
        /// Writes a <see cref="LOC_DATA"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="locdata">The <see cref="LOC_DATA"/> to write.</param>
        public static Stream WriteLOC_DATAToStream(LOC_DATA locdata)
        {
            using (DataStream dataStream = DataStreamFactory.FromMemory())
            {
                var writer = new DataWriter(dataStream)
                {
                    Endianness = EndiannessMode.LittleEndian,
                };

                WriteLOC_DATA(writer, locdata);
                Stream stream = new MemoryStream();
                writer.Stream.WriteTo(stream);
                return stream;
            }
        }


        /// <summary>
        /// Writes a <see cref="LOC_DATA"/> to a byte array.
        /// </summary>
        /// <param name="locdata">The <see cref="LOC_DATA"/> to write.</param>
        public static byte[] WriteLOC_DATAToArray(LOC_DATA locdata)
        {
            using (Stream stream = new MemoryStream())
            using (DataStream dataStream = DataStreamFactory.FromStream(stream))
            {
                var writer = new DataWriter(dataStream)
                {
                    Endianness = EndiannessMode.LittleEndian,
                };

                WriteLOC_DATA(writer, locdata);
                return writer.Stream.ToArray();
            }
        }
    }
}
