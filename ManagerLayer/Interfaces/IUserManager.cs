using CommonLayer.RequestModel;
using ManagerLayer.Services;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        public User UserRegistration(RegisterModel model);
        public User UserLogin(Login model);
    }
}
