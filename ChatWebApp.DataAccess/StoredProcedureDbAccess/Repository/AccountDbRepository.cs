using ChatWebApp.DataAccess.StoredProcedureDbAccess.Abstraction;
using ChatWebApp.Models;
using ChatWebApp.WebedureDbAccess;
using System.Data;
using System;
using Dapper;
using ChatWebApp.Models.Comman;

namespace ChatWebApp.DataAccess.StoredProcedureDbAccess.Repository
{
    public class AccountDbRepository : SqlDbRepository<UserModel>, IAccountDbRepository
    {
        public AccountDbRepository(string connectionstring) : base(connectionstring) { }

        public UserModel SignIn(SignInModel signInModel)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@Email", signInModel.EmailAddress);
            vParams.Add("@Password", AppEncrypt.CreateHash(signInModel.Password));
            var userData = vconn.QueryFirstOrDefault<UserModel>("sp_proc_UserLogin", vParams, commandType: CommandType.StoredProcedure);
            return userData;
        }

        public void SignUp(SignUpModel signUpModel)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@FirstName", signUpModel.FirstName);
            vParams.Add("@LastName", signUpModel.LastName);
            vParams.Add("@Email", signUpModel.EmailAddress);
            vParams.Add("@Password", AppEncrypt.CreateHash(signUpModel.Password));
            vconn.Execute("sp_proc_AddUser", vParams, commandType: CommandType.StoredProcedure);
        }
    }
}
