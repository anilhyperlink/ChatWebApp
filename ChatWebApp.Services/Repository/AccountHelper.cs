using ChatWebApp.DataAccess.StoredProcedureDbAccess.Abstraction;
using ChatWebApp.Models;
using ChatWebApp.Services.Abstraction;
using System.Security.Claims;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;
using ChatWebApp.Models.Comman;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChatWebApp.Services.Repository
{
    public class AccountHelper : IAccountHelper
    {
        private readonly IAccountDbRepository _accountDbRepository;
        private readonly AppSettings _appSettings;
        public AccountHelper(IAccountDbRepository accountDbRepository, IOptions<AppSettings> appSettings)
        {
            _accountDbRepository = accountDbRepository;
            _appSettings = appSettings.Value;
        }

        public string GenerateToken(UserModel userData)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.JWTTokenGenKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.PrimarySid, userData.UserId.ToString()),
                    new Claim(ClaimTypes.Name, userData.FirstName + " " + userData.LastName),
                    new Claim(ClaimTypes.Email, userData.EmailAddress),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public UserModel SignIn(SignInModel signInModel)
        {
            return _accountDbRepository.SignIn(signInModel);
        }

        public void SignUp(SignUpModel signUpModel)
        {
            _accountDbRepository.SignUp(signUpModel);
        }
    }
}
