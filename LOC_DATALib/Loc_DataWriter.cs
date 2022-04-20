using System;
using System.Collections.Generic;
using System.Text;
using Yarhl.IO;

namespace LOC_DATALib
{
    public class Loc_DataWriter
    {
        public static void WriteLOC_DATA(string path, LOC_DATA locdata)
        {
            using (var stream = DataStreamFactory.FromFile(path, FileOpenMode.Write))
            {
                var writer = new DataWriter(stream);

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
                        writer.Write(localisation.GetEntry(locentryindex).TextToBytes()); //TODO
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
        }
    }
}
