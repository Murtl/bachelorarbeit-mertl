namespace webapi.Typen
{
    // Die Klasse Event repräsentiert ein Event mit seinen Eigenschaften
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
