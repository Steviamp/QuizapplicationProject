using quizapplication.Models;
using quizapplication.Services.Interfaces;

namespace quizapplication.Services
{
    public class AuthService : IAuthService
    {
        private readonly int[] validationNumbers = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150 };

        public Task<bool> ValidateLoginAsync(User user)
        {
            if (user == null)
                return Task.FromResult(false);

            if (string.IsNullOrEmpty(user.Username) ||
                user.Username.Length > 15 ||
                !user.Username.All(c => char.IsLower(c) && char.IsLetter(c)))
                return Task.FromResult(false);

            int vowels = user.Username.Count(c => "aeiou".Contains(c));
            if (vowels < 2)
                return Task.FromResult(false);

            if (user.Password < 100 || user.Password > 999)
                return Task.FromResult(false);

            int n = user.Username.Length;
            if (n > validationNumbers.Length)
                return Task.FromResult(false);

            int sum = validationNumbers.Take(n).Sum();
            return Task.FromResult(sum > user.Password);
        }
    }
}

