namespace webapi.Typen
{
    public class Event
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string Description { get; set; }

        public List<string> Participants { get; set; }
    }
}
