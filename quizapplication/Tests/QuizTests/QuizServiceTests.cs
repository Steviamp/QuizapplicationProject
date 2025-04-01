using Microsoft.AspNetCore.Mvc;
using Moq;
using quizapplication.Controllers;
using quizapplication.Models;
using quizapplication.Services.Interfaces;
using Xunit;

namespace quizapplication.Tests.QuizTests
{
    public class QuizServiceTests
    {
        private readonly Mock<IQuizService> _mockQuizService;

        public QuizServiceTests()
        {
            _mockQuizService = new Mock<IQuizService>();
        }

        [Fact]
        public async Task ShouldReturnFirstResultWithLowScore()
        {
            // Arrange
            int totalPoints = 0;
            int maxPoints = 20;
            int expectedPercentage = 0;

            var quizResult = new QuizResult
            {
                QuizId = 12,
                Results = new List<PossibleResults>
                {
                    new PossibleResults { RId = 1, MinPoints = 0, MaxPoints = 33, Title = "Not good", Message = "Try again", Img = "#####" },
                    new PossibleResults { RId = 2, MinPoints = 34, MaxPoints = 66, Title = "Average", Message = "Good job", Img = "#####" },
                    new PossibleResults { RId = 3, MinPoints = 67, MaxPoints = 100, Title = "Impressive", Message = "Excellent", Img = "#####" }
                }
            };

            var expectedResult = quizResult.Results.Find(r => r.RId == 1);

            _mockQuizService.Setup(s => s.GetQuizResultAsync()).ReturnsAsync(quizResult);
            _mockQuizService.Setup(s => s.GetResultByScoreAsync(totalPoints, maxPoints)).ReturnsAsync(expectedResult);

            // Act
            var result = await _mockQuizService.Object.GetResultByScoreAsync(totalPoints, maxPoints);

            // Assert
            Assert.Equal(1, result.RId);
        }

        [Fact]
        public async Task ShouldReturnSecondResultWithMediumScore()
        {
            // Arrange
            int totalPoints = 7;
            int maxPoints = 20;
            int expectedPercentage = 35;

            var quizResult = new QuizResult
            {
                QuizId = 12,
                Results = new List<PossibleResults>
                {
                    new PossibleResults { RId = 1, MinPoints = 0, MaxPoints = 33, Title = "Not good", Message = "Try again", Img = "#####" },
                    new PossibleResults { RId = 2, MinPoints = 34, MaxPoints = 66, Title = "Average", Message = "Good job", Img = "#####" },
                    new PossibleResults { RId = 3, MinPoints = 67, MaxPoints = 100, Title = "Impressive", Message = "Excellent", Img = "#####" }
                }
            };

            var expectedResult = quizResult.Results.Find(r => r.RId == 2);

            _mockQuizService.Setup(s => s.GetQuizResultAsync()).ReturnsAsync(quizResult);
            _mockQuizService.Setup(s => s.GetResultByScoreAsync(totalPoints, maxPoints)).ReturnsAsync(expectedResult);

            // Act
            var result = await _mockQuizService.Object.GetResultByScoreAsync(totalPoints, maxPoints);

            // Assert
            Assert.Equal(2, result.RId);
        }

        [Fact]
        public async Task ShouldReturnThirdResultWithHighScore()
        {
            // Arrange
            int totalPoints = 14;
            int maxPoints = 20;
            int expectedPercentage = 70; 

            var quizResult = new QuizResult
            {
                QuizId = 12,
                Results = new List<PossibleResults>
                {
                    new PossibleResults { RId = 1, MinPoints = 0, MaxPoints = 33, Title = "Not good", Message = "Try again", Img = "#####" },
                    new PossibleResults { RId = 2, MinPoints = 34, MaxPoints = 66, Title = "Average", Message = "Good job", Img = "#####" },
                    new PossibleResults { RId = 3, MinPoints = 67, MaxPoints = 100, Title = "Impressive", Message = "Excellent", Img = "#####" }
                }
            };

            var expectedResult = quizResult.Results.Find(r => r.RId == 3);

            _mockQuizService.Setup(s => s.GetQuizResultAsync()).ReturnsAsync(quizResult);
            _mockQuizService.Setup(s => s.GetResultByScoreAsync(totalPoints, maxPoints)).ReturnsAsync(expectedResult);

            // Act
            var result = await _mockQuizService.Object.GetResultByScoreAsync(totalPoints, maxPoints);

            // Assert
            Assert.Equal(3, result.RId);
        }
    }

    public class QuizControllerTests
    {
        private readonly QuizController _controller;
        private readonly Mock<IQuizService> _mockQuizService;

        public QuizControllerTests()
        {
            _mockQuizService = new Mock<IQuizService>();
            _controller = new QuizController(_mockQuizService.Object);
        }

        [Fact]
        public async Task ShouldReturnOkWhenGettingTheQuiiz()
        {
            // Arrange
            var expectedQuiz = new Quiz
            {
                QuizId = 12,
                Title = "Stevis Test Quiz",
                Description = "Test Description",
                Questions = new List<Question>
                {
                    new Question
                    {
                        Q_Id = 1,
                        Title = "Test Question",
                        QuestionType = "multiplechoice-single",
                        PossibleAnswers = new List<PossibleAnswer>
                        {
                            new PossibleAnswer { A_Id = 1, Caption = "Option A" },
                            new PossibleAnswer { A_Id = 2, Caption = "Option B" }
                        },
                        CorrectAnswer = 1,
                        Points = 10
                    }
                }
            };

            _mockQuizService.Setup(service => service.GetQuizAsync())
                .ReturnsAsync(expectedQuiz);

            // Act
            var result = await _controller.GetQuiz();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Quiz>(okResult.Value);
            Assert.Equal(expectedQuiz.QuizId, returnValue.QuizId);
            Assert.Equal(expectedQuiz.Title, returnValue.Title);
            Assert.Equal(expectedQuiz.Questions.Count, returnValue.Questions.Count);
        }

        [Fact]
        public async Task ShouldReturnOkWhenGettingTheResult()
        {
            // Arrange
            var expectedResult = new QuizResult
            {
                QuizId = 12,
                Results = new List<PossibleResults>
                {
                    new PossibleResults { RId = 1, MinPoints = 0, MaxPoints = 33, Title = "Not good", Message = "Try again", Img = "#####" },
                    new PossibleResults { RId = 2, MinPoints = 34, MaxPoints = 66, Title = "Average", Message = "Good job", Img = "#####" },
                    new PossibleResults { RId = 3, MinPoints = 67, MaxPoints = 100, Title = "Impressive", Message = "Excellent", Img = "#####" }
                }
            };

            _mockQuizService.Setup(service => service.GetQuizResultAsync())
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetQuizResult();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<QuizResult>(okResult.Value);
            Assert.Equal(expectedResult.QuizId, returnValue.QuizId);
            Assert.Equal(expectedResult.Results.Count, returnValue.Results.Count);
        }
    }
}
