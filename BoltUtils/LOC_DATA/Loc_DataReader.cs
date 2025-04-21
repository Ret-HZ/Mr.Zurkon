using System;
using System.Collections.Generic;
using System.Text;
using Yarhl.IO;

namespace BoltUtils.LOC_DATA
{
    public class Loc_DataReader
    {
        public static LOC_DATA ReadLOC_DATA(string path)
        {
            using (var stream = DataStreamFactory.FromFile(path, FileOpenMode.Read))
            {
                var reader = new DataReader(stream)
                {
                    Endianness = EndiannessMode.LittleEndian,
                };


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
                        string text = ReadStringAndConvertSpecialCharacters(reader);

                        localisation.AddEntry(id, text);
                        reader.Stream.PopPosition();
                    }

                    //TODO: Metadata
                    locdata.AddLocalisation(localisation);
                }


                return locdata;
            }
        }



        private static string ReadStringAndConvertSpecialCharacters(DataReader reader)
        {
            List<byte> stringBytes = new List<byte>();
            byte readByte;
            do
            {
                readByte = reader.ReadByte();
                stringBytes.Add(readByte);
            } while (readByte != 0x00);


            string hexStr = Util.ByteArrayToString(stringBytes.ToArray());
            foreach (KeyValuePair<string, string> kvp in SpecialCharacters.CodeToTextTable)
            {
                int index = -1;
                do
                {
                    index = hexStr.IndexOf(kvp.Key, index+1);
                    if (index != -1 && index%2 == 0)
                    {
                        hexStr = hexStr.Remove(index, kvp.Key.Length).Insert(index, Util.TextStringToHexString(kvp.Value));
                    }
                } while (index != -1);

            }

            return Encoding.GetEncoding(28591).GetString(Util.StringToByteArray(hexStr));
        }
    }
}
