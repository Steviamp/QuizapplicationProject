# Quiz Application

A web-based quiz application with questions, user authentication, and score tracking built with ASP.NET Core and vanilla JavaScript.

## Features

- **User Authentication**: Simple login system with custom validation rules
- **Dynamic Quiz Loading**: Loads quiz content from JSON files
- **Multiple Question Types**:
  - Multiple choice (single answer)
  - Multiple choice (multiple answers)
  - True/False questions
- **Score Tracking**: Calculates user scores and provides feedback
- **Result Categories**: Different result messages based on score performance
- **Interactive UI**: Clean and responsive interface with visual feedback
- **Containerization**: Docker support for easy deployment

## Technologies

- **Backend**: ASP.NET Core 9.0 
- **Frontend**: HTML, CSS, Vanilla JavaScript
- **Data Storage**: JSON files for quiz content and results
- **Testing**: xUnit for unit testing
- **Containerization**: Docker

## Project Structure

```
quizapplication/
├── Controllers/            # API controllers
├── Data/                   # JSON data files
├── Models/                 # C# model classes
├── Services/               # Business logic services
│   └── Interfaces/         # Service interfaces
├── Tests/                  # xUnit test files
│   ├── AuthTests/
│   └── QuizTests/
├── wwwroot/                # Static files
│   ├── css/                # Stylesheets
│   ├── js/                 # JavaScript files
│   └── index.html          # Main HTML file
├── Dockerfile              # Docker configuration
├── docker-compose.yml      # Docker Compose configuration
└── quizapplication.csproj  # Project file
```

## Getting Started

### Prerequisites

- .NET 9.0 SDK or later
- Docker (optional, for containerized deployment)

### Local Development Setup

1. Clone the repository
   ```bash
   git clone https://github.com/Steviamp/quizapplication.git
   cd quizapplication
   ```

2. Restore packages and build the project
   ```bash
   dotnet restore
   dotnet build
   ```

3. Run the application
   ```bash
   dotnet run
   ```

4. Access the application at http://localhost:5223

### Docker Setup

1. Build and run with Docker Compose
   ```bash
   docker-compose up --build
   ```

2. Access the application at https://localhost:8080

## Quiz Configuration

The application loads quiz content from JSON files located in the `Data` directory:

- `Quiz.json`: Contains questions, answers, and point values
- `Result.json`: Contains result categories based on score thresholds

```

## Authentication System

The application uses a simple authentication system where:

- Usernames must contain only lowercase letters
- Usernames must have at least 2 vowels
- Passwords must be numeric and between 100-999

## Running Tests

```bash
dotnet test
```