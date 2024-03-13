document.getElementById('logoutBtn').addEventListener('click', function (e) {
	Cookies.remove('token');
	Cookies.remove('email');
	Cookies.set('role', 'User');
	console.log(Cookies.get('role'));
});
