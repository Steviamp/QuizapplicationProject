document.addEventListener('DOMContentLoaded', function () {
    const loginForm = document.getElementById('login-form');
    const loginError = document.getElementById('login-error');

    if (loginForm) {
        loginForm.addEventListener('submit', function (e) {
            e.preventDefault();

            const username = document.getElementById('username').value.trim();
            const password = document.getElementById('password').value.trim();

            if (!isValidUsername(username)) {
                loginError.textContent = 'Username must contain only lowercase letters and have at least 2 vowels';
                return;
            }

            if (!isValidPassword(password)) {
                loginError.textContent = 'Password must be a number between 100 and 999';
                return;
            }

            loginError.textContent = '';

            loginUser(username, parseInt(password));
        });
    }

    function isValidUsername(username) {
        if (!/^[a-z]+$/.test(username)) return false;

        const vowelCount = (username.match(/[aeiou]/g) || []).length;
        return vowelCount >= 2;
    }

    function isValidPassword(password) {
        const num = parseInt(password);
        return !isNaN(num) && num >= 100 && num <= 999;
    }

    function loginUser(username, password) {
        fetch('/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                username: username,
                password: password
            })
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Login failed');
                }
                return response.json();
            })
            .then(data => {
                if (data.success) {
                    document.getElementById('login-container').classList.add('hidden');

                    document.getElementById('quiz-container').classList.remove('hidden');

                    initializeQuiz();
                } else {
                    loginError.textContent = data.message || 'Login failed. Please check your credentials.';
                }
            })
            .catch(error => {
                loginError.textContent = 'Login failed. Please try again.';
                console.error('Login error:', error);
            });
    }
});