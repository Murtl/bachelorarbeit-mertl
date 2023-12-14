namespace fullstack.Typen
{
    // Eventklasse mit den benötigten Eigenschaften
    public class Event
    {
        public required string Id { get; set; }
        public required string Name { get; set; }

        public required string Date { get; set; }

        public required string Time { get; set; }

        public required string Description { get; set; }

        public List<string>? Participants { get; set; }
    }
}
