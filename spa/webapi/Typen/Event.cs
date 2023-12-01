﻿namespace webapi.Typen
{
    public class Event
    {
        public required string Id { get; set; }
        public required string Name { get; set; }

        public required string Date { get; set; }

        public required string Time { get; set; }

        public required string Description { get; set; }

        public required List<string> Participants { get; set; }
    }
}
