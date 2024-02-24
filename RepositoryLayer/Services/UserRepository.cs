using CommonLayer.RequestModel;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly FundoContext context;
        public UserRepository(FundoContext context)
        {
            this.context = context;
        }
        public User UserRegistration(RegisterModel model)
        {
            User entity = new User();
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Email = model.Email;
            entity.Password = model.Password;
            context.UserTable.Add(entity);
            context.SaveChanges();
            return entity;
        }
        public User UserLogin(Login model)
        {
            try
            {
                var user = context.UserTable.FirstOrDefault(u => u.Email == model.Email);
                if (user != null)
                {
                    if (user.Password == model.Password)
                    {
                        return user;
                    }
                    else
                    {
                        throw new Exception("Incorrect password");
                    }
                }
                else
                {
                    throw new Exception("User not found");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed: {ex.Message} ");
                return null;
            }
        }
    }
}
