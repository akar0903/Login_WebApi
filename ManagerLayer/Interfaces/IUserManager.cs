using Common_layer.RequestModel;
using CommonLayer.RequestModel;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manager_Layer.Interfaces
{
    public interface IUserManager
    {
        public User UserRegisteration(RegisterModel model);
        public string UserLogin(Login model);
        public string ForgotPassword(string email);
        public string GenerateToken(string Email, int Id);
        public bool IsEmailAlreadyRegistered(string email);
        public bool ResetPassword(string Email, ResetPasswordModel model);
        public User IsUserThere(RegisterModel model, int id);
        public List<User> SearchUser(string name);
    }
}