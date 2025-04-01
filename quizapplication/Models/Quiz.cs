using System.Text.Json.Serialization;

namespace quizapplication.Models
{
    public class Quiz
    {
        [JsonPropertyName("quiz_id")]
        public int QuizId { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("questions")]
        public List<Question>? Questions { get; set; }
    }
}
