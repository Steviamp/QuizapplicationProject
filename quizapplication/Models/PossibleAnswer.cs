using System.Text.Json.Serialization;

namespace quizapplication.Models
{
        public class PossibleAnswer
        {
            [JsonPropertyName("a_id")]
            public int A_Id { get; set; }

            [JsonPropertyName("caption")]
            public string? Caption { get; set; }
        }
}
