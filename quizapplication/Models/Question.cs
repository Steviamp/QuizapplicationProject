using System.Text.Json.Serialization;

namespace quizapplication.Models
{
    public class Question
    {
        [JsonPropertyName("q_id")]
        public int Q_Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("img")]
        public string? Img { get; set; }

        [JsonPropertyName("question_type")]
        public string? QuestionType { get; set; }

        [JsonPropertyName("possible_answers")]
        public List<PossibleAnswer>? PossibleAnswers { get; set; }

        [JsonPropertyName("correct_answer")]
        public object? CorrectAnswer { get; set; }

        [JsonPropertyName("points")]
        public int Points { get; set; }
    }
}
