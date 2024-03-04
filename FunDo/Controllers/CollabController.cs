using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;

namespace FunDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController : ControllerBase
    {
        private INoteManager manager;
        private readonly FundoContext context;
        public CollabController(INoteManager manager, FundoContext context)
        {
            this.manager = manager;
            this.context = context;
        }
        [Authorize]
        [HttpPost]
        [Route("AddCollab")]
        public ActionResult AddCollab(int noteid,string email,CollabModel model)
        {
            int id = Convert.ToInt32(User.FindFirst("Id").Value);
            var response = manager.AddCollab(noteid, email,model);
            if (response != null)
            {
                return Ok(new ResModel<CollabEntity> { Success = true, Message = "Collab Added", Data = response });
            }
            else
            {
                return BadRequest(new ResModel<CollabEntity> { Success = true, Message = "Not added", Data = response });
            }
        }

    }
}
