using ChatWebApp.Models;
using ChatWebApp.StoredProcedureDbAccess;

namespace ChatWebApp.DataAccess.StoredProcedureDbAccess.Abstraction
{
    public interface IAccountDbRepository : IGenericRepository<UserModel>
    {
        UserModel SignIn(SignInModel signInModel);
        void SignUp(SignUpModel signUpModel);
    }
}
