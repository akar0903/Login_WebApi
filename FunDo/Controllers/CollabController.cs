using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;

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
        public ActionResult AddCollab(int noteid, string email)
        {
            int id = Convert.ToInt32(User.FindFirst("Id").Value);
            var response = manager.AddCollab(noteid, email);
            if (response != null)
            {
                return Ok(new ResModel<CollabEntity> { Success = true, Message = "Collab Added", Data = response });
            }
            else
            {
                return BadRequest(new ResModel<CollabEntity> { Success = true, Message = "Not added", Data = response });
            }
        }
        [Authorize]
        [HttpDelete]
        [Route("RemoveCollab")]
        public ActionResult RemoveCollab(int noteId, string email)
        {
            try
            {
                int id = Convert.ToInt32(User.FindFirst("Id").Value);
                var response = manager.RemoveCollab(noteId, email);
                if (response != null)
                {
                    return Ok(new ResModel<CollabEntity> { Success = true, Message = "Deleted Successfully", Data = response });
                }
                else
                {
                    return BadRequest(new ResModel<CollabEntity> { Success = false, Message = "Deletion failed", Data = response });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResModel<CollabEntity> { Success = false, Message = ex.Message, Data = null });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("GetCollab")]
        public ActionResult GetAllCollab(int noteid,int collabid)
        {
            //int id = Convert.ToInt32(User.FindFirst("Id").Value);
            var response = manager.GetCollab(noteid,collabid);
            if (response != null)
            {
                return Ok(new ResModel<List<CollabEntity>> { Success = true, Message = "Fetched Successfully", Data = response });
            }
            else
            {
                return BadRequest(new ResModel<List<CollabEntity>> { Success = true, Message = "Creation Failed", Data = response });
            }
        }
        [Authorize]
        [HttpPut]
        [Route("TrashCollab")]
        public ActionResult TrashCollab(int noteid)
        {
            var response = manager.Trashcollab(noteid);
            if (response != null)
            {
                return Ok(new ResModel<CollabEntity> { Success = true, Message = "Deleted Successfully", Data = response });
            }
            else
            {
                return BadRequest(new ResModel<CollabEntity> { Success = true, Message = "Delete Failed", Data = response });
            }
        }
    }
}
