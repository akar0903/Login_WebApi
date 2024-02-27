using CommonLayer.RequestModel;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepository
    {
        public User UserRegistration(RegisterModel model);
        public User UserLogin(Login model);
        public string ForgotPassword(string email);
        public string GenerateToken(string email, int id);
    }
}