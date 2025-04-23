namespace BoltUtils.LOC_DATA
{
    public class LocEntry
    {
        public int ID { get; set; }
        public string Text { get; set; }


        public LocEntry(int id, string text)
        {
            this.ID = id;
            this.Text = text;
        }
    }
}
