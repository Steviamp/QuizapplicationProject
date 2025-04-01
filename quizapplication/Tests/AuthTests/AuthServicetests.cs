using Microsoft.AspNetCore.Mvc;
using Moq;
using quizapplication.Controllers;
using quizapplication.Models;
using quizapplication.Services;
using quizapplication.Services.Interfaces;
using Xunit;

namespace quizapplication.Tests.AuthTests
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _authService = new AuthService();
        }

        [Fact]
        public async Task PasswordShouldNotBeLessThan100()
        {
            // Arrange
            var user = new User
            {
                Username = "stevi",
                Password = 99  
            };

            // Act
            var result = await _authService.ValidateLoginAsync(user);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task PasswordShouldNotBeGreaterThan999()
        {
            // Arrange
            var user = new User
            {
                Username = "stevaki",
                Password = 1000  
            };

            // Act
            var result = await _authService.ValidateLoginAsync(user);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UsernameShouldNotContainNumbers()
        {
            // Arrange
            var user = new User
            {
                Username = "stevi123",
                Password = 500
            };

            // Act
            var result = await _authService.ValidateLoginAsync(user);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UsernameShouldContainVowels()
        {
            // Arrange
            var user = new User
            {
                Username = "stv", 
                Password = 500
            };

            // Act
            var result = await _authService.ValidateLoginAsync(user);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UsernameShouldNotBeGreaterThan15()
        {
            // Arrange
            var user = new User
            {
                Username = "stevakistevakistevaki",
                Password = 500
            };

            // Act
            var result = await _authService.ValidateLoginAsync(user);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UserShouldNotBeNull()
        {
            // Act
            var result = await _authService.ValidateLoginAsync(null);

            // Assert
            Assert.False(result);
        }
    }

    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly Mock<IAuthService> _mockAuthService;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task ReturnsOkWhenLoginIsSuccessful()
        {
            // Arrange
            var user = new User { Username = "maria", Password = 123 };
            _mockAuthService.Setup(service => service.ValidateLoginAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.Login(user);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic value = okResult.Value;
            Assert.True((bool)value.success);
            Assert.Equal("Login successful", (string)value.message);
        }

        [Fact]
        public async Task ReturnsBadRequestWhenLoginIsInvalid()
        {
            // Arrange
            var user = new User { Username = "invalid", Password = 999 };
            _mockAuthService.Setup(service => service.ValidateLoginAsync(It.IsAny<User>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.Login(user);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            dynamic value = badRequestResult.Value;
            Assert.False((bool)value.success);
            Assert.Equal("Invalid username or password", (string)value.message);
        }
    }
}
