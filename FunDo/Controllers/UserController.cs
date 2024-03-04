using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using Manager_Layer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Linq;
using System.Net.Mail;
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
        private readonly ILogger<UserController> logger;    
        public UserController(IUserManager userManager, IBus bus, FundoContext context1, ILogger<UserController> logger)
        {
            this.userManager = userManager;
            this.bus = bus;
            this.context1 = context1;
            this.logger = logger;
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
    }
}