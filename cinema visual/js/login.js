let url = 'https://localhost:44315';

document.getElementById('loginBtn').addEventListener('click', function (e) {
	const login = document.getElementById('login').value;
	const password = document.getElementById('password').value;
	axios.post(url + '/LogIn?username=' + login + '&password=' + password).then(function (response) {
		console.log(response.data);
		const token = response.data.access_token;
		const email = response.data.username;
		const role = response.data.role;
		Cookies.set('token', token);
		Cookies.set('email', email);
		Cookies.set('role', role);
		console.log(login);
		console.log(response.data);
		// location.href = 'index.html';
	});
});

// 0) разобрать что сделали
// 1) стр регитр html
// 2) ниже 7 стр записывать токен в куки, записать токен под названием токен и перенаправить на главную стр(через location href)

// 3) cookie js подключить на стр логина и индекса через cdn(как подключаю бутстрап)  и посмотеть как работает(2 метода на установки и получение куки)
// 4)апи метод на регистрацию(создание аккаунта) в контроллере и попробовать подвязать кнопку на стр регистрации к методу

//зачем куки, если авторизация на основе токена
