using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using ManagerLayer.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System.Threading.Tasks;
using System;
using User = RepositoryLayer.Entity.User;
using System.Linq;

namespace FunDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly FundoContext context1;
        private readonly IBus bus;
        public UserController(IUserManager userManager, FundoContext context1, IBus bus)
        {
            this.userManager = userManager;
            this.context1 = context1;
            this.bus = bus;
        }
        [HttpPost]
        [Route("Reg")]
        public ActionResult Register(RegisterModel model)
        {
            var repsonse = userManager.UserRegistration(model);
            if (repsonse != null)
            {
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
            var response = userManager.UserLogin(model);
            if (response != null)
            {
                return Ok(new ResModel<User> { Success = true, Message = "register successfull", Data = response });
            }
            else
            {
                return BadRequest(new ResModel<User> { Success = false, Message = "Resgister failed", Data = response });
            }

        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<ActionResult> ForgotPassword(string Email)
        {
            Send send = new Send();
            var check = userManager.ForgotPassword(Email);
            var checkmail = context1.UserTable.FirstOrDefault(x => x.Email == Email);
            var token = userManager.GenerateToken(checkmail.Email, checkmail.Id);
            send.SendMail(Email, token);
            Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
            var endPoint = await bus.GetSendEndpoint(uri);
            await endPoint.Send(check);
            return Ok(new ResModel<string> { Success = true, Message = "mail sent", Data = token });

        }
        }
    }