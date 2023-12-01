namespace mpa.Models
{
    public class EventModel
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public required string Date { get; set; }

        public required string Time { get; set; }

        public required string Description { get; set; }

        public List<string>? Participants { get; set; }
    }
}
