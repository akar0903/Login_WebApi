using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.RequestModel;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Migrations;
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
            entity.CreatedAt = DateTime.Now;
            entity.UpdatedAt = DateTime.Now;
            context.Note.Add(entity);
            context.SaveChanges();
            return entity;
        }
        public List<NotesEntity> Notes(int id)
        {
            return context.Note.Where(x => x.NoteId == id).ToList();
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
    }
}
