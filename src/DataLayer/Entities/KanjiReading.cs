namespace DataLayer.Entities
{
    public class KanjiReading
    {
        //This name is so that entityframework can use <principal entity type name><principal keyproperty name> to discover it as a foreign key
        public string KanjiNoteCardTopicName { get; set; }
        public string TypeOfReading { get; set; }
        public string Reading { get; set; }
    }
}
