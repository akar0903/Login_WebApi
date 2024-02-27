using CommonLayer.RequestModel;
using ManagerLayer.Services;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using User = RepositoryLayer.Entity.User;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        public User UserRegistration(RegisterModel model);
        public User UserLogin(Login model);
        public string ForgotPassword(string email);
        public string GenerateToken(string email, int id);
    }
}