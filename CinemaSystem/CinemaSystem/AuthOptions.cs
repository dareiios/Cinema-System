using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CinemaSystem
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена. Издатель токена указывает, кто создал токен.
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена. для кого предназначен токен.
        const string KEY = "mysupersecret_secretkey!123";   // ключ для создания токена
        public const int LIFETIME = 1; // время жизни токена - 1 минута

        //возвращает объект SymmetricSecurityKey на основе ключа KEY. Этот объект используется для проверки подписи токена.
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
