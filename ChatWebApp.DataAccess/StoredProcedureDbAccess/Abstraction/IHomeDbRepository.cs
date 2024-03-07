
using ChatWebApp.Models;
using System;
using System.Collections.Generic;

namespace ChatWebApp.DataAccess.StoredProcedureDbAccess.Abstraction
{
    public interface IHomeDbRepository
    {
        List<UserListWithLastMessageModel> GetUserList(Guid gid);
        List<UserListWithLastMessageModel> GetUserMessages(getUserMessage getUserMessage);
        void InsertMessage(MessageModel messageModel);
    }
}
