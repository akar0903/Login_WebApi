using CommonLayer.RequestModel;
using Microsoft.Data.SqlClient.Server;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using BCrypt.Net;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Diagnostics.Eventing.Reader;
using CommonLayer.ResponseModel;
using RepositoryLayer.Interfaces;

namespace Repository_Layer.Services
{
    public class UserRepository : IUserRepository
    {

        private readonly FundoContext context;
        private readonly IConfiguration config;


        public UserRepository(FundoContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }

        public User UserRegisteration(RegisterModel model)
        {
            User entity = new User();
            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Email = model.Email;
            entity.Password = model.Password;

            entity.Password = Encrypt(model.Password);
            User user = context.UserTable.FirstOrDefault(u => u.Email == model.Email);
            if (user != null)
            {
                throw new Exception("User Already Exixts with same Email");
            }
            else
            {
                context.UserTable.Add(entity);
                context.SaveChanges();
                return entity;
            }
        }



        public string Encrypt(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty");
            }

            int saltLength = new Random().Next(10, 13);
            string generatedSalt = BCrypt.Net.BCrypt.GenerateSalt(saltLength);

            return BCrypt.Net.BCrypt.HashPassword(password, generatedSalt);
        }

        public bool Decrypt(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword))
            {
                return false;
            }


            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        public string UserLogin(Login model)
        {

            User user = context.UserTable.FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
            {
                throw new Exception("User Does not Exits ");
            }
            else if (user != null)
            {

                if (Decrypt(model.Password, user.Password))
                {
                    string token = GenerateToken(user.Email, user.Id);
                    return token;
                   
                }
                else
                {
                    throw new Exception("Invalid Password ");
                }

            }
            else
            {
                throw new Exception("Invalid EmailID ");
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
        public string ForgotPassword(string Email)
        {
            var user = context.UserTable.FirstOrDefault(u => u.Email == Email);
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
        public bool ResetPassword(string Email, ResetPasswordModel resetPasswordModel)
        {
            User User = context.UserTable.ToList().Find(user => user.Email == Email);

            if (User != null)
            {
                User.Password = Encrypt(resetPasswordModel.ConfirmPassword);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool IsEmailAlreadyRegistered(string email)
        {
            return context.UserTable.Any(u => u.Email == email);
        }
    }

}