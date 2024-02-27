using CommonLayer.RequestModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly FundoContext context;
        private readonly IConfiguration config;
        public UserRepository(FundoContext context,IConfiguration config)
        {
            this.context = context;
            this.config = config;
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
        public string GenerateToken(string Email, int Id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Email",Email),
                new Claim("Id",Id.ToString())
            };
            var token = new JwtSecurityToken(config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string ForgotPassword(string email)
        {
            var user = context.UserTable.FirstOrDefault(a => a.Email == email);
            if (user != null)
            {
                var token = GenerateToken(user.Email, user.Id);
                return token;
            }
            else
            {
                return null;
            }
        }
    }
}