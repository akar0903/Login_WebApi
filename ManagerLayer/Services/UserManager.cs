using CommonLayer.RequestModel;
using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using User = RepositoryLayer.Entity.User;
namespace ManagerLayer.Services
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository repository;
        public UserManager(IUserRepository repository)
        {
            this.repository = repository;
        }
        public User UserRegistration(RegisterModel model)
        {
            return repository.UserRegistration(model);
        }
        public User UserLogin(Login model)
        {
            return repository.UserLogin(model);
        }
        public string ForgotPassword(string email)
        {
            return repository.ForgotPassword(email);
        }
        public string GenerateToken(string email, int Id)
        {
            return repository.GenerateToken(email, Id);
        }
    }
}