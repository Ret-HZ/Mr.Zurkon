using System;
using System.Collections.Generic;
using System.Text;

namespace BoltUtils.LOC_DATA
{
    public class LOC_DATA
    {
        List<Localisation> Localisations = new List<Localisation>();

        //Unknown data
        public int Version;
        public byte[] unkSCOL;


        public LOC_DATA()
        {

        }


        public void AddLocalisation(Localisation localisation)
        {
            Localisations.Add(localisation);
        }


        public byte GetLocalisationAmount()
        {
            return (byte)Localisations.Count;
        }


        public Int16 GetLocalisationEntryAmount()
        {
            if (Localisations.Count > 0)
            {
                return Localisations[0].GetEntryAmount();
            }
            return 0;
        }


        public Localisation GetLocalisation(int index)
        {
            if (Localisations.Count >= index) return Localisations[index];
            else throw new Exception(); //TODO Add proper exceptions
        }


        public string GetLocalisationDataAsCSV(int index)
        {
            Localisation loc = GetLocalisation(index);

            var csv = new StringBuilder();

            for (int i = 0; i < loc.GetEntryAmount(); i++)
            {
                LocEntry entry = loc.GetEntry(i);
                var newLine = string.Format("{0},{1}", entry.ID, entry.Text.Replace("\n", "\\n")); //Some strings have newlines, we dont want those in the csv
                csv.AppendLine(newLine);
            }

            return csv.ToString();
        }
    }
}
