using System;
using System.Collections.Generic;
using System.IO;
using Yarhl.IO;

namespace BoltUtils.LOC_DATA
{
    public class Loc_DataReader
    {
        /// <summary>
        /// Reads a LOC_DATA file.
        /// </summary>
        /// <param name="reader">The <see cref="DataReader"/>.</param>
        /// <param name="encodingVariant">The <see cref="EncodingVariant"/> to use.</param>
        /// <returns>A <see cref="LOC_DATA"/> object.</returns>
        private static LOC_DATA ReadLOC_DATA(DataReader reader, EncodingVariant encodingVariant)
        {
            string magic = reader.ReadString(8);
            if (magic != "LOC_DATA")
            {
                throw new Exception("Invalid magic. Expected LOC_DATA");
            }
            LOC_DATA locdata = new LOC_DATA();

            locdata.Version = reader.ReadInt32(); // Version?
            HighImpactEncoding HIEncoding = new HighImpactEncoding(encodingVariant, (Version)locdata.Version);

            byte localisationAmount = reader.ReadByte();
            if (locdata.Version == (int)Version.SM_DEMO) // The SM demo has 0xFF as the localisation amount. It is supposed to be 4.
            {
                localisationAmount = 4;
            }
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
                if (locdata.Version == (int)Version.SM_SAC_RETAIL) // This data does not exist in previous versions.
                {
                    MetadataOffsets.Add(reader.ReadInt32());
                }
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
                    string text = ReadString(reader, HIEncoding);

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
        /// <param name="encodingVariant">The <see cref="EncodingVariant"/> to use.</param>
        /// <returns>A <see cref="LOC_DATA"/> object.</returns>
        public static LOC_DATA ReadLOC_DATA(string path, EncodingVariant encodingVariant)
        {
            using (Stream stream = new MemoryStream(File.ReadAllBytes(path)))
            using (var dataStream = DataStreamFactory.FromStream(stream))
            {
                var reader = new DataReader(dataStream)
                {
                    Endianness = EndiannessMode.LittleEndian,
                };

                return ReadLOC_DATA(reader, encodingVariant);
            }
        }


        /// <summary>
        /// Reads a LOC_DATA file.
        /// </summary>
        /// <param name="fileBytes">The LOC_DATA file as byte array.</param>
        /// <param name="offset">The location in the array to start reading data from.</param>
        /// <param name="length">The number of bytes to read from the array.</param>
        /// <param name="encodingVariant">The <see cref="EncodingVariant"/> to use.</param>
        /// <returns>A <see cref="LOC_DATA"/> object.</returns>
        public static LOC_DATA ReadLOC_DATA(byte[] fileBytes, EncodingVariant encodingVariant, int offset = 0, int length = 0)
        {
            if (length == 0) length = fileBytes.Length;
            using (var stream = new MemoryStream(fileBytes, offset, length))
            using (var dataStream = DataStreamFactory.FromStream(stream))
            {
                var reader = new DataReader(dataStream)
                {
                    Endianness = EndiannessMode.LittleEndian,
                };

                return ReadLOC_DATA(reader, encodingVariant);
            }
        }


        /// <summary>
        /// Reads a string using the provided <see cref="HighImpactEncoding"/>.
        /// </summary>
        /// <param name="reader">The <see cref="DataReader"/>.</param>
        /// <param name="encoding">The <see cref="HighImpactEncoding"/> to use.</param>
        /// <returns>A string.</returns>
        private static string ReadString(DataReader reader, HighImpactEncoding encoding)
        {
            List<byte> stringBytes = new List<byte>();
            byte readByte;
            do
            {
                readByte = reader.ReadByte();
                stringBytes.Add(readByte);
            }
            while (readByte != 0x00);

            return encoding.GetString(stringBytes.ToArray());
        }
    }
}
