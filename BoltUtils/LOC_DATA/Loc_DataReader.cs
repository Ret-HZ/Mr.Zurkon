using System;
using System.Collections.Generic;
using System.IO;
using Yarhl.IO;

namespace BoltUtils.LOC_DATA
{
    public class Loc_DataReader
    {
        /// <summary>
        /// Text encoding used by SM and SAC.
        /// </summary>
        private static HighImpactEncoding HIEncoding = new HighImpactEncoding();


        /// <summary>
        /// Reads a LOC_DATA file.
        /// </summary>
        /// <param name="reader">The <see cref="DataReader"/>.</param>
        /// <returns>A <see cref="LOC_DATA"/> object.</returns>
        private static LOC_DATA ReadLOC_DATA(DataReader reader)
        {
            string magic = reader.ReadString(8);
            if (magic != "LOC_DATA")
            {
                throw new Exception("Invalid magic. Expected LOC_DATA");
            }
            LOC_DATA locdata = new LOC_DATA();

            locdata.unk0x08 = reader.ReadByte(); //Version? Type?
            byte[] unk0x09 = reader.ReadBytes(0x3); //Always 0x010612
            byte localisationAmount = reader.ReadByte();
            Int16 locEntryAmount = (short)reader.ReadInt24();

            //SCOL Section
            reader.ReadString(4); //SCOL Magic
            locdata.unkSCOL = reader.ReadBytes(0x20);

            //LTOC Section
            List<int> TDEFStartPointers = new List<int>();
            List<int> TDEFSizes = new List<int>();
            List<int> MetadataOffsets = new List<int>(); //Relative to TDEF start pointers

            reader.ReadString(4); //LTOC Magic
            for (int i=0; i<localisationAmount; i++)
            {
                TDEFStartPointers.Add(reader.ReadInt32());
                TDEFSizes.Add(reader.ReadInt32());
                MetadataOffsets.Add(reader.ReadInt32());
            }

            //TDEF Sections
            for (int locIndex=0; locIndex < localisationAmount; locIndex++)
            {
                Localisation localisation = new Localisation();
                int TDEFstartPointer = TDEFStartPointers[locIndex];
                reader.Stream.Seek(TDEFstartPointer);

                for (int locEntryIndex=0; locEntryIndex < locEntryAmount; locEntryIndex++)
                {
                    reader.ReadString(4); //TDEF Magic
                    int id = reader.ReadInt32();
                    int ptrString = reader.ReadInt32();
                    reader.Stream.PushToPosition(TDEFstartPointer + ptrString);
                    string text = ReadString(reader);

                    localisation.AddEntry(id, text);
                    reader.Stream.PopPosition();
                }

                //TODO: Metadata
                locdata.AddLocalisation(localisation);
            }

            return locdata;
        }


        /// <summary>
        /// Reads a LOC_DATA file.
        /// </summary>
        /// <param name="path">The path to the LOC_DATA file.</param>
        /// <returns>A <see cref="LOC_DATA"/> object.</returns>
        public static LOC_DATA ReadLOC_DATA(string path)
        {
            using (Stream stream = new MemoryStream(File.ReadAllBytes(path)))
            using (var dataStream = DataStreamFactory.FromStream(stream))
            {
                var reader = new DataReader(dataStream)
                {
                    Endianness = EndiannessMode.LittleEndian,
                };

                return ReadLOC_DATA(reader);
            }
        }


        /// <summary>
        /// Reads a LOC_DATA file.
        /// </summary>
        /// <param name="fileBytes">The LOC_DATA file as byte array.</param>
        /// <param name="offset">The location in the array to start reading data from.</param>
        /// <param name="length">The number of bytes to read from the array.</param>
        /// <returns>A <see cref="LOC_DATA"/> object.</returns>
        public static LOC_DATA ReadLOC_DATA(byte[] fileBytes, int offset = 0, int length = 0)
        {
            if (length == 0) length = fileBytes.Length;
            using (var stream = new MemoryStream(fileBytes, offset, length))
            using (var dataStream = DataStreamFactory.FromStream(stream))
            {
                var reader = new DataReader(dataStream)
                {
                    Endianness = EndiannessMode.LittleEndian,
                };

                return ReadLOC_DATA(reader);
            }
        }


        /// <summary>
        /// Reads a string using the <see cref="HighImpactEncoding"/>.
        /// </summary>
        /// <param name="reader">The <see cref="DataReader"/>.</param>
        /// <returns>A string.</returns>
        private static string ReadString(DataReader reader)
        {
            List<byte> stringBytes = new List<byte>();
            byte readByte;
            do
            {
                readByte = reader.ReadByte();
                stringBytes.Add(readByte);
            }
            while (readByte != 0x00);

            return HIEncoding.GetString(stringBytes.ToArray());
        }
    }
}
