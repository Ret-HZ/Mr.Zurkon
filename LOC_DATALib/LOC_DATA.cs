using System;
using System.Collections.Generic;

namespace LOC_DATALib
{
    public class LOC_DATA
    {
        List<Localisation> Localisations = new List<Localisation>();

        //Unknown data
        public byte unk0x08;
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
    }
}
