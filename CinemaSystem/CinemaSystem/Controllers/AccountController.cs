using CinemaSystem.Core;
using CinemaSystem.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CinemaSystem.Controllers
{
    public class AccountController : Controller
    {
        private CinemaSystemContext _context;
        public AccountController(CinemaSystemContext context)
        {
            _context = context;
        }

        [HttpPost("[action]")]
        public IActionResult Register(string email, string password, string confirmPassword)
        {
            if (!IsValidEmail(email))
            {
                return BadRequest(new { errorText = "Неверная почта" });
            }

            var checkEmail = _context.Userss.FirstOrDefault(x => x.Email == email);
            if (checkEmail != null)
            {
                return BadRequest(new { errorText = "Пользователь уже существует" });
            }

            if (!IsValidPassword(password))
            {
                return BadRequest(new { errorText = "пароли невалидный" });
            }

            if (password != confirmPassword)
            {
                return BadRequest(new { errorText = "пароли не совпадают" });

            }

            var roleId = _context.Roles.First(x=>x.Name=="User").Id;
            var newUser = new User()
            {
                Email = email,
                Password = password,
                RoleId = roleId
            };

            _context.Userss.Add(newUser);
            _context.SaveChanges();

            var identity = GetIdentity(newUser.Email, newUser.Password);
            var encodedJwt = GetToken(identity);

            var response = new
            {
                Email = newUser.Email,
                token = encodedJwt
            };

            return Ok(response);
        }

        //где тут входит пользователь
        [HttpPost("[action]")]
        public IActionResult LogIn(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var encodedJwt = GetToken(identity);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name,
                role = identity.Claims.Last().Value
            };

            return Json(response);
        }

        //Для поиска пользователя
        private ClaimsIdentity GetIdentity(string username, string password)
        {
            User person = _context.Userss.Include(x=>x.Role).FirstOrDefault(x => x.Email == username && x.Password == password);
            if (person != null)
            {
                //Объекты claim представляют некоторую информацию о пользователе, которую мы можем использовать для авторизации
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role.Name),

                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        private string GetToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;

            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,

            notBefore: now, //когда токен может быть использован(те сразу же(можно указать другое время))
            claims: identity.Claims,//в токен добавляются набор объектов Claim, которые содержат информацию о логине и роли пользователя
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),//время, когда токен перестанет быть действительным
                    //используется алгоритм HMAC-SHA256 для создания цифровой подписи токена. 
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt); //создается Json-представление токена
            return encodedJwt;

        }

        private bool IsValidEmail(string email)
        {
            string regex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
        }

        private bool IsValidPassword(string password)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            var hasLowerChar = new Regex(@"[a-z]+");
            return hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasLowerChar.IsMatch(password)
                && hasMinimum8Chars.IsMatch(password);
        }
    }
}
