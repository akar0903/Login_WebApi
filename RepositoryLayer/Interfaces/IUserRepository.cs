using CommonLayer.RequestModel;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface IUserRepository
    {
        public User UserRegisteration(RegisterModel model);
        public string UserLogin(Login model);
        public string ForgotPassword(string email);
        public string GenerateToken(string email, int id);
        public bool ResetPassword(string Email, ResetPasswordModel resetPasswordModel);
        public bool IsEmailAlreadyRegistered(string email);
        public User IsUserThere(RegisterModel model, int id);
        public List<User> SearchUser(string name);
        public List<User> OddUser(int id);
    }
}