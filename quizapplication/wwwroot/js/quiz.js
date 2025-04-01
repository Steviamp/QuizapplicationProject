let quiz = null;
let quizResults = null;
let currentQuestionIndex = 0;
let userScore = 0;
let totalPoints = 0;

function initializeQuiz() {
    currentQuestionIndex = 0;
    userScore = 0;
    totalPoints = 0;

    fetchQuiz();
}

function fetchQuiz() {
    fetch('/api/quiz')
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch quiz data');
            }
            return response.json();
        })
        .then(data => {
            quiz = data;

            document.getElementById('quiz-title').textContent = quiz.title;
            document.getElementById('quiz-description').textContent = quiz.description;

            fetchQuizResults();

            displayQuestion(0);
        })
        .catch(error => {
            console.error('Error fetching quiz:', error);
            alert('Failed to load quiz. Please try again.');
        });
}

function fetchQuizResults() {
    fetch('/api/quiz/result')
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to fetch quiz results');
            }
            return response.json();
        })
        .then(data => {
            quizResults = data;
        })
        .catch(error => {
            console.error('Error fetching quiz results:', error);
        });
}

function displayQuestion(index) {
        if (!quiz || !quiz.questions || index >= quiz.questions.length) {
            console.error('Invalid question index or quiz not loaded');
            return;
        }

        const question = quiz.questions[index];
        console.log('Current question:', question);
        console.log('Possible answers:', question.possible_answers);

    document.getElementById('question-title').textContent = question.title;

    const questionImage = document.getElementById('question-image');
    questionImage.innerHTML = '';

    if (question.img && question.img !== '#####') {
        const img = document.createElement('img');
        img.src = question.img;
        img.alt = 'Question Image';
        questionImage.appendChild(img);
    }

    const answersContainer = document.getElementById('answers-container');
    answersContainer.innerHTML = '';

    if (question.question_type === 'truefalse') {
        // True/False question
        const trueOption = createAnswerOption('true', 'True', 'radio');
        const falseOption = createAnswerOption('false', 'False', 'radio');

        answersContainer.appendChild(trueOption);
        answersContainer.appendChild(falseOption);
    } else if (question.question_type === 'multiplechoice-single') {
        // Single choice question
        question.possible_answers.forEach(answer => {
            const option = createAnswerOption(
                answer.a_id.toString(),
                answer.caption,
                'radio'
            );
            answersContainer.appendChild(option);
        });
    } else if (question.question_type === 'multiplechoice-multiple') {
        // Multiple choice question
        question.possible_answers.forEach(answer => {
            const option = createAnswerOption(
                answer.a_id.toString(),
                answer.caption,
                'checkbox'
            );
            answersContainer.appendChild(option);
        });
    }

    const submitButton = document.getElementById('submit-answer');
    submitButton.onclick = () => submitAnswer(index);

    document.getElementById('question-container').classList.remove('hidden');
    document.getElementById('feedback-container').classList.add('hidden');
    document.getElementById('results-container').classList.add('hidden');
}

function createAnswerOption(value, text, type) {
    const div = document.createElement('div');
    div.className = 'answer-option';

    const input = document.createElement('input');
    input.type = type;
    input.name = type === 'radio' ? 'answer' : `answer-${value}`;
    input.value = value;
    input.id = `answer-${value}`;

    const label = document.createElement('label');
    label.htmlFor = `answer-${value}`;
    label.textContent = text;

    div.appendChild(input);
    div.appendChild(label);

    div.addEventListener('click', function () {
        if (type === 'radio') {
            document.querySelectorAll('.answer-option').forEach(option => {
                option.classList.remove('selected');
            });

            input.checked = true;
            div.classList.add('selected');
        }
        else {
            input.checked = !input.checked;
            div.classList.toggle('selected', input.checked);
        }
    });

    return div;
}

