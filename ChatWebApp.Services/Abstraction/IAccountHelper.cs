using ChatWebApp.Models;

namespace ChatWebApp.Services.Abstraction
{
    public interface IAccountHelper
    {
        string GenerateToken(UserModel userData);
        UserModel SignIn(SignInModel signInModel);
        void SignUp(SignUpModel signUpModel);
    }
}
