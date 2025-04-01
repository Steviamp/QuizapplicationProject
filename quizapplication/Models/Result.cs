using System.Text.Json.Serialization;

namespace quizapplication.Models
{
    public class QuizResult
    {
        [JsonPropertyName("quiz_id")]
        public int QuizId { get; set; }

        [JsonPropertyName("results")]
        public List<PossibleResults> Results { get; set; }
    }

    public class PossibleResults
    {
        [JsonPropertyName("r_id")]
        public int RId { get; set; }

        [JsonPropertyName("minpoints")]
        public int MinPoints { get; set; }

        [JsonPropertyName("maxpoints")]
        public int MaxPoints { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("img")]
        public string Img { get; set; }
    }
}
