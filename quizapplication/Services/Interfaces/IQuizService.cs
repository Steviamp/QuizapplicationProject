using quizapplication.Models;

namespace quizapplication.Services.Interfaces
{
    public interface IQuizService
    {
        Task<Quiz> GetQuizAsync();
        Task<QuizResult> GetQuizResultAsync();
        Task<PossibleResults> GetResultByScoreAsync(int totalPoints, int maxPoints);
    }
}
