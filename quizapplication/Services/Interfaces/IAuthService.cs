using quizapplication.Models;

namespace quizapplication.Services.Interfaces
{
    public interface IAuthService
    {
        Task<bool> ValidateLoginAsync(User user);
    }
}