function submitAnswer(questionIndex) {
    const question = quiz.questions[questionIndex];
    const answersContainer = document.getElementById('answers-container');
    let isCorrect = false;
    let userAnswer;

    if (question.question_type === 'truefalse') {
        const selectedInput = answersContainer.querySelector('input[name="answer"]:checked');
        if (!selectedInput) {
            alert('Please select an answer');
            return;
        }

        userAnswer = selectedInput.value === 'true';
        isCorrect = userAnswer === question.correct_answer;
    }
    else if (question.question_type === 'multiplechoice-single') {
        const selectedInput = answersContainer.querySelector('input[name="answer"]:checked');
        if (!selectedInput) {
            alert('Please select an answer');
            return;
        }

        userAnswer = parseInt(selectedInput.value);
        isCorrect = userAnswer === question.correct_answer;
    }
    else if (question.question_type === 'multiplechoice-multiple') {
        const selectedInputs = answersContainer.querySelectorAll('input[type="checkbox"]:checked');
        if (selectedInputs.length === 0) {
            alert('Please select at least one answer');
            return;
        }

        userAnswer = Array.from(selectedInputs).map(input => parseInt(input.value));

        if (Array.isArray(question.correct_answer)) {
            isCorrect =
                userAnswer.length === question.correct_answer.length &&
                userAnswer.every(id => question.correct_answer.includes(id));
        }
    }

    if (isCorrect) {
        userScore += question.points;
    }

    totalPoints += question.points;

    showFeedback(isCorrect, question);

    document.getElementById('submit-answer').classList.add('hidden');

    highlightAnswers(question);

    setTimeout(() => {
        document.getElementById('submit-answer').classList.remove('hidden');

        if (currentQuestionIndex < quiz.questions.length - 1) {
            currentQuestionIndex++;
            displayQuestion(currentQuestionIndex);
        } else {
            showQuizResults();
        }
    }, 3000);
}

function showFeedback(isCorrect, question) {
    const feedbackContainer = document.getElementById('feedback-container');
    const feedbackText = document.getElementById('feedback-text');

    feedbackContainer.classList.remove('hidden', 'correct', 'incorrect');
    feedbackContainer.classList.add(isCorrect ? 'correct' : 'incorrect');

    feedbackText.textContent = isCorrect
        ? 'Correct! Well done.'
        : 'Incorrect. The correct answer is shown in green.';

    feedbackContainer.classList.remove('hidden');
}

function highlightAnswers(question) {
    const answerOptions = document.querySelectorAll('.answer-option');

    answerOptions.forEach(option => {
        const input = option.querySelector('input');
        const answerId = parseInt(input.value);

        // Handle single answer questions
        if (typeof question.correct_answer === 'number' || question.question_type === 'truefalse') {
            let isCorrect;

            if (question.question_type === 'truefalse') {
                isCorrect = (input.value === 'true' && question.correct_answer) ||
                    (input.value === 'false' && !question.correct_answer);
            } else {
                isCorrect = answerId === question.correct_answer;
            }

            if (isCorrect) {
                option.classList.add('correct');
            } else if (input.checked) {
                option.classList.add('incorrect');
            }
        }
        // Handle multiple answer questions
        else if (Array.isArray(question.correct_answer)) {
            if (question.correct_answer.includes(answerId)) {
                option.classList.add('correct');
            } else if (input.checked) {
                option.classList.add('incorrect');
            }
        }
    });
}

function showQuizResults() {
    document.getElementById('question-container').classList.add('hidden');
    document.getElementById('feedback-container').classList.add('hidden');

    const scorePercentage = Math.round((userScore / totalPoints) * 100);

    let result = null;
    for (const possibleResult of quizResults.results) {
        if (scorePercentage >= possibleResult.minpoints && scorePercentage <= possibleResult.maxpoints) {
            result = possibleResult;
            break;
        }
    }

    if (!result) {
        console.error('No matching result found for score', scorePercentage);
        return;
    }

    document.getElementById('result-title').textContent = result.title;
    document.getElementById('result-score').textContent = `Your score: ${userScore}/${totalPoints} (${scorePercentage}%)`;
    document.getElementById('result-message').textContent = result.message;

    const resultImage = document.getElementById('result-image');
    resultImage.innerHTML = '';

    if (result.img && result.img !== '#####') {
        const img = document.createElement('img');
        img.src = result.img;
        img.alt = 'Result Image';
        resultImage.appendChild(img);
    }

    document.getElementById('restart-button').onclick = initializeQuiz;

    document.getElementById('results-container').classList.remove('hidden');
}

document.addEventListener('DOMContentLoaded', function () {
    const restartButton = document.getElementById('restart-button');
    if (restartButton) {
        restartButton.addEventListener('click', initializeQuiz);
    }
});