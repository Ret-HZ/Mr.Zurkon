using System;
using System.Collections.Generic;
using System.Text;

namespace LOC_DATALib
{
    public class Localisation
    {
        public List<LocEntry> Entries = new List<LocEntry>();

        //Unknown metadata stuff
        byte[] unkMetadata;


        public Localisation()
        {

        }


        /// <summary>
        /// Adds an entry to the localisation. (NOTE: All localisations should have the same amount of entries)
        /// </summary>
        /// <param name="id">The text ID</param>
        /// <param name="text">The text</param>
        public void AddEntry(int id, string text)
        {
            LocEntry entry = new LocEntry(id, text);
            Entries.Add(entry);
        }


        public Int16 GetEntryAmount()
        {
            return (Int16)Entries.Count;
        }


        public LocEntry GetEntry(int index)
        {
            if (Entries.Count >= index) return Entries[index];
            else throw new Exception(); //TODO exception
        }


        public void SetText(int id, string text)
        {
            foreach (LocEntry entry in Entries)
            {
                if (entry.ID == id)
                {
                    entry.Text = text;
                    break;
                }
            }
        }
    }
}
