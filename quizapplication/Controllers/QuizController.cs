using Microsoft.AspNetCore.Mvc;
using quizapplication.Services.Interfaces;

namespace quizapplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuiz()
        {
            var quiz = await _quizService.GetQuizAsync();
            return Ok(quiz);
        }

        [HttpGet("result")]
        public async Task<IActionResult> GetQuizResult()
        {
            var result = await _quizService.GetQuizResultAsync();
            return Ok(result);
        }
    }
}
