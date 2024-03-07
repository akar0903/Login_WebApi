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
    public class UserLabelController : ControllerBase
    {
        private readonly INoteManager manager;
        private readonly FundoContext context;
        public UserLabelController(INoteManager manager, FundoContext context)
        {
            this.manager = manager;
            this.context = context;
        }
        [Authorize]
        [HttpPost]
        [Route("Add")]
        public ActionResult AddLabel(int NoteID, string LabelName)
        {
            try
            {
                int id = Convert.ToInt32(User.FindFirst("Id").Value);
                var response = manager.AddLabel(id, NoteID, LabelName);
                if (response != null)
                {
                    return Ok(new ResModel<UserLabel> { Success = true, Message = "Label Added Successfull", Data = response });
                }
                else
                {
                    return BadRequest(new ResModel<UserLabel> { Success = false, Message = "Label Adding Failed", Data = null });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResModel<UserLabel> { Success = false, Message = ex.Message, Data = null });
            }

        }
    }
}
