using Automatonymous.Binders;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using ManagerLayer.Interfaces;
using MassTransit.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
namespace FunDo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private INoteManager manager;

        private readonly FundoContext context;
        public NoteController(INoteManager manager, FundoContext context)
        {
            this.manager = manager;
            this.context = context;
        }
        [Authorize]
        [HttpPost]
        [Route("api/[controller]/note-creation")]
        public ActionResult NoteCreation([FromBody] NoteCreationModel model)
        {
            int id = Convert.ToInt32(User.FindFirst("Id").Value);
            var response = manager.NoteCreation(model, id);
            if (response != null)
            {
                return Ok(new ResModel<NotesEntity> { Success = true, Message = "created successfull", Data = response });
            }
            else
            {
                return BadRequest(new ResModel<NotesEntity> { Success = false, Message = "creation failed", Data = response });
            }
        }
        [Authorize]
        [HttpGet]
        [Route("all")]
        public ActionResult All()
        {
            int id = Convert.ToInt32(User.FindFirst("Id").Value);
            List<NotesEntity> response = manager.Notes(id);
            if (response != null)
            {
                return Ok(new ResModel<List<NotesEntity>> { Success = true, Message = "Fetched Successfully", Data = response });
            }
            else
            {
                return BadRequest(new ResModel<List<NotesEntity>> { Success = true, Message = "Creation Failed", Data = response });
            }
        }
        [Authorize]
        [HttpPut]
        [Route("update")]
        public ActionResult Update(NoteCreationModel model, int noteId)
        {
            try
            {
                int id = Convert.ToInt32(User.FindFirst("Id").Value);
                var response = manager.UpdateNotes(id, model, noteId);
                if (response != null)
                {
                    return Ok(new ResModel<NotesEntity> { Success = true, Message = "Data Updated Successfully", Data = response });
                }
                else
                {
                    return BadRequest(new ResModel<NotesEntity> { Success = false, Message = "Data updation failure", Data = response });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResModel<NotesEntity> { Success = false, Message = ex.Message, Data = null });
            }
        }
        [Authorize]
        [HttpPut]
        [Route("Trash")]
        public ActionResult isTrash(int notesId)
        {
            try
            {
                int id = Convert.ToInt32(User.FindFirst("Id").Value);
                var response = manager.Istrash(id, notesId);
                if (response != null)
                {
                    return Ok(new ResModel<NotesEntity> { Success = true, Message = "added to Trash", Data = response });
                }
                return BadRequest(new ResModel<NotesEntity> { Success = false, Message = "failed to add in trash", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResModel<NotesEntity> { Success = false, Message = ex.Message, Data = null });
            }
        }
        [Authorize]
        [HttpDelete]
        [Route("Delete")]
        public ActionResult Delete(int notesId)
        {
            try
            {
                int id = Convert.ToInt32(User.FindFirst("Id").Value);
                var response = manager.Delete(id, notesId);
                if (response != null)
                {
                    return Ok(new ResModel<NotesEntity> { Success = true, Message = "Deleted Successfully", Data = response });
                }
                else
                {
                    return BadRequest(new ResModel<NotesEntity> { Success = false, Message = "Deletion failed", Data = response });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResModel<NotesEntity> { Success = false, Message = ex.Message, Data = null });
            }
        }
        [Authorize]
        [HttpPost]
        [Route("addColor")]
        public ActionResult Addcolor(string colour, int notesId) {
            try
            {
                var response = manager.Addcolor(colour, notesId);
                if (response != null)
                {
                    return Ok(new ResModel<NotesEntity> { Success = true, Message = "colour added successfully", Data = response });
                }
                else
                {
                    return BadRequest(new ResModel<NotesEntity> { Success = false, Message = "no colour added", Data = response });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResModel<NotesEntity> { Success = false, Message = ex.Message, Data = null });
            }
        }
        [Authorize]
        [HttpPost]
        [Route("archieve")]
        public ActionResult isArchieve(int notesId)
        {
            try
            {
                int id = Convert.ToInt32(User.FindFirst("Id").Value);
                var response = manager.IsArchieve(id, notesId);
                if (response != null)
                {
                    return Ok(new ResModel<NotesEntity> { Success = true, Message = "Archieved auccessfully", Data = response });
                }
                else
                {
                    return BadRequest(new ResModel<NotesEntity> { Success = false, Message = "Archieve failure", Data = response });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResModel<NotesEntity> { Success = false, Message = ex.Message, Data = null });
            }
        }
        [Authorize]
        [HttpPost]
        [Route("isPin")]
        public ActionResult IsPin(int notesId)
        {
            try
            {
                int userId = Convert.ToInt32(User.FindFirst("Id").Value);
                var response = manager.isPin(userId, notesId);
                if (response != null)
                {
                    return Ok(new ResModel<NotesEntity> { Success = true, Message = "Pin successfull", Data = response });
                }
                else
                {
                    return BadRequest(new ResModel<NotesEntity> { Success = false, Message = "Pin failed", Data = response });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResModel<NotesEntity> { Success = false, Message = ex.Message, Data = null });
            }
        }
        [Authorize]
        [HttpPut]
        [Route("AddRemainder")]
        public ActionResult AddRemainder(int notesId, DateTime time)
        {
            try
            {
                int userId = Convert.ToInt32(User.FindFirst("Id").Value);
                var response = manager.AddRemainder(notesId, time);
                if (response != null)
                {
                    return Ok(new ResModel<NotesEntity> { Success = true, Message = "Remainder Added", Data = response });
                }
                return BadRequest(new ResModel<NotesEntity> { Success = false, Message = "Remainder cannot be Added", Data = response });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResModel<NotesEntity> { Success = false, Message = ex.Message, Data = null });
            }
        }
        [Authorize]
        [HttpPut]
        [Route("Addimage")]
        public ActionResult Addimage(string fpath, int notesId)
        {

            int userId = Convert.ToInt32(User.FindFirst("Id").Value);
            var response = manager.UploadImage(fpath, notesId, userId);
            if (response != null)
            {
                return Ok(new ResModel<string> { Success = true, Message = "Image uploaded", Data = response });
            }
            return BadRequest(new ResModel<string> { Success = false, Message = "Image uploding failed", Data = response });
        }
        [Authorize]
        [HttpPost]
        [Route("api/[controller]/add-label")]
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
        [Authorize]
        [HttpPost]
        [Route("updatelabel")]
        public ActionResult UpdateLabel(int NoteID, string LabelName, string name)
        {
            try
            {
                int id = Convert.ToInt32(User.FindFirst("Id").Value);
                var response = manager.UpdateLabel(NoteID, name, LabelName);
                if (response != null)
                {
                    return Ok(new ResModel<UserLabel> { Success = true, Message = "Label updation successfull", Data = null });
                }
                else
                {
                    return BadRequest(new ResModel<UserLabel> { Success = false, Message = "Label updation failed", Data = null });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResModel<UserLabel> { Success = false, Message = ex.Message, Data = null });
            }
        }
        [Authorize]
        [HttpPost]
        [Route("getlabels")]
        public ActionResult GetLabel()
        {
            int id = Convert.ToInt32(User.FindFirst("Id").Value);
            List<UserLabel> response = manager.GetLabel(id);
            if (response != null)
            {
                return Ok(new ResModel<List<UserLabel>> { Success = true, Message = "Fetched Successfully", Data = response });
            }
            else
            {
                return BadRequest(new ResModel<List<UserLabel>> { Success = true, Message = "Creation Failed", Data = response });
            }

        }
        [Authorize]
        [HttpDelete]
        [Route("LabelDelete")]
        public ActionResult LabelDelete(int labelId)
        {
            try
            {
                int id = Convert.ToInt32(User.FindFirst("Id").Value);
                var response = manager.LabelDelete(labelId);
                if (response != null)
                {
                    return Ok(new ResModel<UserLabel> { Success = true, Message = "Deleted Successfully", Data = response });
                }
                else
                {
                    return BadRequest(new ResModel<UserLabel> { Success = false, Message = "Deletion failed", Data = response });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ResModel<UserLabel> { Success = false, Message = ex.Message, Data = null });
            }
        }

        [Authorize]
        [HttpGet]
        [Route("review")]
        public ActionResult GetNotesById(int noteId)
        {
            int id = Convert.ToInt32(User.FindFirst("Id").Value);
            String response = manager.GetNotesById(noteId);
            try
            {
                if (response != null)
                {
                    return Ok(new ResModel<String> { Success = true, Message = "fetch Successfully", Data = response });
                }
                else
                {
                    return BadRequest(new ResModel<String> { Success = false, Message = "fetch failed", Data = response });
                }
            }
            
            catch (Exception ex)
            {
                return BadRequest(new ResModel<String> { Success = false, Message = ex.Message, Data = null });
            }
        }
    }
        
       

    }



 
        

   


