let token = Cookies.get('token');
if (token) {
	axios.defaults.headers.common = { Authorization: `bearer ${token}` };
}
