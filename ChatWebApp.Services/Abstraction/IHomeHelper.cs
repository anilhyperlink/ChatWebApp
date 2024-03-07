using ChatWebApp.Models;
using System;
using System.Collections.Generic;

namespace ChatWebApp.Services.Abstraction
{
    public interface IHomeHelper
    {
        List<UserListWithLastMessageModel> GetUserList(Guid gid);
        List<UserListWithLastMessageModel> GetUserMessages(getUserMessage getUserMessage);
        void InsertMessage(MessageModel messageModel);
    }
}
