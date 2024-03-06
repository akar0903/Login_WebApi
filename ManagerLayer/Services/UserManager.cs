using CommonLayer.RequestModel;
using Manager_Layer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System.Collections.Generic;

namespace Manager_Layer.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository repository;
        public UserManager(IUserRepository repository)
        {
            this.repository = repository;
        }
        public User UserRegisteration(RegisterModel model)
        {
            return repository.UserRegisteration(model);
        }
        public string UserLogin(Login model)
        {
            return repository.UserLogin(model);
        }
        public string ForgotPassword(string email)
        {
            return repository.ForgotPassword(email);
        }
        public string GenerateToken(string Email, int Id)
        {
            return repository.GenerateToken(Email, Id);
        }
        public bool IsEmailAlreadyRegistered(string email)
        {
            return repository.IsEmailAlreadyRegistered(email);
        }
        public bool ResetPassword(string Email, ResetPasswordModel model)
        {
            return repository.ResetPassword(Email, model);
        }
        public User IsUserThere(RegisterModel model, int id)
        {
            return repository.IsUserThere(model, id);
        }
        public List<User> SearchUser(string name)
        {
            return repository.SearchUser(name);
        }
    }
}