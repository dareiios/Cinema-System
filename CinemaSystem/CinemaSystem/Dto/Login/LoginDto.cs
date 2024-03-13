namespace CinemaSystem.Dto.Login
{
    public class LoginDto
    {
        public LoginDto(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; set; }
        public string Password { get; set; }

        
    }
}
