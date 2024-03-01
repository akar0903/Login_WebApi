using Common_layer.RequestModel;
using CommonLayer.RequestModel;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using User = RepositoryLayer.Entity.User;

namespace Repository_layer.Services
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
            entity.Password = Encryption("eergewweterg4tq3rewgq34t343g3tky", model.Password);
            User user = context.UserTable.FirstOrDefault(a => a.Email == model.Email);
            if (user != null)
            {
                throw new Exception("Email already exist");
            }
            else
            {
                context.UserTable.Add(entity);
                context.SaveChanges();
                return entity;
            }
        }
        public string UserLogin(Login model)
        {
            try
            {
                var user = context.UserTable.FirstOrDefault(a => a.Email == model.Email);
                string password = Decryption("eergewweterg4tq3rewgq34t343g3tky", user.Password);
                if (user != null)
                {
                    if (password == model.Password)
                    {
                        var token = GenerateToken(user.Email, user.Id);
                        return token;
                    }
                    else
                    {
                        throw new Exception("Incorrect password");
                    }
                }
                else
                {
                    throw new Exception("user not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login failed {ex.Message}");
                return null;
            }
        }
        public bool IsEmailAlreadyRegistered(string email)
        {
            return context.UserTable.Any(u => u.Email == email);
        }

        public string Encryption(string key, string Password)
        {
            byte[] Initial_Vector = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {

                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Initial_Vector;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(Password);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string Decryption(string key, string Password)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(Password);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }


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
                string token = GenerateToken(user.Email, user.Id);
                return token;
            }
            else
            {
                return null;
            }
        }
        public bool ResetPassword(string Email, ResetPasswordModel model)
        {
            User user = context.UserTable.ToList().Find(user => user.Email == Email);
            if (user != null)
            {
                user.Password = Encryption("eergewweterg4tq3rewgq34t343g3tky", model.ConfirmPassword);

                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}