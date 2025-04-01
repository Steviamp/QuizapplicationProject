using quizapplication.Models;
using quizapplication.Services.Interfaces;
using System.Text.Json;

namespace quizapplication.Services
{
    public class QuizService : IQuizService
    {
        private readonly string _quizFilePath;
        private readonly string _resultFilePath;

        public QuizService()
        {
            string baseDirectory = Directory.GetCurrentDirectory();
            _quizFilePath = Path.Combine(baseDirectory, "Data", "Quiz.json");
            _resultFilePath = Path.Combine(baseDirectory, "Data", "Result.json");
        }

        public async Task<Quiz> GetQuizAsync()
        {
            if (!File.Exists(_quizFilePath))
            {
                throw new FileNotFoundException($"Quiz file not found at: {_quizFilePath}");
            }

            string jsonContent = await File.ReadAllTextAsync(_quizFilePath);
            return JsonSerializer.Deserialize<Quiz>(jsonContent);
        }

        public async Task<QuizResult> GetQuizResultAsync()
        {
            if (!File.Exists(_resultFilePath))
            {
                throw new FileNotFoundException($"Result file not found at: {_resultFilePath}");
            }

            string jsonContent = await File.ReadAllTextAsync(_resultFilePath);
            return JsonSerializer.Deserialize<QuizResult>(jsonContent);
        }

        public async Task<PossibleResults> GetResultByScoreAsync(int totalPoints, int maxPoints)
        {
            var percentageScore = (double)totalPoints / maxPoints * 100;

            return await GetPossibleResultByPercentageAsync((int)percentageScore);
        }

        private async Task<PossibleResults> GetPossibleResultByPercentageAsync(int percentageScore)
        {
            var quizResult = await GetQuizResultAsync();

            return quizResult.Results.FirstOrDefault(r =>
            percentageScore >= r.MinPoints && percentageScore <= r.MaxPoints);
        }
    }
}
