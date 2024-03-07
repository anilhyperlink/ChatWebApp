using ChatWebApp.DataAccess.StoredProcedureDbAccess.Abstraction;
using ChatWebApp.Models;
using ChatWebApp.Models.Comman;
using ChatWebApp.WebedureDbAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;

namespace ChatWebApp.DataAccess.StoredProcedureDbAccess.Repository
{
    public class HomeDbRepository : SqlDbRepository<UserModel>, IHomeDbRepository
    {
        public HomeDbRepository(string connectionstring) : base(connectionstring) { }

        public List<UserListWithLastMessageModel> GetUserList(Guid gid)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@UserId", gid);
            var userData = vconn.Query<UserListWithLastMessageModel>("sp_proc_GetUserListWithLastMessage", vParams, commandType: CommandType.StoredProcedure).ToList();
            return userData;
        }

        public List<UserListWithLastMessageModel> GetUserMessages(getUserMessage getUserMessage)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@sId", getUserMessage.SId);
            vParams.Add("@rId", getUserMessage.RId);
            var userData = vconn.Query<UserListWithLastMessageModel>("sp_proc_GetUserMessages", vParams, commandType: CommandType.StoredProcedure).ToList();
            return userData;
        }

        public void InsertMessage(MessageModel messageModel)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@SenderId", messageModel.SenderId);
            vParams.Add("@ReceiverId", messageModel.ReceiverId);
            vParams.Add("@MessageText", messageModel.MessageText);
            vParams.Add("@GroupName", messageModel.GroupName);
            vconn.Execute("sp_proc_InsertMessage", vParams, commandType: CommandType.StoredProcedure);
        }
    }
}
