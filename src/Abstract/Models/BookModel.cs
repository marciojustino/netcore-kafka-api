namespace Abstract.Models
{
    using System;
    using System.Text.Json.Serialization;

    public class BookModel
    {
        [JsonPropertyName("id")]
        public Guid Id { get; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("publishedAt")]
        public DateTime? PublishedAt { get; set; }
        
        [JsonPropertyName("resume")]
        public string Resume { get; set; }

        public BookModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
