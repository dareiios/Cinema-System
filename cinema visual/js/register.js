let url = 'https://localhost:44315';

document.getElementById('registerBtn').addEventListener('click', function (e) {
	const email = document.getElementById('email').value;
	const password = document.getElementById('password').value;
	const confirmPassword = document.getElementById('confirmPassword').value;
	axios
		.post(url + '/Register?email=' + email + '&password=' + password + '&confirmPassword=' + confirmPassword)
		.then(function (response) {
			console.log(response.data);

			const token = response.data.token;
			const email = response.data.email;
			Cookies.set('token', token);
			Cookies.set('email', email);
			location.href = 'index.html';
		});
});
