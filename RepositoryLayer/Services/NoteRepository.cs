using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.RequestModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;

namespace RepositoryLayer.Services
{
    public class NoteRepository:INoteRepository
    {
        private readonly FundoContext context;
        public NoteRepository(FundoContext context)
        {
            this.context = context;
        }
        public NotesEntity NoteCreation(NoteCreationModel model,int id)
        {
          
            NotesEntity entity = new NotesEntity();
            entity.Title = model.Title;
            entity.Description = model.Description;
            entity.Reminder = model.Reminder;
            entity.Colour = model.Colour;
            entity.Image = model.Image;
            entity.IsArchive = false;
            entity.IsPin = false;
            entity.IsTrash = false;
            entity.UserId = id;
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            context.Note.Add(entity);
            context.SaveChanges();
            return entity;
        }
        public List<NotesEntity> Notes( int id)
        {
            List<NotesEntity> notesEntities = context.Note.Where(x => x.UserId == id).ToList();
            return notesEntities;
        }
        public NotesEntity UpdateNotes(int userId, NoteCreationModel model, int NotesId)
        {
            var notesEntity = context.Note.FirstOrDefault(x => ((x.UserId == userId) && (x.NoteId == NotesId)));

            if (notesEntity == null)
            {
                throw new Exception("Notes did not found");
            }
            if (!string.IsNullOrEmpty(model.Title))
            {
                notesEntity.Title = model.Title;
            }
            if (!string.IsNullOrEmpty(model.Description))
            {
                notesEntity.Description = model.Description;
            }
            
            context.SaveChanges();
            return notesEntity;
        }
        public NotesEntity Istrash(int userId, int notesId)
        {
            var notesEntiy = context.Note.FirstOrDefault(x => ((x.UserId == userId) && (x.NoteId == notesId)));
            if (notesEntiy != null)
            {
                notesEntiy.IsTrash = true;
                context.SaveChanges();
            }
            return notesEntiy;
        }
        public NotesEntity Delete(int userId, int NotesId)
        {
            var notesEntity = context.Note.FirstOrDefault(x => ((x.UserId == userId) && (x.NoteId == NotesId)));
            if (notesEntity != null)
            {
                context.Note.Remove(notesEntity);
                context.SaveChanges();
            }
            return notesEntity;
            throw new Exception("notes did not found");
        }

        public NotesEntity Addcolor(string colour, int NotesId)
        {
            var notesEntity = context.Note.FirstOrDefault(x => x.NoteId == NotesId);
            if (notesEntity != null)
            {
                notesEntity.Colour = colour;
                context.Note.Update(notesEntity);
                context.SaveChanges();
            }
            return notesEntity;
        }
        public NotesEntity IsTrash(int userId, int notesId)
        {
            var notesEntity = context.Note.FirstOrDefault(x => x.NoteId == notesId);
            if(notesEntity != null)
            {
                if (notesEntity.IsArchive)
                {
                    notesEntity.IsArchive = false;
                }
                else
                {
                    notesEntity.IsArchive = true;
                }
                context.SaveChanges();
                return notesEntity;
            }
            throw new Exception("notes are not found");
        }
        public NotesEntity IsArchieve(int userId, int notesId)
        {
            var notesEntity = context.Note.FirstOrDefault(x => x.NoteId == notesId);
            if (notesEntity != null)
            {
                if (notesEntity.IsArchive)
                {
                    notesEntity.IsArchive = false;
                }
                else
                {
                    notesEntity.IsArchive = true;
                }
                context.SaveChanges();
                return notesEntity;
            }
            throw new Exception("notes did not found");
        }
        public NotesEntity isPin(int userId, int notesId)
        {
            var notesEntity = context.Note.FirstOrDefault(x => x.NoteId == notesId);
            if (notesEntity != null)
            {
                if (notesEntity.IsPin)
                {
                    notesEntity.IsPin = false;
                }
                else
                {
                    notesEntity.IsPin = true;
                }
                context.SaveChanges();
                return notesEntity;
            }
            throw new Exception("notes did not found");
        }
        public NotesEntity AddRemainder(int notesId, DateTime dateTime)
        {
            var notesEntity = context.Note.FirstOrDefault(x => x.NoteId == notesId);
            if (notesEntity != null)
            {
                notesEntity.Reminder = dateTime;
                context.Update(notesEntity);
                context.SaveChanges();
                return notesEntity;
            }
            throw new Exception("notes did not found");
        }
        public string UploadImage(string fpath, int notesId, int userId)
        {
            try
            {
                var notesEntityUserId = context.Note.Where(x => x.UserId == userId);
                if (notesEntityUserId != null)
                {
                    var notesEntityNoteId = notesEntityUserId.FirstOrDefault(x => x.NoteId == notesId);
                    if (notesEntityNoteId != null)
                    {
                        Account account = new Account("dia3hvdxc", "724524628225628", "X6Jm68BOifYnoUR6L3sM9ss1BnQ");
                        Cloudinary cloudinary = new Cloudinary(account);
                        ImageUploadParams uploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(fpath),
                            PublicId = notesEntityNoteId.Title
                        };
                        ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);
                        notesEntityNoteId.UpdatedAt = DateTime.Now;
                        notesEntityNoteId.Image = uploadResult.Url.ToString();
                        context.SaveChanges();
                        return "Image uploaded Successfully";
                    }
                    return null;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public UserLabel AddLabel(int UserID, int NoteID, string name)
        {
            var s = context.UserLabel.FirstOrDefault(l => ((l.UserId == UserID)&&(l.NoteId == NoteID) && (!string.IsNullOrEmpty(l.LabelName))));
            if (s == null)
            {
                UserLabel entity = new UserLabel();
                entity.UserId = UserID;
                entity.NoteId = NoteID;
                entity.LabelName = name;
                context.UserLabel.Add(entity);
                context.SaveChanges();
                return entity;
            }
            else
            {
                throw new Exception("Label already added to the Note");
            }
        }
        public UserLabel UpdateLabel(int NoteID,string name,string newname)
        {
            var UserLabel = context.UserLabel.FirstOrDefault(x => ((x.NoteId == NoteID)));
            if (UserLabel == null)
            {
                throw new Exception("Label not found"); 
            }
            else
            {
                UserLabel.LabelName = newname;
            }
            context.SaveChanges();
            return UserLabel;
        }
        public List<UserLabel> GetLabel(int id)
        {
            List<UserLabel> labelEntities = context.UserLabel.Where(x => x.NoteId == id).ToList();
            return labelEntities;
        }
        public UserLabel LabelDelete(int labelId)
        {
            var UserLabel = context.UserLabel.FirstOrDefault(x => ((x.LabelId == labelId)));
            if (UserLabel != null)
            {
                context.UserLabel.Remove(UserLabel);
                context.SaveChanges();
            }
            return UserLabel;
            throw new Exception("notes did not found");
        }
        public NotesEntity GetNotesById(int notesId)
        {
            var notesEntity = context.Note.FirstOrDefault(x => x.NoteId == notesId);
            if (notesEntity != null)
            {
                return notesEntity;
            }
            else
            {
                return null;
            }
        }
    }
}
