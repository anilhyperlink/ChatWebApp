using ChatWebApp.DataAccess.StoredProcedureDbAccess.Abstraction;
using ChatWebApp.Models;
using ChatWebApp.Services.Abstraction;
using System;
using System.Collections.Generic;

namespace ChatWebApp.Services.Repository
{
    public class HomeHelper : IHomeHelper
    {
        private readonly IHomeDbRepository _homeDbRepository;

        public HomeHelper(IHomeDbRepository homeDbRepository)
        {
            _homeDbRepository = homeDbRepository;
        }
        public List<UserListWithLastMessageModel> GetUserList(Guid gid)
        {
            return _homeDbRepository.GetUserList(gid);
        }

        public List<UserListWithLastMessageModel> GetUserMessages(getUserMessage getUserMessage)
        {
            return _homeDbRepository.GetUserMessages(getUserMessage);
        }

        public void InsertMessage(MessageModel messageModel)
        {
            _homeDbRepository.InsertMessage(messageModel);
        }
    }
}
