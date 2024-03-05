using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using GreenPipes.Caching;
using Manager_Layer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace FunDoNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly FundoContext context1;
        private readonly IBus bus;
        private readonly IDistributedCache _cache;
        private readonly ILogger<UserController> logger;    
       
        public UserController(IUserManager userManager, IBus bus, FundoContext context1, ILogger<UserController> logger, IDistributedCache _cache)
        {
            this.userManager = userManager;
            this.bus = bus;
            this.context1 = context1;
            this.logger = logger;
            this._cache = _cache;
        }
        [HttpPost]
        [Route("Reg")]
        public ActionResult Register(RegisterModel model)
        {
            var repsonse = userManager.UserRegisteration(model);
            if (repsonse != null)
            {
                logger.LogInformation("Register Successful");
                return Ok(new ResModel<User> { Success = true, Message = "register successfull", Data = repsonse });
            }
            else
            {
                return BadRequest(new ResModel<User> { Success = false, Message = "Resgister failed", Data = repsonse });
            }
        }
        [HttpPost]
        [Route("Log")]
        public ActionResult Login(Login model)
        {
            try
            {
                string response = userManager.UserLogin(model);
                if (response != null)
                {
                    return Ok(new ResModel<string> { Success = true, Message = "Login successful", Data = response });
                }
                else
                {
                    return BadRequest(new ResModel<string> { Success = false, Message = "Login Failed", Data = response });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResModel<string> { Success = false, Message = ex.Message, Data = null });

            }
        }
        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword(string Email)
        {
            if (userManager.IsEmailAlreadyRegistered(Email))
            {
                Send send = new Send();
                var check = userManager.ForgotPassword(Email);
                var checkmail = context1.UserTable.FirstOrDefault(x => x.Email == Email);
                string token = userManager.GenerateToken(checkmail.Email, checkmail.Id);
                send.SendMail(Email, token);
                Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                var endPoint = await bus.GetSendEndpoint(uri);
                await endPoint.Send(check);
                return Ok(new ResModel<string> { Success = true, Message = "mail sent", Data = token });
            }
            else
            {
                return BadRequest(new ResModel<string> { Success = false, Message = "mail not sent", Data = Email });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("ResetPassword")]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            try
            {
                string Email = User.FindFirst("Email").Value;
                if (userManager.ResetPassword(Email, model))
                {
                    return Ok(new ResModel<bool> { Success = true, Message = "Password changed", Data = true });
                }
                else
                {
                    return BadRequest(new ResModel<bool> { Success = false, Message = "Password not changed", Data = false });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        [Route("GetAll/{Id}/{enableCache}")]
        public async Task<List<User>> GetAll(int Id)
        {
           
            string cacheKey = Id.ToString();

            // Trying to get data from the Redis cache
            byte[] cachedData = await _cache.GetAsync(cacheKey);
            List<User> articleMatrices = new List<User>();
            if (cachedData != null)
            {
                // If the data is found in the cache, encode and deserialize cached data.
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                articleMatrices = JsonSerializer.Deserialize<List<User>>(cachedDataString);
            }
            else
            {
                // If the data is not found in the cache, then fetch data from database
                articleMatrices = context1.UserTable.Where(x => x.Id == Id).OrderByDescending(x => x.FirstName).ToList();

                // Serializing the data
                string cachedDataString = JsonSerializer.Serialize(articleMatrices);
                var dataToCache = Encoding.UTF8.GetBytes(cachedDataString);

                // Setting up the cache options
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                // Add the data into the cache
                await _cache.SetAsync(cacheKey, dataToCache, options);
            }
            return articleMatrices;
        }
    }
}